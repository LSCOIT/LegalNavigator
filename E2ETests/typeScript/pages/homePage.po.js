"use strict";
/*
 * For more info on locators, check https://www.protractortest.org/#/locators
 */
Object.defineProperty(exports, "__esModule", { value: true });
const protractor_1 = require("protractor");
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
class HomePageObject {
    constructor() {
        this.searchTab = protractor_1.$("span[class='inline search-text']");
        this.searchInputField = protractor_1.$("#search");
        this.searchButton = protractor_1.$("input ~ button");
        this.searchResults = protractor_1.$("app-search-results");
    }
    enterSearchInput(text) {
        if (protractor_1.browser.isElementPresent(this.searchTab)) {
            this.searchTab.click(); // To reveal the search input field
        }
        expect(protractor_1.browser.isElementPresent(this.searchInputField)).to.eventually.be.true;
        this.searchInputField.sendKeys(text);
        expect(this.searchButton.isPresent()).to.eventually.be.true;
        this.searchButton.click();
        return true;
    }
    getSearchResults() {
        protractor_1.browser.sleep(10000);
        return expect(protractor_1.browser.isElementPresent(this.searchResults)).to.eventually.be.false;
    }
    clearSearchInput() {
        expect(protractor_1.browser.isElementPresent(this.searchInputField)).to.eventually.be.true;
        return this.searchInputField.clear();
    }
}
exports.HomePageObject = HomePageObject;
