Feature: Save to Profile
    As a user
    I need to save a resource topic
    So that I can review it later

@SaveToProfileScenario
Scenario Outline: Save a resource topic to profile
  Given I am on the resource page for "<topic>" at "<link>"
#   When I click the Save to profile button
#   Then I see a confirmation "Resource saved to profile"
#   And I can see the resource topic with "<name>" listed in my profile

  Examples:
    | link | topic |
    | https://access2justicewebtesting.azurewebsites.net/topics/d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef | Divorce |