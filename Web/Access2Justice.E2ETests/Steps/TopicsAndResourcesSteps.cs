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
        GuidedAssistant GuidedAssistant = new GuidedAssistant();

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

        [Given(@"I am on the Guided Assistant page")]
        public void GivenIAmOnTheGuidedAssistantPage()
        {
            GuidedAssistant.NavigateToGuidedAssistantPage();
        }

        [When(@"I click on the See More Topics button")]
        public void WhenIClickOnTheSeeMoreTopicsButton()
        {
            GuidedAssistant.ClickButton("see-more-topics-button-guided-assistant");
        }

        [Given(@"I am on the Topics & Resources page")]
        public void GivenIAmOnTheTopicsResourcesPage()
        {
            SharedObjects.ClickButton("topics-and-resources-navigation-link");
        }

        [When(@"I click on the first main topic")]
        public void WhenIClickOnTheFirstMainTopic()
        {
            TopicsAndResources.ClickOnTopic();
        }

        [When(@"I click on the first subtopic")]
        public void WhenIClickOnTheFirstSubtopic()
        {
            TopicsAndResources.ClickOnSubtopic();
        }

        [Then(@"I should see a list of resources")]
        public void ThenIShouldSeeAListOfResources()
        {
            TopicsAndResources.ConfirmResourcesAreShown();
        }


    }
}
