using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace csharp_selenium_test
{
    class BasePage
    {
        private IWebDriver driver;
        protected WebDriverWait wait;
        
        public BasePage(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }
    }
}
