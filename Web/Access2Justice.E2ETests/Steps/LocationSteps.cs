/*
 * Controls are located in shared->map->map.component.html file 
 * How to use css selector: http://www.binaryclips.com/2016/03/css-selectors-for-selenium-webdriver.html
 * How to configure Chrome browser: http://chromedriver.chromium.org/capabilities
 * SpecFlow.Assist.Dynamic: https://github.com/marcusoftnet/SpecFlow.Assist.Dynamic/wiki/Table-to-dynamic-instance
 * Explicit Wait: http://toolsqa.com/selenium-webdriver/c-sharp/advance-explicit-webdriver-waits-in-c/
 * Implicit Wait, Explicit Wait or Fluent Wait: https://www.guru99.com/implicit-explicit-waits-selenium.html
 * Sample waiting code: https://gist.github.com/up1/d925783ea8e5f706f3bb58c3ce1ef7eb
 * Wait till element is clickable: https://stackoverflow.com/questions/16057031/webdriver-how-to-wait-until-the-element-is-clickable-in-webdriver-c-sharp
 */

using System;
using TechTalk.SpecFlow;
using SpecFlow.Assist.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;


namespace Access2Justice.E2ETests.Steps
{
    [Binding]
    public class LocationSteps
    {
        IWebDriver driver;
        private dynamic _inputStateName;
        private dynamic _displayedStateName;
        DefaultWait<IWebDriver> fluentWait;

        [Given(@"I am on the Access2Justice website with location detection blocked")]
        public void GivenIAmOnTheAccessJusticeWebsiteWithLocationDetectionBlocked()
        {
            ChromeOptions options = new ChromeOptions();
            // Start Chrome maximized
            options.AddArguments("start-maximized");
            // Disable the "Know your location" pop up
            options.AddUserProfilePreference("profile.managed_default_content_settings.geolocation", 2);
            driver = new ChromeDriver(options);
            driver.Url = "http://localhost:5150/";
            fluentWait = new DefaultWait<IWebDriver>(driver);
            fluentWait.Timeout = TimeSpan.FromSeconds(20);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(200);
            fluentWait.IgnoreExceptionTypes(typeof(WebDriverException));
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            fluentWait.IgnoreExceptionTypes(typeof(TimeoutException));
            fluentWait.IgnoreExceptionTypes(typeof(ElementNotVisibleException));
            fluentWait.Message = "Why :(((((((";
        }

        [Given(@"I am on the Access2Justice website with state set")]
        public void GivenIAmOnTheAccessJusticeWebsiteWithStateSet(dynamic instance)
        {
            GivenIAmOnTheAccessJusticeWebsiteWithLocationDetectionBlocked();
            GivenIAmPromptedToSetMyLocation();
            WhenIEnterMyStateName(instance);
            ThenICanSeeMyStateNameOnTheUpperNavigationBar(instance);
        }

        [Given(@"I am prompted to set my location")]
        public void GivenIAmPromptedToSetMyLocation()
        {
            Assert.IsTrue(driver.FindElement(By.TagName("modal-container")).Displayed);
        }

        [When(@"I click on the Change button")]
        public void WhenIClickOnTheChangeButton()
        {
            IWebElement changeButton;

            changeButton = fluentWait.Until(d =>
            {
                IWebElement element = d.FindElement(By.CssSelector("button[test-id=change-location-button]"));
                //IWebElement element2 = d.FindElement(By.TagName("modal-container"));
                // checking !modal.Displayed doesn't work (change button still get clicked)
                // Checking modal == null produces NoSuchElementException error
                List<IWebElement> element2 = d.FindElements(By.TagName("modal-container")).ToList();
                if (element != null && element.Displayed && element.Enabled && element2.Count == 0)
                {
                    return element;
                }

                return null;
            });

        
            changeButton.Click();
            
            
        }


        [When(@"I enter my state name")]
        public void WhenIEnterMyStateName(dynamic instance)
        {
            _inputStateName = instance;
            IWebElement searchButton;

            driver.FindElement(By.Id("search-box")).SendKeys(_inputStateName.State);

            // Wait till search button is clickable to bypass the overlay
            searchButton = fluentWait.Until(d =>
            {
                IWebElement element = d.FindElement(By.CssSelector("button[test-id=search-location-button]"));
                if (element != null && element.Displayed && element.Displayed)
                {
                    return element;
                }

                return null;
            });

            searchButton.Click();
            // Fix this after Winnie makes update button unclickble until location has been set
            // Use ExpectedConditions instead
            System.Threading.Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("button[test-id=update-location-button]")).Click();
        }
        
        [Then(@"I can see my state name on the upper navigation bar")]
        public void ThenICanSeeMyStateNameOnTheUpperNavigationBar(dynamic instance)
        {
            _displayedStateName = instance;
            IWebElement userLocation;

            userLocation = fluentWait.Until(d =>
            {
                IWebElement element = d.FindElement(By.CssSelector("span[test-id=user-location]"));
                if (element.Displayed) {
                    return element;
                }

                return null;
            });

            Assert.IsTrue(userLocation.Text == _displayedStateName.State);           
        }

        [Then(@"I accomplished my task")]
        public void ThenIAccomplishedMyTask()
        {
            driver.Quit();
        }

    }
}
