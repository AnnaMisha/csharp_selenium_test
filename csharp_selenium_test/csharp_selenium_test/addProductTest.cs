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
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

namespace csharp_selenium_test
{
    [TestFixture]
    public class AddProductTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private static Random random = new Random(Guid.NewGuid().GetHashCode());

        static void login(IWebDriver driver, WebDriverWait wait)
        {
            driver.Url = "http://localhost/litecart/admin/login.php";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.TitleIs("My Store"));
        }

        [SetUp]
        public void start()
        {
            ChromeOptions options = new ChromeOptions();
            //options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            options.AddArgument("start-maximized");

            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            login(driver, wait);
        }

        [Test]
        public void addProductTest()
        {
            string catalogSelector = 
                "ul[id = box-apps-menu][class = list-vertical] li[id = app-]";
            driver.FindElements(By.CssSelector(catalogSelector))[1].Click();
            Thread.Sleep(500);
            driver.FindElements(By.CssSelector("a.button"))[1].Click();
            Thread.Sleep(500);
            driver.FindElements(By.CssSelector("input[name=status]"))[0].Click();
            string prod_name = "product#" + Convert.ToString(random.Next(0,10000000));
            driver.FindElement(By.CssSelector("input[name^=name]")).SendKeys(prod_name);
            driver.FindElement(By.CssSelector("input[name =code]")).SendKeys("12345");
            driver.FindElements(By.CssSelector("input[name^=categories]"))[1].Click();
            SelectElement defCategory = new SelectElement(
                driver.FindElement(By.CssSelector("select[name=default_category_id]")));
            defCategory.SelectByIndex(1);
            driver.FindElements(By.CssSelector("input[name^=product_groups]"))[2].Click();
            driver.FindElement(By.CssSelector("input[name =quantity]")).Clear();
            driver.FindElement(By.CssSelector("input[name =quantity]")).SendKeys("20");
            
            var binPath = System.AppDomain.CurrentDomain.BaseDirectory;
            var projPath = Path.GetFullPath(Path.Combine(binPath, "..\\..\\"));
            string picPath = projPath + "Img\\prod.png";
            driver.FindElement(By.CssSelector("input[type=file][name^=new_images]")).
                SendKeys(picPath);

            driver.FindElement(By.CssSelector("input[name=date_valid_from]")).
                SendKeys("09012018");
            driver.FindElement(By.CssSelector("input[name=date_valid_to")).
                SendKeys("09302018");

            driver.FindElements(By.CssSelector("ul.index li"))[1].Click();
            Thread.Sleep(500);

            SelectElement manufacturer = new SelectElement(
                driver.FindElement(By.CssSelector("select[name=manufacturer_id]")));
            manufacturer.SelectByIndex(1);
            driver.FindElement(By.CssSelector("input[name =keywords]")).SendKeys("some word");
            driver.FindElement(By.CssSelector("input[name^=short_description]")).SendKeys("short description");
            driver.FindElement(By.CssSelector("div[class=trumbowyg-editor]")).SendKeys("detailed product description");
            driver.FindElement(By.CssSelector("input[name ^= head_title]")).SendKeys("the best product ever");
            driver.FindElement(By.CssSelector("input[name ^= meta_description]")).SendKeys("the best product");

            driver.FindElements(By.CssSelector("ul.index li"))[3].Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("input[name=purchase_price]")).Clear();
            driver.FindElement(By.CssSelector("input[name=purchase_price]")).SendKeys("1");
            SelectElement currency = new SelectElement(
                driver.FindElement(By.CssSelector("select[name=purchase_price_currency_code]")));
            currency.SelectByIndex(1);

            var price = driver.FindElements(By.CssSelector("input[name^=prices]"));
            price[0].Clear();
            price[0].SendKeys("1");
            price[1].Clear();
            price[1].SendKeys("1");

            driver.FindElement(By.CssSelector("button[name=save]")).Click();
            Thread.Sleep(500);
            var prodTable = driver.FindElements(By.CssSelector("table[class=dataTable] tr.row"));
            bool isAdded = false;
            foreach (var row in prodTable)
            {
                string str = row.FindElements(By.CssSelector("td"))[2].GetAttribute("textContent");
                if ((" " + prod_name) == row.FindElements(By.CssSelector("td"))[2].GetAttribute("textContent"))
                    isAdded = true;
            }
            Assert.AreEqual(isAdded, true);
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
