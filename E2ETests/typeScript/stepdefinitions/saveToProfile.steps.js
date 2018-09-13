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
const resourcePage_po_1 = require("../pages/resourcePage.po");
const loginComponent_po_1 = require("../pages/loginComponent.po");
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;
const should = chai.should;
const resourcePage = new resourcePage_po_1.ResourcePage();
const login = new loginComponent_po_1.LoginComponent();
cucumber_1.Given('I naviagte to the resource page for {string} at {string}', (topic, link) => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.get(link);
    yield protractor_1.browser.sleep(2000);
    expect(protractor_1.browser.getCurrentUrl()).to.eventually.equal(link);
    expect(protractor_1.$(".subtopic-heading").getText()).to.eventually.equal(topic);
}));
cucumber_1.Given('I have not saved this resource topic before', () => {
    // Might want to delete that resource if it exists
    return true;
});
cucumber_1.Given('I have not already signed in', () => {
    expect(protractor_1.$$(".account-menu").get(0).getText()).to.eventually.have.string("Log in");
});
cucumber_1.When('I click the Save to profile button', () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.sleep(2000);
    yield resourcePage.saveButton.click();
    yield protractor_1.browser.sleep(2000);
}));
cucumber_1.Then('I should be prompted to sign in', () => {
    expect(protractor_1.browser.getTitle()).to.eventually.equal("Sign in to your account");
});
cucumber_1.Then('I see a confirmation "Resource saved to profile"', () => {
    protractor_1.browser.wait(protractor_1.$(".toast-message").isDisplayed()).then(() => {
        expect(protractor_1.$(".toast-message").getText()).to.eventually.equal("Resource saved to profile");
    });
});
cucumber_1.Then('I can see the resource topic with {string} listed in my profile', (topic) => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.$("#signin-dropdown a").click();
    yield protractor_1.browser.wait(protractor_1.$(".dropdown-menu").isDisplayed());
    yield protractor_1.$$(".dropdown-item").get(0).click();
    expect(protractor_1.browser.getCurrentUrl()).to.eventually.equal('https://access2justicewebtesting.azurewebsites.net/profile');
    yield protractor_1.$$(".nav-link").get(1).click(); // My Saved Resources tab
    //var elem = await $$("h4").filter(elem => elem.innerHTML === 'Divorce').count();
    yield protractor_1.browser.sleep(2000);
    var count = yield protractor_1.$$("h4").filter(function (elem) {
        return elem.getText().then(function (text) {
            return text === 'Divorce';
        });
    }).count();
    expect(count).to.equal(1);
}));
cucumber_1.Then('I delete the resource', () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.$$(".btn-group").get(3).click(); // will break if there is more than 1 saved resource
    yield protractor_1.browser.wait(protractor_1.$("#dropdown-disabled-item").isDisplayed());
    yield protractor_1.$("a app-remove-button").click();
}));
