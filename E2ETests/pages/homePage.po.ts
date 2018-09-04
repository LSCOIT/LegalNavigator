/*
 * For more info on locators, check https://www.protractortest.org/#/locators
 * For more info on async/await, check https://www.protractortest.org/#/async-await
 * For more info on debuggig Protractor tests, check https://www.protractortest.org/#/debugging#disabled-control-flow
 */

import { $, $$, by, element, until, browser, protractor } from "protractor";
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const assert = chai.assert;
var option = 0;

export class HomePageObject {
    public searchTab = $("span[class='inline search-text']");
    public searchInputField =  $("#search");
    public searchButton = $("input ~ button");
    public results = $$("app-resource-card"); 
   
    public async enterSearchInput(text: string) {
        if (option == 0) {
            await this.searchTab.click(); 
        }
           
        await this.searchInputField.sendKeys(text);
        await this.searchButton.click();
        option++;
    }

    public async getSearchResults() {
        // Wait till the results are loaded
        await browser.wait(until.elementsLocated(by.tagName("app-resource-card")));
        expect(await this.results.count()).to.be.at.least(1);
    }

    public async clearSearchInput() {
        await this.searchInputField.clear();
    }
}