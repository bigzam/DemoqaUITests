using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace DemoqaUITests
{
    public abstract class PageBase
    {
        protected readonly IWebDriver Driver;

        protected PageBase(IWebDriver driver)
        {
            this.Driver = driver;
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        protected void WaitUntilElementIsVisible(By element)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(element));
        }

        protected string GetElementAttribute(By element, string attributeName)
        {
            return Driver.FindElement(element).GetAttribute(attributeName);
        }
    }

    public class ProductPage:PageBase
    {
        
        private readonly By addButton = By.ClassName("product_form_ajax");
        private readonly By goToCheckoutButton = By.ClassName("go_to_checkout");
        private readonly By continueShoppingButton = By.ClassName("continue_shopping");
        private readonly By productPrice = By.ClassName("currentprice");

        public ProductPage(IWebDriver driver, string productPageUrl):base(driver)
        {
            Driver.Url = productPageUrl;
        }

        public CheckoutPage AddToCartAndCheckout()
        {
            Driver.FindElement(addButton).Submit();
            Driver.FindElement(goToCheckoutButton).Click();
            return new CheckoutPage(Driver);
        }

        public ProductPage AddToCartAndContinueShopping()
        {
            Driver.FindElement(addButton).Submit();
            Driver.FindElement(continueShoppingButton).Click();
            return this;
        }

        public float GetProductPrice()
        {
            return TestBase.GetPriceFromString(Driver.FindElement(productPrice).Text);
        }
    }

    public class CheckoutPage : PageBase
    {
        private readonly By removeForm = By.ClassName("remove");
        
        private readonly By cartContent = By.ClassName("entry-content");
        
        private readonly By continueButton = By.ClassName("step2");

        private readonly By selectCountry= By.ClassName("current_country");

        private readonly By calculateButton = By.Name("wpsc_submit_zipcode");

        private readonly By totalPriceDisplay = By.XPath("//span[@class='pricedisplay checkout-total']");

        private readonly By shippingPriceDisplay = By.XPath("//span[@class='pricedisplay checkout-shipping']");

        public float TotalPrice { get; private set; }

        public float TotalShippingPrice { get; private set; }

        public CheckoutPage(IWebDriver driver):base(driver)
        {
            Driver.Url = "http://store.demoqa.com/products-page/checkout/";
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            if (!Driver.Title.Equals("Checkout | Online store", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidDataException("This is not iPhone order page");

            }
        }


        public int RemoveAllItems()
        {
            int cartProductsRowsCount = Driver.FindElements(removeForm).Count;
            for (int i = 0; i < cartProductsRowsCount; i++)
            {
                try
                {
                    Driver.FindElement(removeForm).Submit();
                }
                catch (NoSuchElementException)
                {
                    // do nothing if element not found
                }
            }
            return cartProductsRowsCount;
        }


        public string GetCartMessage()
        {
            return Driver.FindElement(cartContent).Text.Trim();
        }

        public CheckoutPage Checkout()
        {
            WaitUntilElementIsVisible(continueButton);
            Driver.FindElement(continueButton).Click();

            return this;
        }

        public CheckoutPage CalculateTotalShippingPrice(string country, string state="")
        {
            Driver.FindElement(selectCountry).SendKeys(country);
            Driver.FindElement(calculateButton).Click();

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(delegate { return !string.IsNullOrWhiteSpace(Driver.FindElement(totalPriceDisplay).Text); });

            TotalPrice = TestBase.GetPriceFromString(Driver.FindElement(totalPriceDisplay).Text.Replace("$", ""));
            TotalShippingPrice = TestBase.GetPriceFromString(Driver.FindElement(shippingPriceDisplay).Text.Replace("$", ""));

            return this;
        }
    }

    public class ProfilePage : PageBase
    {
        private readonly By firstNameFiled = By.Id("first_name");

        private readonly By lastNameFiled = By.Id("last_name");
        
        private readonly By updateProfileButton = By.Id("submit");

        private readonly By accountSubMenu = By.XPath("//li[@id='wp-admin-bar-my-account']/a[@class='ab-item']");

        private readonly By logOutCommand = By.XPath("//*[@id='wp-admin-bar-logout']");
        
        public ProfilePage(IWebDriver driver):base(driver)
        {

        }

        public ProfilePage EditProfile(string newFirstName, string newLastName)
        {
            Driver.FindElement(firstNameFiled).Clear();
            Driver.FindElement(firstNameFiled).SendKeys(newFirstName);
            Driver.FindElement(lastNameFiled).Clear();
            Driver.FindElement(lastNameFiled).SendKeys(newLastName);
            Driver.FindElement(updateProfileButton).Click(); 
            return this;
        }

        public LoginPage Logout()
        {
            Actions a = new Actions(Driver);
            a.MoveToElement(Driver.FindElement(accountSubMenu)).Build().Perform();
            WaitUntilElementIsVisible(logOutCommand);
            Driver.FindElement(logOutCommand).Click();
            return new LoginPage(Driver, true);
        }

        public string GetFirstName()
        {
            return GetElementAttribute(firstNameFiled, "value");
        }

        public string GetLastName()
        {
            return GetElementAttribute(lastNameFiled, "value");
        }
    }


    public class LoginPage : PageBase
    {
        private readonly By userNameField = By.Id("user_login");
        private readonly By userPasswordFiled = By.Id("user_pass");
        private readonly By submitButton= By.Id("wp-submit");

        public LoginPage(IWebDriver driver, bool loggedOut = false) : base(driver)
        {
            
            if (loggedOut)
            {
                Driver.Url = "http://store.demoqa.com/tools-qa/?loggedout=true";
            }
            else
            {
                Driver.Url = "http://store.demoqa.com/tools-qa/";
            }
        }

        public ProfilePage Login(string userName, string password)
        {
            Driver.FindElement(userNameField).Clear();
            Driver.FindElement(userNameField).SendKeys(userName);
            Driver.FindElement(userPasswordFiled).Clear();
            Driver.FindElement(userPasswordFiled).SendKeys(password);
            Driver.FindElement(submitButton).Click();

            return new ProfilePage(Driver);
        }
        
    }
}
