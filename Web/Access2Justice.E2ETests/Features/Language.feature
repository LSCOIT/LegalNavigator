Feature: Set language
    As a user
    I need to set my language 
    So I can understand the contents of the website 

@SmokeTest
Scenario: Set preferred language
    Given I am on the Access2Justice website with state set
	| State  |
	| Alaska |
    #When I select a language from the navigation bar
    #Then I should see my page translated
	Then I accomplished my task
