using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.IE;
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
            driver = new ChromeDriver();
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
