using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
//using TechTalk.SpecFlow.Assist;
using SpecFlow.Assist.Dynamic;
//using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeleniumExtras.PageObjects;

namespace Access2Justice.E2ETests.PageObjects
{
    class HomePage 
    {
        private IWebDriver driver => ScenarioContext.Current.Get<IWebDriver>("driver");
        private DefaultWait<IWebDriver> fluentWait => ScenarioContext.Current.Get<DefaultWait<IWebDriver>>("fluentWait");

        [FindsBy(How = How.Id, Using = "language-dropdown")]
        private IWebElement setLanguageButton;

        [FindsBy(How = How.TagName, Using = "select")]
        private IWebElement languageDropDown;

        [FindsBy(How = How.Id, Using = "change-location-button")]
        private IWebElement changeLocationButton;

        [FindsBy(How = How.Id, Using = "search-box")]
        private IWebElement searchLocationInputBox;

        [FindsBy(How = How.Id, Using = "search-location-button")]
        private IWebElement searchLocationButton;

        [FindsBy(How = How.Id, Using = "update-location-button")]
        private IWebElement updateLocationButton;

        [FindsBy(How = How.Id, Using = "user-location")]
        private IWebElement userLocation;

        [FindsBy(How = How.TagName, Using = "modal-container")]
        private IWebElement modal;

        [FindsBy(How = How.Id, Using = "search-phrase-tab")]
        private IWebElement searchPhraseTab;

        [FindsBy(How = How.Id, Using = "search")]
        private IWebElement searchPhraseInputField;

        [FindsBy(How = How.Id, Using = "search-phrase-button")]
        private IWebElement searchPhraseButton;

        [FindsBy(How = How.TagName, Using = "app-resource-card")]
        private IList<IWebElement> searchPhraseResults;

        public HomePage()
        {
            // Need to find another solution for PageFactory in the future: 
            // https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras
            PageFactory.InitElements(driver, this);
        }

        public void PickLanguage(dynamic language)
        {
            fluentWait.Until(d =>
            {
                // Work around
                List<IWebElement> modal = d.FindElements(By.TagName("modal-container")).ToList();
                if (setLanguageButton != null && setLanguageButton.Enabled && setLanguageButton.Displayed && modal.Count == 0)
                {
                    return true;
                }

                return false;
            });

            setLanguageButton.Click();
            Assert.IsTrue(languageDropDown.Displayed);
            
            SelectElement languageOptions = new SelectElement(languageDropDown);
            languageOptions.SelectByText(Convert.ToString(language));
        }

        public void ConfirmPageTranslated(dynamic language)
        {
            fluentWait.Until(d =>
            {
                IWebElement element = driver.FindElement(By.TagName("html"));
                if (element != null && element.GetAttribute("class") == "translated-ltr")
                {
                    return true;
                }

                return false;
            });

            // Need to change this later to make it dynamic
            Assert.IsTrue(driver.FindElement(By.TagName("html")).GetAttribute("lang") == language);
        }

        public void ClickChangeLocationButton()
        {
            fluentWait.Until(d =>
            {         
                //IWebElement element2 = d.FindElement(By.TagName("modal-container"));
                // checking !modal.Displayed doesn't work (change button still get clicked)
                // Checking modal == null produces NoSuchElementException error
                List<IWebElement> modal = d.FindElements(By.TagName("modal-container")).ToList();
                if (changeLocationButton != null && changeLocationButton.Displayed && changeLocationButton.Enabled && modal.Count == 0)
                {
                    return true;
                }

                return false;
            });

            changeLocationButton.Click();
        }

        public void EnterStateName(dynamic state)
        {
            // Has to declare the string outside before putting it in SendKeys
            string stateName = Convert.ToString(state);
            searchLocationInputBox.Clear();
            searchLocationInputBox.SendKeys(stateName);

            // Wait till search button is clickable to bypass the overlay
            fluentWait.Until(d =>
            {
                if (searchLocationButton != null && searchLocationButton.Enabled && searchLocationButton.Displayed)
                {
                    return true;
                }

                return false;
            });

            searchLocationButton.Click();
            // Fix this after Winnie makes update button unclickble until location has been set
            // Use ExpectedConditions instead
            System.Threading.Thread.Sleep(2000);
            updateLocationButton.Click();
        }

        public void ConfirmStateNameHasBeenSet(dynamic state)
        {
            fluentWait.Until(d =>
            {    
                if (userLocation.Displayed)
                {
                    return true;
                }

                return false;
            });

            Assert.AreEqual(userLocation.Text, Convert.ToString(state));
        }

        public void ConfirmModalPresent()
        {
            Assert.IsTrue(modal.Displayed);
        }

        public void ConfirmPageTitle()
        {
            Assert.AreEqual(driver.Title, "Access to Justice");
        }

        public void SearchByPhrase(string phrase)
        {
            fluentWait.Until(d =>
            {
                List<IWebElement> overlay = d.FindElements(By.ClassName("black-overlay")).ToList();
                List<IWebElement> modal = d.FindElements(By.TagName("modal-container")).ToList();
                if (searchPhraseTab != null && searchPhraseTab.Enabled && searchPhraseTab.Displayed && overlay.Count == 0 && modal.Count == 0)
                {
                    return true;
                }
                return false;
            });
            searchPhraseTab.Click();
            searchPhraseInputField.SendKeys(phrase);
            searchPhraseButton.Click();
        }

        public void ConfirmResults()
        {
            fluentWait.Until(d =>
            {
                if (searchPhraseResults != null && searchPhraseResults.Count > 0)
                {
                    return true;
                }

                return false;
            });
            Assert.IsTrue(searchPhraseResults.Count > 0);
        }

        public void ClickButton(string buttonId)
        {

            IWebElement buttonToClick = fluentWait.Until(d =>
            {
                IWebElement modal = driver.FindElement(By.TagName("modal-container"));
                IWebElement button = driver.FindElement(By.Id(buttonId));
                if (button != null && button.Displayed && button.Enabled && modal != null)
                {
                    return button;
                }

                return null;
            });

            buttonToClick.Click();
        }

        public void ConfirmOnHomePage()
        {
            Assert.AreEqual(driver.Url, ScenarioContext.Current["baseUrl"]);
        }
    }
}
