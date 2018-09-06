Feature: Login
    As a user
    I need to Login
    So I can save my resources and plans

@LoginScenario
Scenario Outline: Log into the application using the correct credentials
  Given I am on the Access2Justice website and I clicked the login tab
  When I set the 'email' '<email>' and click Next button
  When I set the 'password' '<password>' and click Sign In button
  Then I am redirected to home page and shown as signed in with username '<userName>'
  Then A session storage with name profileData is created with the UserName equal to '<userName>' and UserId equal to '<userId>'

  Examples:
    | email | password | userName | userId |
    | test-a2j@outlook.com | testa2j1234 | Random Tester | 109789E51F15…C5D280CCE370E6696D2DA52D3695C29D7CE56BECF8257B3 |