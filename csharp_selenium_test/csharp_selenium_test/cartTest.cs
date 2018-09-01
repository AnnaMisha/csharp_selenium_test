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
    public class CartTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        //main page
        By productsLocator = By.CssSelector("div[class=middle] > div[class=content] div[id^=box][class=box] li");
        //product page
        By addButtonLocator = By.CssSelector("button[name=add_cart_product]");
        By cartCounterLocator = By.CssSelector("div[id=cart] a[class=content] span[class=quantity]");
        By cartCheckoutLocator = By.CssSelector("div[id=cart] a[class=link]");
        By selectSizeLocator = By.CssSelector("select[name^=option]");
        By removeButtonLocator = By.CssSelector("button[name=remove_cart_item]");
        By orderTableLocator = By.CssSelector("table[class^=dataTable]");

        void goToMainPage()
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
        }

        void selectSizeIfApplicable()
        {
            var selectSizeElements = driver.FindElements(selectSizeLocator);
            if (selectSizeElements.Count > 0)
                (new SelectElement(selectSizeElements[0])).SelectByIndex(1);
        }

        IWebElement waitForAddButton()
        {
            return wait.Until(ExpectedConditions.ElementExists(addButtonLocator));
        }

        void selectFirstProduct()
        {
            var products = driver.FindElements(productsLocator);
            products[0].Click();
            wait.Until(ExpectedConditions.StalenessOf(products[0]));
        }

        void addProductToCart()
        {
            var addButton = waitForAddButton();
            var cartCounter = driver.FindElement(cartCounterLocator);
            var itemNumber = Convert.ToString(Convert.ToInt32(cartCounter.GetAttribute("textContent")) + 1);

            selectSizeIfApplicable();
            addButton.Click();
            wait.Until(driver => driver.FindElement(cartCounterLocator).GetAttribute("textContent")
                    .Equals(itemNumber));
        }

        void goToCart()
        {
            wait.Until(ExpectedConditions.ElementExists(cartCheckoutLocator)).Click();
        }

        bool removeProductFromCart()
        {
            var removeButtons = wait.Until(driver => driver.FindElements(removeButtonLocator));
            
            if (removeButtons.Count > 0)
            {
                var orderTable = driver.FindElement(orderTableLocator);
                removeButtons[0].Click();
                wait.Until(ExpectedConditions.StalenessOf(orderTable));
                return true;
            }
            else
                return false;
        }

        [SetUp]
        public void start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");

            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
        }

        [Test]
        public void cartTest()
        {
            for (int i = 0; i < 3; i++)
            {
                goToMainPage();
                selectFirstProduct();
                addProductToCart();
            }
            goToMainPage();

            goToCart();
            bool del = true;
            while (del)
                del = removeProductFromCart();

            Thread.Sleep(3000);
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}

