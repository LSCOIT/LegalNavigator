Feature: Set language
    As a user
    I need to set my language 
    So I can understand the contents of the website 

Scenario: Set preferred language
	Given I am on the Access2Justice website
	And current state is set to
	| State  |
	| Alaska |
    When I select a language from the navigation bar
	| Selected Language |
	| Chinese (Simplified) |
    Then I should see my page translated
	| Expected Language |
	| zh-CN             |

