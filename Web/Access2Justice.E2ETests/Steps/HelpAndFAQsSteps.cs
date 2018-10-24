using Access2Justice.E2ETests.PageObjects;
using TechTalk.SpecFlow;

namespace Access2Justice.E2ETests.Steps
{
    [Binding]
    public class HelpAndFAQsSteps
    {
        BaseClass BaseClass = new BaseClass();

        [When(@"I click on the Help & FAQs link on the upper navigation bar")]
        public void WhenIClickOnTheHelpFAQsLinkOnTheUpperNavigationBar()
        {
            BaseClass.ClickOnControl("faq-navigation-link");
        }
        
        [Then(@"I should be directed to the Help & FAQs page")]
        public void ThenIShouldBeDirectedToTheHelpFAQsPage()
        {
            BaseClass.ConfirmOnCorrectPage("help");
        }
    }
}
