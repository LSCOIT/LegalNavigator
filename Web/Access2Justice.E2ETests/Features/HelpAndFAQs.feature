Feature: HelpAndFAQs
	As a user
	I need to view answers to frequently asked questions
	So I know how to better navigate the site and use the tools

Scenario: Navigate to Help & FAQs page from home page
	Given I am on the Access2Justice website with state set
	| State  |
	| Alaska |
	When I click on the About link on the lower navigation bar
	Then I should be directed to the About page