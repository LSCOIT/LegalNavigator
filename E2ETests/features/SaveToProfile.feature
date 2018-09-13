@SaveToProfileScenario

Feature: Save to Profile
    As a user
    I need to save a resource topic
    So that I can review it later

Scenario: Save a resource topic given I've signed in and have not saved the same resource already
  Given I am on the Access2Justice website and I clicked the login tab
  And I set the 'email' "test-a2j@outlook.com" and click Next button
  And I set the 'password' "testa2j1234" and click Sign In button
  And I naviagte to the resource page for "Divorce" at "https://access2justicewebtesting.azurewebsites.net/topics/d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef"
  And I have not saved this resource topic before
  When I click the Save to profile button
  Then I see a confirmation "Resource saved to profile"
  And I can see the resource topic with "Divorce" listed in my profile
  And I delete the resource 

Scenario: Save a resource topic given I've not signed in
