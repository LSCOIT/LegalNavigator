/*
 * For more info on locators, check https://www.protractortest.org/#/locators
 */

import { $, by, element, until, browser } from "protractor";
import { isNullOrUndefined } from "util";
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;

export class HomePageObject {
    public searchTab = $("span[class='inline search-text']");
    public searchInputField =  $("#search");
    public searchButton = $("input ~ button");
    public searchResults = $("app-search-results");
   
    public enterSearchInput(text: string) {
        if(browser.isElementPresent(this.searchTab)) {
            this.searchTab.click(); // To reveal the search input field
        }

        expect(browser.isElementPresent(this.searchInputField)).to.eventually.be.true;
        
        this.searchInputField.sendKeys(text);
        
        expect(this.searchButton.isPresent()).to.eventually.be.true;
        
        this.searchButton.click();

        return true;
    }

    public getSearchResults() {
        browser.sleep(10000);

       return expect(browser.isElementPresent(this.searchResults)).to.eventually.be.false;
    }

    public clearSearchInput() {
        expect(browser.isElementPresent(this.searchInputField)).to.eventually.be.true;

        return this.searchInputField.clear();
    }
}