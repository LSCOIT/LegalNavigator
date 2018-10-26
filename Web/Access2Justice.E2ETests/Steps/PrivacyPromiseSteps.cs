using Access2Justice.E2ETests.PageObjects;
using TechTalk.SpecFlow;

namespace Access2Justice.E2ETests.Steps
{
    [Binding]
    public class PrivacyPromiseSteps
    {
        BaseClass BaseClass = new BaseClass();

        [When(@"I click on the Privacy Promise link on the upper navigation bar")]
        public void WhenIClickOnThePrivacyPromiseLinkOnTheUpperNavigationBar()
        {
            BaseClass.ClickOnControl("privacy-promise-navigation-link");
        }
        
        [Then(@"I should be directed to the Privacy Promise page")]
        public void ThenIShouldBeDirectedToThePrivacyPromisePage()
        {
            BaseClass.ConfirmOnCorrectPage("privacy");
        }
    }
}
