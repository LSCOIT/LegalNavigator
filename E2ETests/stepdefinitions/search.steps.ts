import { browser } from 'protractor';
import { Given, When, Then } from 'cucumber';
import { SearchComponent } from '../pages/searchComponent.po';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;

const search: SearchComponent = new SearchComponent();

Given('I am on the Access2Justice website', () => {  
    expect(browser.getTitle()).to.eventually.equal("Access to Justice");
});

When(/^I type "(.*?)" into the search input field and click search button$/,
   async (text: string) => await search.enterSearchInput(text)
);

Then('I can see search results', {timeout: 3 * 5000},
    async () => await search.getSearchResults()
);

Then('I clear the search text',
    async () => await search.clearSearchInput()
);