using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoqaUITests
{
    [TestClass]
    public class OrderTests:TestBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Initialize();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            CleanUp();
        }

        [TestMethod, Priority(1)]
        [Description("Submit an order for an Apple iPhone4s 16GB SIM-Free – Black")]
        public void TestSubmitIPhone16GBBlackOrder()
        {
            ProductPage iphoneOrder = new ProductPage(Driver, IPhone16GBBlackOrderPageUrl);
            float productPrice = iphoneOrder.GetProductPrice();

            CheckoutPage checkoutPage = iphoneOrder.AddToCartAndCheckout();
            CheckoutPage checkoutInfoPage = checkoutPage.Checkout();
            checkoutInfoPage.CalculateTotalShippingPrice("USA");
            Assert.AreEqual(productPrice + checkoutInfoPage.TotalShippingPrice, checkoutInfoPage.TotalPrice);
        }

    }
}
