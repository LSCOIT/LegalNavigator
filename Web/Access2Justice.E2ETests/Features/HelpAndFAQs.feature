﻿Feature: HelpAndFAQs
	As a user
	I need to view answers to frequently asked questions
	So I know how to better navigate the site and use the tools

Scenario Outline: Navigate to Help & FAQs page from home page
	Given I am on the Access2Justice website
	And current state is set to <State>
	When I click on the Help & FAQs link on the upper navigation bar
	Then I should be directed to the Help & FAQs page

Examples: 
	| State  |
	| Hawaii |