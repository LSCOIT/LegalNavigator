Feature: Search for resources
  As a user
  I need to search by a phrase
  So that I can see the resources

@SearchScenario
Scenario Outline: Type in a search term
  Given I am on the Access2Justice website
  When I type "<word>" into the search input field and click search button
  Then I can see search results
  Then I clear the search text

  Examples:
    | word | 
    | I am getting a divorce |
    | I am getting kicked out |
    | I am facing workplace harassment |

  