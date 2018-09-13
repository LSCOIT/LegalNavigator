# Using http://a2jstageweb.azurewebsites.net/ because location implementation has been updated there

Feature: Set an update location
    As a user
    I need to set and update location
    So I can see resources relevant for my state

@SetLocationOnEntry
Scenario: Log into the application using the correct credentials
    Given I am on the staged Access2Justice website
    Then I am prompted to set my location
    And I can see my location on the upper navigation bar

@UpdateLocation
Scenario: Log into the application using the correct credentials
    Then I can update my location by clicking on the Change button the upper navigation bar


