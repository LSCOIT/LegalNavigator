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
    public class AboutSteps
    {
        SharedObjects SharedObjects = new SharedObjects();
        About AboutPage = new About();

        [When(@"I click on the About link on the lower navigation bar")]
        public void WhenIClickOnTheAboutLinkOnTheLowerNavigationBar()
        {
            SharedObjects.ClickButton("about-navigation-link");
        }
        
        [Then(@"I should be directed to the About page")]
        public void ThenIShouldBeDirectedToTheAboutPage()
        {
            AboutPage.ConfirmOnAboutPage();
        }
    }
}
