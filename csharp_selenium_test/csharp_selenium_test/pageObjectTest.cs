using System;
using System.Threading;
using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;

namespace csharp_selenium_test
{
    [TestFixture]
    public class PageObjectTest
    {
        Application app;

        [SetUp]
        public void start()
        {
            app = new Application();  
        }

        [Test]
        public void pageObjectTest()
        {
            app.navigateToStore();
            for (int i = 0; i < 3; i++)
                app.addProductToCart();
            app.navigateToCart();
            app.removeAllProductsFromCart();
            app.navigateToStore();
            Assert.AreEqual(app.getCartItemsNumber(), 0);
        }

        [TearDown]
        public void stop()
        {
            app.quit();
        }
    }
}
