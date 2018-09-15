using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Threading;

namespace csharp_selenium_test
{
    class LeftNavigationPanel : BasePage
    {
        public LeftNavigationPanel(IWebDriver driver, WebDriverWait wait) : 
            base(driver, wait)
        {
            //PageFactory.InitElements(driver, this);
            PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(50)));
        }

        [FindsBy(How = How.CssSelector, Using = "ul[id=box-apps-menu] li[id=app-]")]
        private IList<IWebElement> mainItems;

        [FindsBy(How = How.CssSelector, Using = "ul[class=docs] li[id^=doc]")]
        private IList<IWebElement> subItems;

        internal class BaseItem
        {
            internal IList<IWebElement> subItems;
            internal BaseItem(IList<IWebElement> subItems)
            {
                this.subItems = subItems;
            }

            internal void clickByIndex(int index)
            {
                subItems[index].Click();
            }
        }

        internal class Appearence : BaseItem
        {
            internal Appearence(IList<IWebElement> subItems) : base(subItems) { }

            public void clickTemplate() { subItems[1].Click(); }

            public void clickLogotype() { subItems[1].Click(); }
        }

        public Appearence clickAppearence()
        {
            mainItems[0].Click();
            return new Appearence(subItems);
        }

        internal class Catalog : BaseItem
        {
            internal Catalog(IList<IWebElement> subItems) : base(subItems) { }

            public void clickCatalog() { subItems[0].Click(); }

            public void clickProductGroups() { subItems[1].Click(); }

            public void clickOptionGroups() { subItems[2].Click(); }

            public void clickManufacturers() { subItems[3].Click(); }

            public void clickSuppliers() { subItems[4].Click(); }

            public void clickDeliveryStatuses() { subItems[5].Click(); }

            public void clickSoldOutStatuses() { subItems[6].Click(); }

            public void clickQuantityUnits() { subItems[7].Click(); }

            public void clickCVSImportExports() { subItems[8].Click(); }
        }

        public Catalog clickCatalog()
        {
            mainItems[1].Click();
            return new Catalog(subItems);
        }

        public void clickCountries()
        {
            mainItems[2].Click();
        }

        public void clickCurrencies()
        {
            mainItems[3].Click();
        }

        internal class Customers : BaseItem
        {
            internal Customers(IList<IWebElement> subItems) : base(subItems) { }

            public void clickCustomers() { subItems[0].Click(); }
            public void clickCSVImportExport() { subItems[1].Click(); }
            public void clickNewsletter() { subItems[2].Click(); }
        }

        public Customers clickCustomers()
        {
            mainItems[4].Click();
            return new Customers(subItems);
        }

        public void clickGeoZones()
        {
            mainItems[5].Click();
        }

        internal class Languages : BaseItem
        {
            internal Languages(IList<IWebElement> subItems) : base(subItems) { }

            public void clickLanguages() { subItems[0].Click(); }
            public void clickStorageEncoding() { subItems[1].Click(); }
        }

        public Languages clickLanguages()
        {
            mainItems[6].Click();
            return new Languages(subItems);
        }

        internal class Modules : BaseItem
        {
            internal Modules(IList<IWebElement> subItems) : base(subItems) { }

            public void clickBackgroundJobs() { subItems[0].Click(); }
            public void clickCustomer() { subItems[1].Click(); }
            public void clickShipping() { subItems[2].Click(); }
            public void clickPayment() { subItems[3].Click(); }
            public void clickOrderTotal() { subItems[4].Click(); }
            public void clickOrderSuccess() { subItems[5].Click(); }
            public void clickOrderAction() { subItems[6].Click(); }

        }

        public Modules clickModules()
        {
            mainItems[7].Click();
            return new Modules(subItems);
        }

        internal class Orders : BaseItem
        {
            internal Orders(IList<IWebElement> subItems) : base(subItems) { }

            public void clickOrders() { subItems[0].Click(); }
            public void clickOrderStatuses() { subItems[1].Click(); }
        }

        public Orders clickOrders()
        {
            mainItems[8].Click();
            return new Orders(subItems);
        }

        public void clickPages()
        {
            mainItems[9].Click();
        }
    }
}
