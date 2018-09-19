@SetAndUpdateLocation
Feature: Set and update location
    As a user
    I need to set and update location
    So I can see resources relevant for my state

@SetLocation
Scenario: Set location on entry
    Given I am on the staged Access2Justice website
    When I am prompted to set my location
    And I enter 'Alaska' as my state name
    Then I can see "Alaska" on the upper navigation bar

@UpdateLocation
Scenario: Update location
    Given I am on the staged Access2Justice website
    When I click on the Change button
    And I enter 'Hawaii' as my state name
    Then I can see "Hawaii" on the upper navigation bar
    


