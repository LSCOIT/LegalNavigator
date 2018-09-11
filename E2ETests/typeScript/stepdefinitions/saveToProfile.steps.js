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
const loginComponent_po_1 = require("../pages/loginComponent.po");
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;
const login = new loginComponent_po_1.LoginComponent();
// By default, async functions timeout after 5000 ms
// For more info on setting timeout, 
// check https://github.com/cucumber/cucumber-js/blob/master/docs/support_files/timeouts.md
cucumber_1.setDefaultTimeout(20 * 1000);
var urlRegex = "(https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*))";
cucumber_1.Given('I am on the resource page for {string} at {url}', (topic, link) => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.get(link);
    expect(protractor_1.browser.getCurrentUrl()).to.eventually.equal('https://access2justicewebtesting.azurewebsites.net/topics/d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef');
    //expect($("h3").getText()).to.equal('Divorce');
}));
// When(/^I set the '([^']*)' '([^']*)' and click Next button$/, 
//     async (type: string, email: string) => await login.enterData("email", email)
// ); 
// When(/^I set the '([^']*)' '([^']*)' and click Sign In button$/, 
//     async (type: string, password: string) => await login.enterData("password", password)
// );
// Then(/^I am redirected to home page and shown as signed in with username '([^']*)'$/, 
//     async (userName: string) => {
//         await browser.waitForAngular();
//         expect(await browser.getCurrentUrl()).to.equal(config.baseUrl);
//         expect(await $("#signin-dropdown a").getText()).to.have.string(userName);
//     }
// );
