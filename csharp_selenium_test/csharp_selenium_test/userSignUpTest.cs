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

namespace csharp_selenium_test
{
    class Customer
    {
        private static Random random = new Random(Guid.NewGuid().GetHashCode());

        public string firstName = RandomString(8);
        public string lastName = RandomString(8);
        public string address = RandomNumString(4) + " " + RandomString(10) + "St";
        public string zip = RandomNumString(5);
        public string city = RandomString(8);
        public string email = RandomString(8) + "@mail.com";
        public string phone = "+1" + RandomNumString(10);
        public string password = "123456789";
        public string country = "United States";
        public int stateIndex = random.Next(0, 49);

        static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static string RandomNumString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
    [TestFixture]
    public class userSignUpTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;        

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
        }

        [Test]
        public void userSignUp()
        {
            driver.Url = "http://localhost/litecart/en/create_account";
            wait.Until(ExpectedConditions.TitleIs("Create Account | My Store"));
            Customer client = new Customer();

            driver.FindElement(By.CssSelector("input[name=firstname]")).SendKeys(client.firstName);
            driver.FindElement(By.CssSelector("input[name=lastname]")).SendKeys(client.lastName);
            driver.FindElement(By.CssSelector("input[name=address1]")).SendKeys(client.address);
            driver.FindElement(By.CssSelector("input[name=postcode]")).SendKeys(client.zip);
            driver.FindElement(By.CssSelector("input[name=city]")).SendKeys(client.city);
            driver.FindElement(By.CssSelector("input[name=email]")).SendKeys(client.email);
            driver.FindElement(By.CssSelector("input[name=phone]")).SendKeys(client.phone);
            driver.FindElement(By.CssSelector("input[name=password]")).SendKeys(client.password);
            driver.FindElement(By.CssSelector("input[name=confirmed_password]")).SendKeys(client.password);

            var select1 = driver.FindElement(By.CssSelector("select[name=country_code]"));
            SelectElement countrySelect = new SelectElement(select1);
            countrySelect.SelectByText(client.country);

            var select2 = driver.FindElement(By.CssSelector("select[name=zone_code]"));
            SelectElement zoneSelect = new SelectElement(select2);
            zoneSelect.SelectByIndex(client.stateIndex);

            driver.FindElement(By.CssSelector("button[name=create_account]")).Click();
            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));

            driver.FindElements(By.CssSelector("div[id=box-account] a"))[3].Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("input[name=email]")).SendKeys(client.email);
            driver.FindElement(By.CssSelector("input[name=password]")).SendKeys(client.password);
            driver.FindElement(By.CssSelector("button[name=login]")).Click();

            driver.FindElements(By.CssSelector("div[id=box-account] a"))[3].Click();
            Thread.Sleep(2000);
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
