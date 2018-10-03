﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
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
        private IWebElement searchInputBox;

        [FindsBy(How = How.Id, Using = "search-location-button")]
        private IWebElement searchLocationButton;

        [FindsBy(How = How.Id, Using = "update-location-button")]
        private IWebElement updateLocationButton;

        [FindsBy(How = How.Id, Using = "user-location")]
        private IWebElement userLocation;

        [FindsBy(How = How.TagName, Using = "modal-container")]
        private IWebElement modal;

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

        public void ConfirmPageTranslated()
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
            Assert.IsTrue(driver.FindElement(By.TagName("html")).GetAttribute("lang") == "zh-CN");
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
            searchInputBox.Clear();
            searchInputBox.SendKeys(stateName);

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

        public void confirmStateNameHasBeenSet(dynamic state)
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

        public void confirmModalPresent()
        {
            Assert.IsTrue(modal.Displayed);
        }

        public void confirmPageTitle()
        {
            Assert.AreEqual(driver.Title, "Access to Justice");
        }
    }
}
