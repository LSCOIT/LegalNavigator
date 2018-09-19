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
const languageComponent_po_1 = require("../pages/languageComponent.po");
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;
let EC = protractor_1.protractor.ExpectedConditions;
const language = new languageComponent_po_1.LanguageComponent();
cucumber_1.setDefaultTimeout(30 * 1000);
cucumber_1.When('I select a language from the navigation bar', () => __awaiter(this, void 0, void 0, function* () {
    // var isClickable = EC.elementToBeClickable($('#language-dropdown'));
    // await browser.wait(isClickable, 30000); 
    //expect($('.modal-content').isDisplayed()).to.eventually.be.false;
    //await expect($('.black-overlay').isDisplayed()).to.eventually.be.false;
    // let ans = await $('.black-overlay').isDisplayed();
    // console.log("displayed: " + ans);
    yield protractor_1.browser.sleep(5000);
    expect(protractor_1.$('#language-dropdown').isDisplayed()).to.eventually.be.true;
    yield protractor_1.$('#language-dropdown').click();
    // let first =await $$("option").filter(function(elem) {
    //     return elem.getText().then(function(text) {
    //         return text === 'Chinese (Simplified)';
    //     });
    // }).first();
    yield protractor_1.$$("option").get(2).click();
}));
cucumber_1.Then('I should see my page translated', () => __awaiter(this, void 0, void 0, function* () {
    var lang = yield protractor_1.$('html').getAttribute('lang');
    return lang === 'af';
}));
