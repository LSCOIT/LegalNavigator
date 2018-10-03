Feature: Search
	As a user
	I need to search by a phrase
	So that I can see the resources

Scenario: Search by phrase
	Given I am on the Access2Justice website with state set
	| State  |
	| Alaska |
	When I type a phrase into the search input field and click search button
	| Phrase | 
    | I am getting a divorce |
	Then I can see search results
    