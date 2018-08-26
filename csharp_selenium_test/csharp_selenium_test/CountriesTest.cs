using System;
using System.Threading;
using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace csharp_selenium_test
{
    [TestFixture]
    public class CountriesTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        private static void loginToAdmin(IWebDriver driver, WebDriverWait wait)
        {
            driver.Url = "http://localhost/litecart/admin/login.php";
            try
            {
                driver.FindElement(By.Name("username")).SendKeys("admin");
                driver.FindElement(By.Name("password")).SendKeys("admin");
                driver.FindElement(By.Name("login")).Click();
            }
            catch
            {
                
            }
            wait.Until(ExpectedConditions.TitleIs("My Store"));
        }

        private static IWebElement[][] getTable(IWebDriver driver, string cssSelector, 
            bool transpose, int upperOffset, int lowerOffset)
        {
            IWebElement[][] result;
            IWebElement[][] countriesTable;
            var table = driver.FindElements(By.CssSelector(cssSelector));
            int numRows = table.Count - upperOffset - lowerOffset;
            int numCols = table[1].FindElements(By.CssSelector("td")).Count;
            countriesTable = new IWebElement[numRows][];
            for(int i = 0; i < numRows; i++)
            {
                countriesTable[i] = new IWebElement[numCols];
                var subTable = table[i + upperOffset].FindElements(By.CssSelector("td"));
                for (int j = 0; j < numCols; j++)
                {
                    countriesTable[i][j] = subTable[j];
                }
            }

            if (transpose)
            {
                IWebElement[][] countriesTableT;
                countriesTableT = new IWebElement[numCols][];
                for (int i = 0; i < numCols; i++)
                {
                    countriesTableT[i] = new IWebElement[numRows];
                    for (int j = 0; j < numRows; j++)
                        countriesTableT[i][j] = countriesTable[j][i];
                }
                result = countriesTableT;
            }
            else
                result = countriesTable;

            return result;
        }

        private static string[] extractProperty(IWebElement[] table, string property)
        {
            //extract countries column
            string[] countries = new string[table.Length];
            for(int i = 0; i < table.Length; i++)
            {
                countries[i] = table[i].GetAttribute(property);
            }
            return countries;
        }

        private static bool isSorted(string[] unsorted)
        {
            string[] sorted = new string[unsorted.Length];
            //clone array
            for (int i = 0; i < unsorted.Length; i++)
                sorted[unsorted.Length - i - 1] = unsorted[i];
            Array.Sort(sorted);

            bool check = true;
            for (int i = 0; i < unsorted.Length; i++)
                if (unsorted[i] != sorted[i])
                    check = false;
            return check;
        }

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50);
            loginToAdmin(driver, wait);
        }

        [Test]
        public void countriesSortingTest()
        {
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            var table = getTable(driver, "form[name=countries_form] tr[class=row]", true, 0, 0);
            //is sorted check
            var countries = extractProperty(table[4], "textContent");          
            Assert.AreEqual(isSorted(countries), true);

            //get a list of links for countries with zones
            for (int i = 0; i < table[0].Length; i++)
            {
                string value = table[5][i].GetAttribute("textContent");
                int numZones = Convert.ToInt32(value);
                if (numZones > 0)
                {
                    table[4][i].FindElement(By.TagName("a")).Click();
                    wait.Until(ExpectedConditions.TitleIs("Edit Country | My Store"));
                    //test zones
                    var zonesTable = getTable(driver, "table[id = table-zones] tr:not([class = header])", true, 0, 1);
                    var zones = extractProperty(zonesTable[2], "textContent");
                    Assert.AreEqual(isSorted(zones), true);
                    driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";

                    //refresh IDs
                    table = getTable(driver, "form[name=countries_form] tr[class=row]", true, 0, 1);
                }
            }
        }

        [Test]
        public void geoZonesSortingTest()
        {
            driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
            var table = getTable(driver, "table[class = dataTable] tr[class = row]", true, 0, 0);

            for (int i = 0; i < table[0].Length; i++)
            {
                table[2][i].FindElement(By.TagName("a")).Click();
                wait.Until(ExpectedConditions.TitleIs("Edit Geo Zone | My Store"));
                //test zones
                string css_selector =
                    "table[id = table-zones] tr:not([class = header])";
                //"table[id = table-zones] tr:not([class = header]) ";
                var geoZonesTable = getTable(driver,css_selector, true, 0, 1);
                int numOfGeoZones = geoZonesTable[2].Length;
                IWebElement[] extractedInfo = new IWebElement[numOfGeoZones];
                for(int j = 0; j < numOfGeoZones; j++)
                    extractedInfo[j] = 
                        geoZonesTable[2][j].FindElement(By.CssSelector("select[name *= zone_code] option[selected = selected]"));
                var geoZones = extractProperty(extractedInfo, "textContent");
                
                Assert.AreEqual(isSorted(geoZones), true);
                driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";
                //refresh IDs
                table = getTable(driver, "table[class = dataTable] tr[class = row]", true, 0, 0);
            }
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

