using System;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;
using Access2Justice.E2ETests.PageObjects;

namespace Access2Justice.E2ETests.Steps
{
    [Binding]
    public class PrivacyPromiseSteps
    {
        NavigationBar SharedObjects = new NavigationBar();
        PrivacyPromise PrivacyPromise = new PrivacyPromise();

        [When(@"I click on the Privacy Promise link on the upper navigation bar")]
        public void WhenIClickOnThePrivacyPromiseLinkOnTheUpperNavigationBar()
        {
            SharedObjects.ClickButton("privacy-promise-navigation-link");
        }
        
        [Then(@"I should be directed to the Privacy Promise page")]
        public void ThenIShouldBeDirectedToThePrivacyPromisePage()
        {
            PrivacyPromise.ConfirmOnPrivacyPromisePage();
        }
    }
}
