Feature: Location
	As a user
    I need to set and update location
    So I can see resources relevant for my state

@SmokeTest @SetLocation
Scenario: Set location on entry
	Given I am on the Access2Justice website
	And I am prompted to set my location
	When I enter my state name
	| State  |
	| Alaska | 
	Then I can see my state name on the upper navigation bar
	| State  |
	| Alaska | 


