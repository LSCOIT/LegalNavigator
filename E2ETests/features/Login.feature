Feature: Login
    As a user
    I need to Login
    So I can save my resources and plans

@LoginScenario
Scenario Outline: Log into the application using the correct credentials
  Given I am on the Access2Justice website
  When I type "<word>" into the search input field and click search button
  Then I can see search results
  Then I clear the search text

  Examples:
    | word | 
    | I am getting a divorce |
    | I am getting kicked out |
    | I am facing workplace harassment |