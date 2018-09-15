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
    public class ProxyTest
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

        static void printOutBrowserLogs(IWebDriver driver)
        {
            var log = driver.Manage().Logs.GetLog("browser");
            Assert.AreEqual(log.Count, 0);
            foreach (LogEntry l in log)
                Console.WriteLine(l);
        }

        [SetUp]
        public void start()
        {
            Proxy proxy = new Proxy();
            proxy.Kind = ProxyKind.Manual;
            proxy.HttpProxy = "localhost:8080";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            options.Proxy = proxy;

            driver = new EventFiringWebDriver(new ChromeDriver(options));
            driver.FindingElement += (sender, e) => Console.WriteLine(e.FindMethod);
            driver.FindElementCompleted += (sender, e) => Console.WriteLine(e.FindMethod + " found");
            driver.ExceptionThrown += (sender, e) => Console.WriteLine(e.ThrownException);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            login(driver, wait);
        }

        [Test]
        public void proxyTest()
        {

        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

