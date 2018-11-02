Feature: TopicsAndResources
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario Outline: Navigate to topics page from home page
	Given I am on the Access2Justice website
	And current state is set to <State>
	And I am on the home page
	When I press See More Topics button in the section named More Information, Videos, and Links to Resources
	Then I should directed to the Topics and Resources page

Examples: 
	| State  |
	| Hawaii |

Scenario Outline: Navigate to topics page by clicking on lower navigation bar
	Given I am on the Access2Justice website
	And current state is set to <State>
	When I click on Topics & Resources on the lower navigation bar
	Then I should directed to the Topics and Resources page

Examples: 
	| State  |
	| Hawaii |

Scenario Outline: Navigate to topics page from Guided Assistance Page
	Given I am on the Access2Justice website
	And current state is set to <State>
	And I am on the Guided Assistant page
	When I click on the See More Topics button
	Then I should directed to the Topics and Resources page

Examples: 
	| State  |
	| Hawaii |

Scenario Outline: Navigate to a subtopic and see a list of resources
	Given I am on the Access2Justice website
	And current state is set to <State>
	And I am on the Topics & Resources page
	When I click on the first main topic
	And I click on the first subtopic
	Then I should see a list of resources

Examples: 
	| State  |
	| Hawaii |

Scenario Outline: View Service Organizations in Your Community pane in Family topic
	Given I am on the Access2Justice website
	And current state is set to <State>
	And I am on the Topics & Resources page
	When I click on the first main topic
	Then I should see the Service Organizations in Your Community pane to the right

Examples: 
	| State  |
	| Hawaii |