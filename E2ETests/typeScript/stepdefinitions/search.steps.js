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
const cucumber_1 = require("cucumber");
const searchComponent_po_1 = require("../pages/searchComponent.po");
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;
const search = new searchComponent_po_1.SearchComponent();
cucumber_1.Given('I am on the Access2Justice website', () => {
    expect(protractor_1.browser.getTitle()).to.eventually.equal("Access to Justice");
});
cucumber_1.When(/^I type "(.*?)" into the search input field and click search button$/, (text) => __awaiter(this, void 0, void 0, function* () { return yield search.enterSearchInput(text); }));
cucumber_1.Then('I can see search results', { timeout: 3 * 5000 }, () => __awaiter(this, void 0, void 0, function* () { return yield search.getSearchResults(); }));
cucumber_1.Then('I clear the search text', () => __awaiter(this, void 0, void 0, function* () { return yield search.clearSearchInput(); }));
