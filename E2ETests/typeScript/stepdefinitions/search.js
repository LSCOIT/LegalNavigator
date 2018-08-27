"use strict";
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
const searchPage_1 = require("../pages/searchPage");
const { When, Then } = require("cucumber");
const search = new searchPage_1.SearchPageObject();
When(/^I type "(.*?)"$/, (text) => __awaiter(this, void 0, void 0, function* () {
    yield search.searchTextBox.sendKeys(text);
}));
When(/^I click on search button$/, () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.actions().sendKeys(protractor_1.protractor.Key.ENTER).perform();
}));
Then(/^I click on google logo$/, () => __awaiter(this, void 0, void 0, function* () {
    yield search.logo.click();
}));
