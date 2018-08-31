/*
 * For more info on locators, check https://www.protractortest.org/#/locators
 * For more info on async/await, check https://www.protractortest.org/#/async-await
 * For more info on debuggig Protractor tests, check https://www.protractortest.org/#/debugging#disabled-control-flow
 */

import { $, $$, by, element, until, browser, protractor } from "protractor";
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const assert = chai.assert;

export class HomePageObject {
    public searchTab = $("span[class='inline search-text']");
    public searchInputField =  $("#search");
    public searchButton = $("input ~ button");
    public searchResults = $("app-search-results");
    public resources = $$("app-resource-card");  // all the resources
   
    public async enterSearchInput(text: string) {
        await this.searchTab.click();    
        await this.searchInputField.sendKeys(text);
        await this.searchButton.click();
    }

    public async getSearchResults() {
        // Better than browser.sleep() since this might save time
        var condition = until.elementsLocated(by.tagName("app-resource-card"));
        browser.wait(condition, 15000);

        expect(await this.resources.isPresent()).to.equal(true);
        expect(await this.resources.count()).to.be.at.least(1);
    }

    public async clearSearchInput() {
        await this.searchInputField.clear();
    }
}