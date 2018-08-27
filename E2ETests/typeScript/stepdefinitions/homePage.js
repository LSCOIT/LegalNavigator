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
const { Given } = require("cucumber");
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const search = new searchPage_1.SearchPageObject();
Given(/^I am on "(.*?)" search page$/, (text) => __awaiter(this, void 0, void 0, function* () {
    if (text === 'google') {
        yield expect(protractor_1.browser.getTitle()).to.eventually.equal("Google");
    }
    else if (text === 'cucumber') {
        yield expect(protractor_1.browser.getTitle()).to.eventually.equal(text + " - Google Search");
    }
    else if (text === 'protractor') {
        yield expect(protractor_1.browser.getTitle()).to.eventually.equal(text + " - Google Search");
    }
}));
