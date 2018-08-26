using System;
using System.Threading;
using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace csharp_selenium_test
{
    [TestFixture]
    public class StyleTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [Test]
        public void styleTestForChrome()
        {
            //start the browser
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50);

            //test
            styleTest(driver, wait);

            //stop the browser
            driver.Quit();
            driver = null;
        }

        [Test]
        public void styleTestForIE()
        {
            //start the browser
            driver = new InternetExplorerDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50);

            //test
            styleTest(driver, wait);

            //stop the browser
            driver.Quit();
            driver = null;
        }

        [Test]
        public void styleTestForFireFox()
        {
            //start the browser
            driver = new FirefoxDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50);

            //test
            styleTest(driver, wait);

            //stop the browser
            driver.Quit();
            driver = null;
        }

        private static void styleTest(IWebDriver driver, WebDriverWait wait)
        {
            //main page
            driver.Url = "http://localhost/litecart/en/";
            var mainItem = driver.FindElements(By.CssSelector("div[id=box-campaigns] li"))[0];
            var reg_price_web_element = mainItem.FindElement(By.CssSelector("s[class = regular-price]"));
            var campaign_price_web_element = mainItem.FindElement(By.CssSelector("strong[class = campaign-price]"));

            string name_main_page =
                mainItem.FindElement(By.CssSelector("div[class = name]")).GetAttribute("textContent");
            string reg_price_main_page = reg_price_web_element.GetAttribute("textContent");
            string reg_price_font_style_main_page = reg_price_web_element.GetCssValue("text-decoration");
            var reg_price_color_main_page = ColorHelper.ParseColor(reg_price_web_element.GetCssValue("color"));
            var reg_price_size_main_page = reg_price_web_element.Size;

            string campaign_price_main_page = campaign_price_web_element.GetAttribute("textContent");
            string campaign_price_font_style_main_page = reg_price_web_element.GetCssValue("font-weight");
            var campaign_price_color_main_page = ColorHelper.ParseColor(campaign_price_web_element.GetCssValue("color"));
            var campaign_price_size_main_page = campaign_price_web_element.Size;

            //mainItem.Click(); левый клик не работает в IE
            driver.Url = mainItem.FindElement(By.CssSelector("a[class = link]")).GetAttribute("href");
            wait.Until(ExpectedConditions.TitleIs("Yellow Duck | Subcategory | Rubber Ducks | My Store"));
            reg_price_web_element =
                driver.FindElement(By.CssSelector("div[id=box-product] div[class=price-wrapper] s[class=regular-price]"));
            campaign_price_web_element =
                driver.FindElement(By.CssSelector("div[id=box-product] div[class=price-wrapper] strong[class=campaign-price]"));

            string name_prod_page =
                driver.FindElement(By.CssSelector("div[id=box-product] h1[class=title]")).GetAttribute("textContent");
            string reg_price_prod_page = reg_price_web_element.GetAttribute("textContent");
            string reg_price_font_style_prod_page = reg_price_web_element.GetCssValue("text-decoration");
            var reg_price_color_prod_page = ColorHelper.ParseColor(reg_price_web_element.GetCssValue("color"));
            var reg_price_size_prod_page = reg_price_web_element.Size;

            string campaign_price_prod_page = campaign_price_web_element.GetAttribute("textContent");
            string campaign_price_font_style_prod_page = campaign_price_web_element.GetCssValue("font-weight");
            var campaign_price_color_prod_page = ColorHelper.ParseColor(campaign_price_web_element.GetCssValue("color"));
            var campaign_price_size_prod_page = campaign_price_web_element.Size;

            //a
            Assert.AreEqual(name_main_page, name_prod_page);
            //b
            Assert.AreEqual(reg_price_main_page, reg_price_prod_page);
            Assert.AreEqual(campaign_price_main_page, campaign_price_prod_page);
            //c
            Assert.AreEqual(Regex.IsMatch(reg_price_font_style_main_page, "line-through"), true);
            Assert.AreEqual((reg_price_color_main_page.B == reg_price_color_main_page.G) &&
                (reg_price_color_main_page.B == reg_price_color_main_page.R), true);
            Assert.AreEqual(Regex.IsMatch(reg_price_font_style_prod_page, "line-through"), true);
            Assert.AreEqual((reg_price_color_prod_page.B == reg_price_color_prod_page.G) &&
                (reg_price_color_prod_page.B == reg_price_color_prod_page.R), true);
            //d
            Assert.AreEqual(Regex.IsMatch(campaign_price_font_style_main_page, "400"), true);
            Assert.AreEqual((campaign_price_color_main_page.B == 0) &&
                (campaign_price_color_main_page.G == 0) &&
                (campaign_price_color_main_page.R > 0), true);
            Assert.AreEqual(Regex.IsMatch(campaign_price_font_style_prod_page, "700"), true);
            Assert.AreEqual((campaign_price_color_prod_page.B == 0) &&
                (campaign_price_color_prod_page.G == 0) &&
                (campaign_price_color_prod_page.R > 0), true);
            //e
            Assert.AreEqual((reg_price_size_main_page.Height < campaign_price_size_main_page.Height) &&
                (reg_price_size_main_page.Width < campaign_price_size_main_page.Width), true);
            Assert.AreEqual((reg_price_size_prod_page.Height < campaign_price_size_prod_page.Height) &&
                (reg_price_size_prod_page.Width < campaign_price_size_prod_page.Width), true);
        }

    }

    public static class ColorHelper
    {
        public static Color ParseColor(string cssColor)
        {
            cssColor = cssColor.Trim();

            if (cssColor.StartsWith("#"))
            {
                return ColorTranslator.FromHtml(cssColor);
            }
            else if (cssColor.StartsWith("rgb")) //rgb or argb
            {
                int left = cssColor.IndexOf('(');
                int right = cssColor.IndexOf(')');

                if (left < 0 || right < 0)
                    throw new FormatException("rgba format error");
                string noBrackets = cssColor.Substring(left + 1, right - left - 1);

                string[] parts = noBrackets.Split(',');

                int r = int.Parse(parts[0], CultureInfo.InvariantCulture);
                int g = int.Parse(parts[1], CultureInfo.InvariantCulture);
                int b = int.Parse(parts[2], CultureInfo.InvariantCulture);

                if (parts.Length == 3)
                {
                    return Color.FromArgb(r, g, b);
                }
                else if (parts.Length == 4)
                {
                    float a = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    return Color.FromArgb((int)(a * 255), r, g, b);
                }
            }
            throw new FormatException("Not rgb, rgba or hexa color string");
        }
    }
}


