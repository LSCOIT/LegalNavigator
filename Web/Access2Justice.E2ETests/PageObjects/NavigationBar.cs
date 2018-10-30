using Microsoft.VisualStudio.TestTools.UnitTesting;
//using TechTalk.SpecFlow.Assist;
//using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Access2Justice.E2ETests.PageObjects
{
    class NavigationBar
    {
        #region
        private IWebDriver Driver => ScenarioContext.Current.Get<IWebDriver>("driver");
        private DefaultWait<IWebDriver> FluentWait => ScenarioContext.Current.Get<DefaultWait<IWebDriver>>("fluentWait");

        [FindsBy(How = How.Id, Using = "language-dropdown")]
        private IWebElement setLanguageButton;

        [FindsBy(How = How.TagName, Using = "select")]
        private IWebElement languageDropDown;

        [FindsBy(How = How.TagName, Using = "modal-container")]
        private IList<IWebElement> modalList;

        [FindsBy(How = How.Id, Using = "search-box")]
        private IWebElement searchLocationInputBox;

        [FindsBy(How = How.Id, Using = "search-location-button")]
        private IWebElement searchLocationButton;

        [FindsBy(How = How.Id, Using = "update-location-button")]
        private IWebElement updateLocationButton;

        [FindsBy(How = How.Id, Using = "search")]
        private IWebElement searchPhraseInputField;

        [FindsBy(How = How.Id, Using = "search-phrase-button")]
        private IWebElement searchPhraseButton;

        [FindsBy(How = How.TagName, Using = "app-resource-card")]
        private IList<IWebElement> searchPhraseResults;

        [FindsBy(How = How.TagName, Using = "app-web-resource")]
        private IList<IWebElement> searchPhraseWebResults;

        [FindsBy(How = How.TagName, Using = "html")]
        private IWebElement html;
        #endregion

        BaseClass BaseClass = new BaseClass();

        public NavigationBar()
        {
            // Need to find another solution for PageFactory in the future: 
            // https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras
            PageFactory.InitElements(Driver, this);
        }

        public void PickLanguage(string language)
        {
            FluentWait.Until(d =>
            {
                if (setLanguageButton != null && setLanguageButton.Enabled && setLanguageButton.Displayed && modalList.Count == 0)
                {
                    return true;
                }

                return false;
            });

            setLanguageButton.Click();
            Assert.IsTrue(languageDropDown.Displayed);

            SelectElement languageOptions = new SelectElement(languageDropDown);
            languageOptions.SelectByText(language);
        }

        public void ConfirmPageTranslated(string language)
        {
            FluentWait.Until(d =>
            {
                IWebElement element = Driver.FindElement(By.TagName("html"));
                if (html != null && html.GetAttribute("class") == "translated-ltr")
                {
                    return true;
                }

                return false;
            });

            Assert.IsTrue(html.GetAttribute("lang") == language);
        }

        public void ConfirmModalPresent()
        {
            Assert.IsTrue(modalList.Count > 0);
        }

        public void EnterStateName(dynamic state)
        {
            // Has to declare the string outside before putting it in SendKeys
            string stateName = Convert.ToString(state);
            searchLocationInputBox.Clear();
            searchLocationInputBox.SendKeys(stateName);
            BaseClass.ClickOnControl("search-location-button");
            // Fix this after Winnie makes update button unclickble until location has been set
            // Use ExpectedConditions instead
            System.Threading.Thread.Sleep(2000);
            updateLocationButton.Click();
        }

        public void ConfirmStateNameHasBeenSet(dynamic state)
        {
            FluentWait.Until(d =>
            {
                IWebElement userLocation = Driver.FindElement(By.Id("user-location"));

                if (userLocation.Displayed)
                {
                    Assert.AreEqual(userLocation.Text, Convert.ToString(state));
                    return true;
                }

                return false;
            });
        }

        public void SearchByPhrase(string phrase)
        {
            BaseClass.ClickOnControl("search-phrase-tab");
            searchPhraseInputField.SendKeys(phrase);
            searchPhraseButton.Click();
        }

        public void ConfirmSearchResults()
        {
            FluentWait.Until(d =>
            {
                if (searchPhraseResults != null && (searchPhraseResults.Count > 0 || searchPhraseWebResults.Count > 0))
                {
                    return true;
                }

                return false;
            });
            Assert.IsTrue(searchPhraseResults.Count > 0 || searchPhraseWebResults.Count > 0);
        }
    }
}
