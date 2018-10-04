Feature: TopicsAndResources
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Navigate to topics page from home page
	Given I am on the Access2Justice website with state set
	| State  |
	| Alaska |
	And I am on the home page
	When I press See More Topics button in the section named More Information, Videos, and Links to Resources
	Then I should directed to the Topics and Resources page