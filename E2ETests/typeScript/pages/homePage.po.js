"use strict";
/*
 * For more info on locators, check https://www.protractortest.org/#/locators
 * For more info on async/await, check https://www.protractortest.org/#/async-await
 * For more info on debuggig Protractor tests, check https://www.protractortest.org/#/debugging#disabled-control-flow
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
            yield this.searchTab.click();
            yield this.searchInputField.sendKeys(text);
            yield this.searchButton.click();
        });
    }
    getSearchResults() {
        return __awaiter(this, void 0, void 0, function* () {
            // Better than browser.sleep() since this might save time
            var condition = protractor_1.until.elementsLocated(protractor_1.by.tagName("app-resource-card"));
            protractor_1.browser.wait(condition, 15000);
            expect(yield this.resources.isPresent()).to.equal(true);
            expect(yield this.resources.count()).to.be.at.least(1);
        });
    }
    clearSearchInput() {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.searchInputField.clear();
        });
    }
}
exports.HomePageObject = HomePageObject;
