using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemoqaUITests
{
    [TestClass]
    public class AccountTests:TestBase
    {
        private const string ValidUserName = "Val";
        private const string ValidPassword = "123456";

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
        [Description("Verify updating account is saved")]
        public void TestEditProfilePage()
        {
            LoginPage loginPage = new LoginPage(Driver);
            ProfilePage profilePage = loginPage.Login(ValidUserName, ValidPassword);
            string newFisrtName = Guid.NewGuid().ToString();
            string newLastName = Guid.NewGuid().ToString();
            profilePage.EditProfile(newFisrtName, newLastName);
            loginPage = profilePage.Logout();
            profilePage = loginPage.Login(ValidUserName, ValidPassword);

            Assert.AreEqual(newFisrtName, profilePage.GetFirstName());
            Assert.AreEqual(newLastName, profilePage.GetLastName());
        }
    }
}
