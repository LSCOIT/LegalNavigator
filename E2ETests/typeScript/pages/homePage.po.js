"use strict";
/*
 * For more info on locators, check https://www.protractortest.org/#/locators
 */
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const protractor_1 = require("protractor");
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const assert = chai.assert;
class HomePageObject {
    constructor() {
        this.searchTab = protractor_1.$("span[class='inline search-text']");
        this.searchInputField = protractor_1.$("#search");
        this.searchButton = protractor_1.$("input ~ button");
        this.searchResults = protractor_1.$("app-search-results");
        this.resources = protractor_1.$$("app-resource-card"); // all the resources
    }
    enterSearchInput(text) {
        return __awaiter(this, void 0, void 0, function* () {
            // this.searchTab.isPresent().then((isPresent) => {
            //     if (isPresent) {
            //         assert.isFulfilled(this.searchTab.click(), null);// To reveal the search input field
            //     }
            // }).catch(() => {
            //     console.log("why you toooooo");
            // });
            yield this.searchTab.click();
            //expect(this.searchInputField.isPresent()).to.eventually.equal(true);
            yield this.searchInputField.sendKeys(text);
            yield this.searchButton.click();
            // return this.searchInputField.sendKeys(text).then(()=> {       
            //     this.searchButton.click();      
            // }).catch(() => {
            //     console.log("whyyyyyyyyy fuck you ")
            // });
        });
    }
    getSearchResults() {
        return __awaiter(this, void 0, void 0, function* () {
            var condition = protractor_1.until.elementsLocated(protractor_1.by.tagName("app-search-results"));
            protractor_1.browser.wait(condition);
            var count = protractor_1.until.elementsLocated(protractor_1.by.tagName("app-resource-card"));
            protractor_1.browser.wait(count);
            this.resources.then(function (elems) {
                var count = elems.length;
                expect(count).to.equal(10);
            });
            yield protractor_1.browser.sleep(5000);
        });
    }
    clearSearchInput() {
        return __awaiter(this, void 0, void 0, function* () {
            //expect(this.searchInputField.isPresent()).to.eventually.equal(true);
            yield this.searchInputField.clear();
        });
    }
}
exports.HomePageObject = HomePageObject;
