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

namespace csharp_selenium_test
{
    [TestFixture]
    public class GridTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            //ChromeOptions options = new ChromeOptions();
            InternetExplorerOptions options = new InternetExplorerOptions();
            
            //FirefoxOptions options = new FirefoxOptions();
            //options.AddArgument("start-maximized");

            driver = new RemoteWebDriver(new Uri("http://192.168.0.18:4444/wd/hub"), options);
            
            //driverCH = new ChromeDriver(options);
            //wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50);
        }

        [Test]
        public void gridTest()
        {
            driver.Url = "http://google.com";
            Thread.Sleep(15000);
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
