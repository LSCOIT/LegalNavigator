﻿using System;
using TechTalk.SpecFlow;
using Access2Justice.E2ETests.PageObjects;
using TechTalk.SpecFlow.Assist;

namespace Access2Justice.E2ETests.Steps
{
    [Binding]
    public class SearchSteps
    {
        HomePage HomePage = new HomePage();

        [When(@"I type (.*) into the search input field and click search button")]
        public void WhenITypeAPhraseIntoTheSearchInputFieldAndClickSearchButton(string phrase)
        {
            HomePage.SearchByPhrase(phrase);
        }
        
        [Then(@"I can see search results")]
        public void ThenICanSeeSearchResults()
        {
            HomePage.ConfirmResults();
        }
    }
}
