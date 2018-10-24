Feature: About
	As a user
	I need to view the mission and offerings of a2j
	So I know what the organization stands for

Scenario: Navigate to About page from home page
	Given I am on the Access2Justice website
	And current state is set to
	| State  |
	| Alaska |
	When I click on the About link on the lower navigation bar
	Then I should be directed to the About page
