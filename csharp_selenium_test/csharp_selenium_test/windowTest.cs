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
    public class WindowTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        ReadOnlyCollection<IWebElement> getLinkList(IWebDriver driver)
        {
            return driver.FindElements(By.XPath(".//form/table[1]//a/i"));
        }

        void checkWindow(IWebDriver driver, IWebElement link)
        {
            string mainWindow = driver.CurrentWindowHandle;
            IList<string> oldWindows = driver.WindowHandles;
            link.Click();
            string newWindow = wait.Until(checkForNewWindowHandle(oldWindows));
            driver.SwitchTo().Window(newWindow);
            Thread.Sleep(200);
            driver.Close();
            driver.SwitchTo().Window(mainWindow);
        }

        Func<IWebDriver, string> checkForNewWindowHandle(IList<string> oldWindows)
        {
            return (driver) =>
            {
                IList<string> newWindowsReadOnly = driver.WindowHandles;
                List<string> newWindows = new List<string>();

                //copy handles to non-ReadOnly list
                foreach (var handle in newWindowsReadOnly)
                    newWindows.Add(handle);

                foreach (var handle in oldWindows)
                    newWindows.Remove(handle);

                if (newWindows.Count > 0)
                    return newWindows[0];
                else
                    return null;
            };
        }

        [SetUp]
        public void start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");

            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50);
            CountriesTest.loginToAdmin(driver, wait);
        }

        [Test]
        public void windowTest()
        {
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";
            wait.Until(ExpectedConditions.TitleContains("Countries | My Store"));
            var table = CountriesTest.getTable(driver, "form[name=countries_form] tr[class=row]", true, 0, 0);

            table[4][223].FindElement(By.CssSelector("a")).Click();
            wait.Until(ExpectedConditions.TitleContains("Edit Country | My Store"));

            var linkList = getLinkList(driver);
            foreach (var link in linkList)
            {
                checkWindow(driver, link);
                Thread.Sleep(200);
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


