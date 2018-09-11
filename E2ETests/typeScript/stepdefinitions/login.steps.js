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
const config_1 = require("../config/config");
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;
const login = new loginComponent_po_1.LoginComponent();
// By default, async functions timeout after 5000 ms
// For more info on setting timeout, 
// check https://github.com/cucumber/cucumber-js/blob/master/docs/support_files/timeouts.md
cucumber_1.setDefaultTimeout(20 * 1000);
cucumber_1.Given('I am on the Access2Justice website and I clicked the login tab', () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.sleep(2000); // Wait for page to load
    expect(protractor_1.browser.getTitle()).to.eventually.equal("Access to Justice");
    yield login.loginTab.click();
}));
'I set the {string} {string} and click Next button';
cucumber_1.When('I set the {string} {string} and click Next button', (type, email) => __awaiter(this, void 0, void 0, function* () { return yield login.enterData("email", email); }));
cucumber_1.When('I set the {string} {string} and click Sign In button', (type, password) => __awaiter(this, void 0, void 0, function* () { return yield login.enterData("password", password); }));
cucumber_1.Then(/^I am redirected to home page and shown as signed in with username '([^']*)'$/, (userName) => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.waitForAngular();
    expect(yield protractor_1.browser.getCurrentUrl()).to.equal(config_1.config.baseUrl);
    expect(yield protractor_1.$("#signin-dropdown a").getText()).to.have.string(userName);
}));
