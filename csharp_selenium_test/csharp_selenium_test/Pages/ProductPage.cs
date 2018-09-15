using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Threading;

namespace csharp_selenium_test
{
    class ProductPage : BasePage
    {
        internal CartWidget cartWidget;

        public ProductPage(IWebDriver driver, WebDriverWait wait) : 
            base(driver, wait)
        {
            PageFactory.InitElements(
                this, new RetryingElementLocator(driver, 
                TimeSpan.FromMilliseconds(500)));
            cartWidget = new CartWidget(driver,wait);
        }

        [FindsBy(How = How.CssSelector, Using = "h1.title")]
        private IWebElement Name;

        [FindsBy(How = How.CssSelector, Using = "div.codes span")]
        private IWebElement Code;

        [FindsBy(How = How.CssSelector, Using = "div.information span.price")]
        private IWebElement Price;

        [FindsBy(How = How.CssSelector, Using = "div.stock-available span.value")]
        private IWebElement StockStatus;

        [FindsBy(How = How.CssSelector, Using = "div.stock-delivery span.value")]
        private IWebElement DeliveryStatus;

        [FindsBy(How = How.CssSelector, Using = "select[name^=option]")]
        private IList<IWebElement> Size;

        [FindsBy(How = How.CssSelector, Using = "input[name=quantity]")]
        private IWebElement Quantity;

        [FindsBy(How = How.CssSelector, Using = "button[name=add_cart_product]")]
        private IWebElement AddToCartButton;

        public string getProductName()
        {
            return Name.GetAttribute("textContent");
        }

        public string getProductCode()
        {
            return Code.GetAttribute("textContent");
        }

        public string getProductPrice()
        {
            return Price.GetAttribute("textContent");
        }
        
        public string getStockStatus()
        {
            return StockStatus.GetAttribute("textContent");
        }

        public string getDeliveryStatus()
        {
            return DeliveryStatus.GetAttribute("textContent");
        }

        private void selectSize(int sizeIndex)
        {
            if (Size.Count > 0)
                (new SelectElement(Size[0])).SelectByIndex(sizeIndex);
        }

        private void setQuantity(int quantity)
        {
            string strQuantity = Convert.ToString(quantity);
            Quantity.Clear();
            Quantity.SendKeys(strQuantity);
        }

        public void addProductToCart(int quantity, int size)
        {
            selectSize(size);
            setQuantity(quantity);
            int initialItemsNumber = cartWidget.itemsNumber();
            AddToCartButton.Click();
            wait.Until(driver => cartWidget.itemsNumber()
            .Equals(initialItemsNumber + quantity));
        }
    }
}

