using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Threading;

namespace csharp_selenium_test
{
    class MainPage:BasePage
    {
        internal CartWidget cartWidget;

        public MainPage(IWebDriver driver, WebDriverWait wait) : 
            base(driver, wait)
        {
            PageFactory.InitElements(
                this, new RetryingElementLocator(driver, 
                TimeSpan.FromSeconds(50)));
            cartWidget = new CartWidget(driver, wait);
        }

        [FindsBy(How = How.CssSelector, Using = "div[id=slider-wrapper] li")]
        private IList<IWebElement> logoHeader;

        [FindsBy(How = How.CssSelector, Using = "div[id=box-logotypes] li")]
        private IList<IWebElement> Logo;

        [FindsBy(How = How.CssSelector, Using = "div[id=box-most-popular] li")]
        private IList<IWebElement> mostPopularProducts;

        [FindsBy(How = How.CssSelector, Using = "div[id=box-campaigns] li")]
        private IList<IWebElement> campaignsProducts;

        [FindsBy(How = How.CssSelector, Using = "div[id=box-latest-products] li")]
        private IList<IWebElement> latestProducts;

        public void clickMostPopularProductByIndex(int index)
        {
            if ((index >= 0) && (index < mostPopularProducts.Count))
                mostPopularProducts[index].Click();
        }

        public void clickCampaignsProductByIndex(int index)
        {
            if ((index >= 0) && (index < campaignsProducts.Count))
                campaignsProducts[index].Click();
        }

        public void clickLatestProductByIndex(int index)
        {
            if ((index >= 0) && (index < latestProducts.Count))
                latestProducts[index].Click();
        }
    }
}
