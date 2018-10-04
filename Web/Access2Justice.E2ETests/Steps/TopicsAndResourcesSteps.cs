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
    public class TopicsAndResourcesSteps
    {
        HomePage HomePage = new HomePage();
        TopicsAndResources TopicsAndResources = new TopicsAndResources();
        SharedObjects SharedObjects = new SharedObjects();

        [Given(@"I am on the home page")]
        public void GivenIAmOnTheHomePage()
        {
            HomePage.ConfirmOnHomePage();
        }
        
        [When(@"I press See More Topics button in the section named More Information, Videos, and Links to Resources")]
        public void WhenIPressSeeMoreTopicsButtonInTheSectionNamedMoreInformationVideosAndLinksToResources()
        {
            HomePage.ClickButton("see-more-topics-button");
        }

        [When(@"I click on Topics & Resources on the lower navigation bar")]
        public void WhenIClickOnTopicsResourcesOnTheLowerNavigationBar()
        {
            SharedObjects.ClickButton("topics-and-resources-navigation-link");
        }

        [Then(@"I should directed to the Topics and Resources page")]
        public void ThenIShouldDirectedToTheTopicsAndResourcesPage()
        {
            TopicsAndResources.ConfirmOnTopicsAndResourcesPage();
        }
    }
}
