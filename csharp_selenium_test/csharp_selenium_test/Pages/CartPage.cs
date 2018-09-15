using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Threading;

namespace csharp_selenium_test
{
    class CartPage : BasePage
    {
        public CartPage(IWebDriver driver, WebDriverWait wait) : 
            base(driver, wait)
        {
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromMilliseconds(500)));
        }

        [FindsBy(How = How.CssSelector, Using = "button[name=remove_cart_item]")]
        private IList<IWebElement> removeButtons;

        [FindsBy(How = How.CssSelector, Using = "table[class^=dataTable] tr")]
        private IList<IWebElement> rows;

        public bool removeSomeProduct()
        {
            if (removeButtons.Count > 0)
            {
                int initialRows = rows.Count;
                removeButtons[0].Click();
                wait.Until(checkTableUpdated(initialRows));
            }

            if (rows.Count == 0)
                return false;
            else
                return true;
        }

        Func<IWebDriver, string> checkTableUpdated(int initialRows)
        {
            return (driver) =>
            {
                if (rows.Count == 0)
                    return "updated";
                else if (rows.Count == initialRows - 1)
                    return "update";
                else
                    return null;
            };
        }
    }
}
