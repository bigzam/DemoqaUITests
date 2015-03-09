using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoqaUITests
{
    [TestClass]
    public class CartTests:TestBase
    {
        private const string EmptyCartMessage = "Oops, there is nothing in your cart.";   
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
        [Description("Add single item to the cart and remove it")]
        public void TestRemoveSingleItem()
        {
            ProductPage iPhonePage = new ProductPage(Driver, IPhone16GBBlackOrderPageUrl);
            RemoveItemsFromCart(iPhonePage.AddToCartAndCheckout());
        }

        [TestMethod, Priority(1)]
        [Description("Add 2 items to the cart and remove them")]
        public void TestRemove2Items()
        {
            ProductPage iPhone16Page = new ProductPage(Driver, IPhone16GBBlackOrderPageUrl);
            iPhone16Page.AddToCartAndContinueShopping();
            ProductPage iPhone32Page = new ProductPage(Driver, IPhone32GBWhiteOrderPageUrl);
            RemoveItemsFromCart(iPhone32Page.AddToCartAndCheckout());
        }

        private static int RemoveItemsFromCart(CheckoutPage checkoutPage)
        {
            int n = checkoutPage.RemoveAllItems();
            Assert.AreEqual(EmptyCartMessage, checkoutPage.GetCartMessage());
            return n;
        }

    }
}
