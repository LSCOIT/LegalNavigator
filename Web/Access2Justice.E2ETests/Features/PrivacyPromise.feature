Feature: PrivacyPromise
	As a user
	I need to read the privacy promise
	So I can know how a2j handles my data

Scenario: Navigate to Privacy Promise page from home page
	Given I am on the Access2Justice website
	And current state is set to
	| State  |
	| Alaska |
	When I click on the Privacy Promise link on the upper navigation bar
	Then I should be directed to the Privacy Promise page

