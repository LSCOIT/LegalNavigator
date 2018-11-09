Feature: Search
	As a user
	I need to search by a phrase
	So that I can see the resources

Scenario Outline: Search by phrase
	Given I am on the Access2Justice website
	And current state is set to <State>
	When I type <Phrase> into the search input field and click search button
	Then I can see search results
    
Examples: 
	| Phrase                  | State  |
	| I am getting a divorce  | Hawaii |
	| I am getting kicked out | Hawaii | 