using System;
using TechTalk.SpecFlow;
using Access2Justice.E2ETests.PageObjects;
using TechTalk.SpecFlow.Assist;

namespace Access2Justice.E2ETests.Steps
{
    [Binding]
    public class SearchSteps
    {
        HomePage HomePage = new HomePage();

        [When(@"I type a phrase into the search input field and click search button")]
        public void WhenITypeAPhraseIntoTheSearchInputFieldAndClickSearchButton(Table instance)
        {
            dynamic phrases = instance.CreateDynamicInstance();
            HomePage.searchByPhrase(phrases.Phrase);
        }
        
        [Then(@"I can see search results")]
        public void ThenICanSeeSearchResults()
        {
            HomePage.confirmResults();
        }
    }
}
