using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DemoqaUITests
{
    public class TestBase
    {
        protected IWebDriver Driver;
        protected const string IPhone16GBBlackOrderPageUrl =
    "http://store.demoqa.com/products-page/product-category/iphones/apple-iphone-4s-16gb-sim-free-black/";

        protected const string IPhone32GBWhiteOrderPageUrl =
            "http://store.demoqa.com/products-page/product-category/iphones/apple-iphone-4s-32gb-sim-free-white/";

        public static float GetPriceFromString(string price)
        {
            return Convert.ToSingle(price.Replace("$", string.Empty));
        }

        public void Initialize()
        {
            Driver = new ChromeDriver();
        }

        public void CleanUp()
        {
            Driver.Dispose();
        }
    }
}
