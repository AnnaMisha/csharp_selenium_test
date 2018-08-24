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
    public class StickerMainPageTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        private static ReadOnlyCollection<IWebElement> getTilesList(IWebDriver driver)
        {
            return driver.FindElements(By.CssSelector(
                    "div[class=middle] > div[class=content] div[id^=box][class=box] li"));
        }

        private static ReadOnlyCollection<IWebElement> getStickersFromTileList(IWebElement tile)
        {
            return tile.FindElements(By.CssSelector(
                    "div[class^=sticker]"));
        }

        [SetUp]
        public void start()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
        }

        [Test]
        public void stickerMainPageTest()
        {
            driver.Url = "http://localhost/litecart/en/";

            wait.Until(ExpectedConditions.TitleIs("Online Store | My Store"));

            var tilesList = getTilesList(driver);
            int numberOfTiles = tilesList.Count;
            foreach(var tile in tilesList)
            {
                var stickersList = getStickersFromTileList(tile);
                Assert.AreEqual(stickersList.Count, 1);
            }

            Thread.Sleep(1000);
        }

        [TearDown]
        public void stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
