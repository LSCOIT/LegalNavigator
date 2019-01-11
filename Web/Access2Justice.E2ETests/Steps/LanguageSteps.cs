using Access2Justice.E2ETests.PageObjects;
using TechTalk.SpecFlow;

namespace Access2Justice.E2ETests
{
    [Binding]
    public class SetLanguageSteps : TechTalk.SpecFlow.Steps
    {
        NavigationBar NavigationBar = new NavigationBar();

        [When(@"I select a language (.*) from the navigation bar")]
        public void WhenISelectALanguageFromTheNavigationBar(string language)
        {
            NavigationBar.PickLanguage(language);
        }
        
        [Then(@"I should see my page translated as (.*)")]
        public void ThenIShouldSeeMyPageTranslated(string language)
        {
            NavigationBar.ConfirmPageTranslated(language);
        }
    }
}
