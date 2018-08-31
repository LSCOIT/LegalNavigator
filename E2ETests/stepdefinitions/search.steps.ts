import { browser } from 'protractor';
import { Given, When, Then } from 'cucumber';
import { HomePageObject } from '../pages/homePage.po';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;

const search_homePage: HomePageObject = new HomePageObject();

Given('I am on the Access2Justice website', () => {  
    expect(browser.getTitle()).to.eventually.equal("Access to Justice");
});

When(/^I type "(.*?)" into the search input field and click search button$/,
   async (text: string) => await search_homePage.enterSearchInput(text));

Then('I can see search results', {timeout: 3 * 5000},
    async () => await search_homePage.getSearchResults());

Then('I clear the search text',
    async () => await search_homePage.clearSearchInput());