using Access2Justice.E2ETests.PageObjects;
using TechTalk.SpecFlow;

namespace Access2Justice.E2ETests.Steps
{
    [Binding]
    public class AboutSteps
    {
        BaseClass BaseClass = new BaseClass();

        [When(@"I click on the About link on the lower navigation bar")]
        public void WhenIClickOnTheAboutLinkOnTheLowerNavigationBar()
        {
            BaseClass.ClickOnControl("about-navigation-link");
        }
        
        [Then(@"I should be directed to the About page")]
        public void ThenIShouldBeDirectedToTheAboutPage()
        {
            BaseClass.ConfirmOnCorrectPage("about");
        }
    }
}
