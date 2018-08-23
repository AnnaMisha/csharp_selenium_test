using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace csharp_example
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void start()
        {
            //driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            //InternetExplorerOptions options = new InternetExplorerOptions();
            //options.RequireWindowFocus = true;
            //driver = new InternetExplorerDriver(options);

            //old scheme for Firefox
            FirefoxOptions options = new FirefoxOptions();
            options.UseLegacyImplementation = false;            
            options.BrowserExecutableLocation =
                @"c:\Program Files\Firefox Nightly\firefox.exe";
            driver = new FirefoxDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        [Test]
        public void loginTest()
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
