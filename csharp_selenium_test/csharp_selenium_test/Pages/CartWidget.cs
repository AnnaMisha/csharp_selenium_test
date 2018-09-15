using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Threading;

namespace csharp_selenium_test
{
    class CartWidget : BasePage
    {
        public CartWidget(IWebDriver driver, WebDriverWait wait) : 
            base(driver, wait)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(500)));
        }

        [FindsBy(How = How.CssSelector, Using = "div[id=cart] span.quantity")]
        private IWebElement quantity;

        [FindsBy(How = How.CssSelector, Using = "div[id=cart] a.link")]
        private IWebElement link;

        public int itemsNumber()
        {
            return Convert.ToInt32(quantity.GetAttribute("textContent"));
        }

        public void checkout()
        {
            link.Click();
        }

        public void waitUntil(int expectedValue)
        {
            string value = Convert.ToString(expectedValue);
            wait.Until((driver) => quantity.GetAttribute("textContent")
            .Equals(value));
        }
    }
}
