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
    internal class Application
    {
        private WebDriverWait wait;
        private EventFiringWebDriver driver;
        private MainPage mainPage;
        private ProductPage productPage;
        public CartPage cartPage;

        public Application()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");

            driver = new EventFiringWebDriver(new ChromeDriver(options));
            driver.FindingElement += (sender, e) => Console.WriteLine(e.FindMethod);
            driver.FindElementCompleted += (sender, e) => Console.WriteLine(e.FindMethod + " found");
            driver.ExceptionThrown += (sender, e) => Console.WriteLine(e.ThrownException);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            mainPage = new MainPage(driver, wait);
            productPage = new ProductPage(driver, wait);
            cartPage = new CartPage(driver, wait);
        }

        public void navigateToStore()
        {
            driver.Url = "http://localhost/litecart/en/";
            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));
        }

        public void navigateToCart()
        {
            if (driver.Title != "Online Store | My Store")
                navigateToStore();
            mainPage.cartWidget.checkout();
        }

        public void addProductToCart()
        {
            mainPage.clickMostPopularProductByIndex(0);
            productPage.addProductToCart(1, 1);
            navigateToStore();
        }

        public bool removeProductFromCart()
        {
            return cartPage.removeSomeProduct();
        }

        public void removeAllProductsFromCart()
        {
            bool del = true;
            while(del)
                del = cartPage.removeSomeProduct();
        }

        public int getCartItemsNumber()
        {
            return mainPage.cartWidget.itemsNumber();
        }

        public void quit()
        {
            driver.Quit();
        }
    }
}
