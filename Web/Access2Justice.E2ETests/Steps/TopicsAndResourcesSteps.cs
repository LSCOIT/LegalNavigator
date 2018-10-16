using Access2Justice.E2ETests.PageObjects;
using TechTalk.SpecFlow;
using System;

namespace Access2Justice.E2ETests.Steps
{
    [Binding]
    public class TopicsAndResourcesSteps
    {
        TopicsAndResources TopicsAndResources = new TopicsAndResources();
        BaseClass BaseClass = new BaseClass();

        [Given(@"I am on the home page")]
        public void GivenIAmOnTheHomePage()
        {
            string url = Convert.ToString(ScenarioContext.Current["baseUrl"]);
            BaseClass.ConfirmOnCorrectPage(url);
        }
        
        [When(@"I press See More Topics button in the section named More Information, Videos, and Links to Resources")]
        public void WhenIPressSeeMoreTopicsButtonInTheSectionNamedMoreInformationVideosAndLinksToResources()
        {
            BaseClass.ClickOnControl("see-more-topics-button");
        }

        [When(@"I click on Topics & Resources on the lower navigation bar")]
        public void WhenIClickOnTopicsResourcesOnTheLowerNavigationBar()
        {
            BaseClass.ClickOnControl("topics-and-resources-navigation-link");
        }

        [Then(@"I should directed to the Topics and Resources page")]
        public void ThenIShouldDirectedToTheTopicsAndResourcesPage()
        {
            BaseClass.ConfirmOnCorrectPage("topics");
        }

        [Given(@"I am on the Guided Assistant page")]
        public void GivenIAmOnTheGuidedAssistantPage()
        {
            BaseClass.ClickOnControl("guided-assistant-navigation-link");
        }

        [When(@"I click on the See More Topics button")]
        public void WhenIClickOnTheSeeMoreTopicsButton()
        {
            BaseClass.ClickOnControl("see-more-topics-button-guided-assistant");
        }

        [Given(@"I am on the Topics & Resources page")]
        public void GivenIAmOnTheTopicsResourcesPage()
        {
            BaseClass.ClickOnControl("topics-and-resources-navigation-link");
        }

        [When(@"I click on the first main topic")]
        public void WhenIClickOnTheFirstMainTopic()
        {
            TopicsAndResources.ClickOnTopicByCssSelector(".topic-list a");
        }

        [When(@"I click on the first subtopic")]
        public void WhenIClickOnTheFirstSubtopic()
        {
            TopicsAndResources.ClickOnTopicByCssSelector(".subtopics ul li a");
        }

        [Then(@"I should see a list of resources")]
        public void ThenIShouldSeeAListOfResources()
        {
            TopicsAndResources.ConfirmResourcesAreShown();
        }

        [Then(@"I should see the Service Organizations in Your Community pane to the right")]
        public void ThenIShouldSeeTheServiceOrganizationsInYourCommunityPaneToTheRight()
        {
            TopicsAndResources.ConfirmServiceOrganizationSideBarIsDisplayed();
        }

    }
}
