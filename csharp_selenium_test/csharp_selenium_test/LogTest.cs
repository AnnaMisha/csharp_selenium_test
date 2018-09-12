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
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;

namespace csharp_selenium_test
{
    [TestFixture]
    public class LogTest
    {
        private WebDriverWait wait;
        private EventFiringWebDriver driver;

        static void login(IWebDriver driver, WebDriverWait wait)
        {
            driver.Url = "http://localhost/litecart/admin/login.php";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
            wait.Until(ExpectedConditions.TitleIs("My Store"));
        }

        static void goToCatalog(IWebDriver driver, WebDriverWait wait)
        {
            driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";
            wait.Until(ExpectedConditions.TitleContains("Catalog"));
        }

        static void printOutBrowserLogs(IWebDriver driver)
        {
            var log = driver.Manage().Logs.GetLog("browser");
            Assert.AreEqual(log.Count, 0);
            foreach (LogEntry l in log)
                Console.WriteLine(l);
        }

        static IList<IWebElement> getTable(IWebDriver driver, WebDriverWait wait)
        {
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("table[class=dataTable] tr.row")));
            return driver.FindElements(By.CssSelector("table[class=dataTable] tr.row"));
        }

        [SetUp]
        public void start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");

            driver = new EventFiringWebDriver(new ChromeDriver(options));
            driver.FindingElement += (sender, e) => Console.WriteLine(e.FindMethod);
            driver.FindElementCompleted += (sender, e) => Console.WriteLine(e.FindMethod + " found");
            driver.ExceptionThrown += (sender, e) => Console.WriteLine(e.ThrownException);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            login(driver, wait);
        }

        [Test]
        public void logTest()
        {
            goToCatalog(driver, wait);
            var prodTable = getTable(driver, wait);
            int num = prodTable.Count;
            for (int i = 0; i < num; i++)
            {
                prodTable[i].
                    FindElements(By.CssSelector("td"))[2].
                    FindElement(By.CssSelector("a")).
                    Click();
                //Thread.Sleep(500);
                goToCatalog(driver, wait);
                prodTable = getTable(driver, wait);
                printOutBrowserLogs(driver);
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
