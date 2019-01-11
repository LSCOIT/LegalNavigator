Feature: Set language
    As a user
    I need to set my language 
    So I can understand the contents of the website 

Scenario Outline: Set preferred language
	Given I am on the Access2Justice website
	And current state is set to <State>
    When I select a language <Selected Language> from the navigation bar
    Then I should see my page translated as <Expected Language>

Examples: 
	| State  | Selected Language    | Expected Language |
	| Hawaii | Chinese (Simplified) | zh-CN             |