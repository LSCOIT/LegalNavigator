/*
 * For more info on locators, check https://www.protractortest.org/#/locators
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
        // this.searchTab.isPresent().then((isPresent) => {
        //     if (isPresent) {
        //         assert.isFulfilled(this.searchTab.click(), null);// To reveal the search input field
        //     }
        // }).catch(() => {
        //     console.log("why you toooooo");
        // });

        await this.searchTab.click();
        
        //expect(this.searchInputField.isPresent()).to.eventually.equal(true);
        
        await this.searchInputField.sendKeys(text);
        await this.searchButton.click();

        // return this.searchInputField.sendKeys(text).then(()=> {       
        //     this.searchButton.click();      
        // }).catch(() => {
        //     console.log("whyyyyyyyyy fuck you ")
        // });
    }

    public async getSearchResults() {
        var condition = until.elementsLocated(by.tagName("app-search-results"));
        browser.wait(condition);

        var count = until.elementsLocated(by.tagName("app-resource-card"));
        browser.wait(count);

        this.resources.then(function(elems){
            var count = elems.length;
            expect(count).to.equal(10);
        }) 
        
        await browser.sleep(5000);
    }

    public async clearSearchInput() {
        //expect(this.searchInputField.isPresent()).to.eventually.equal(true);

        await this.searchInputField.clear();
    }
}