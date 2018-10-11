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
    public class HelpAndFAQsSteps
    {
        NavigationBar SharedObjects = new NavigationBar();
        HelpAndFAQs HelpAndFAQs = new HelpAndFAQs();

        [When(@"I click on the Help & FAQs link on the upper navigation bar")]
        public void WhenIClickOnTheHelpFAQsLinkOnTheUpperNavigationBar()
        {
            SharedObjects.ClickButton("faq-navigation-link");
        }
        
        [Then(@"I should be directed to the Help & FAQs page")]
        public void ThenIShouldBeDirectedToTheHelpFAQsPage()
        {
            HelpAndFAQs.ConfirmOnFAQPage();
        }
    }
}
