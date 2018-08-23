using System;
using System.Threading;
using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace csharp_selenium_test
{
    [TestFixture]
    public class AdminPageTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        private static ReadOnlyCollection<IWebElement> getItemsList(IWebDriver driver)
        {
            return driver.FindElements(By.CssSelector(
                    "ul[id = box-apps-menu][class = list-vertical] li[id = app-]"));
        }

        private static ReadOnlyCollection<IWebElement> getSubItemsList(IWebDriver driver)
        {
            return driver.FindElements(By.CssSelector(
                    "ul#box-apps-menu.list-vertical ul.docs li"));
        }

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
        }

        [Test]
        public void adminPageTest()
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

            var itemsList = getItemsList(driver);
            int numberOfItems = itemsList.Count;
            for (int i = 0; i < numberOfItems; i++)
            {
                itemsList[i].Click();
                var subItemsList = getSubItemsList(driver);
                int numberOfSubItems = subItemsList.Count;
                for (int j = 0; j < numberOfSubItems; j++)
                {
                    subItemsList[j].Click();

                    Assert.AreNotEqual(driver.FindElements(By.CssSelector("h1")).Count, 0);

                    subItemsList = getSubItemsList(driver);
                }
                itemsList = getItemsList(driver);
            }
            
            Thread.Sleep(1000);
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
