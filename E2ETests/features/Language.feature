# Using http://a2jstageweb.azurewebsites.net/ because lanaguage implementation has been updated there
# Setting location here is a repetitive step. Need to change later

Feature: Set an update location
    As a user
    I need to set my language 
    So I can understand the contents of the website 

@SetLanguage
Scenario: Set preferred language
    Given I am on the staged Access2Justice website
    When I select a language from the navigation bar
    Then I should see my page translated
