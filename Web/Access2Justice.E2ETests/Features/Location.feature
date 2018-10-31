Feature: Set Location
	As a user
    I need to set and update location
    So I can see resources relevant for my state

Scenario Outline: Set location on entry
	Given I am on the Access2Justice website
	And I am prompted to set my location
	When I enter my state name as <State>
	Then I can see my state name <State> on the upper navigation bar

Examples: 
	| State  |
	| Hawaii |

Scenario Outline: Change location
	Given I am on the Access2Justice website
	And current state is set to <Old State>
	When I click on the Change button
    And I enter my state name as <New State> 
    Then I can see my state name <New State> on the upper navigation bar

Examples: 
	| Old State | New State |
	| Hawaii    | Alaska    |

