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
const resourcePage = new resourcePage_po_1.ResourcePage();
const login = new loginComponent_po_1.LoginComponent();
cucumber_1.Given('I naviagte to the resource page for {string} at {string}', (topic, link) => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.get(link);
    yield protractor_1.browser.sleep(2000); // wait for page to load
    expect(protractor_1.browser.getCurrentUrl()).to.eventually.equal(link);
    expect(protractor_1.$(".subtopic-heading").getText()).to.eventually.equal(topic);
}));
cucumber_1.Given('I have not saved this resource topic before', () => {
    // Might want to delete that resource if it exists
    return true;
});
cucumber_1.When('I click the Save to profile button', () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.sleep(5000);
    yield resourcePage.saveButton.click();
}));
cucumber_1.Then('I see a confirmation "Resource saved to profile"', () => __awaiter(this, void 0, void 0, function* () {
    expect(protractor_1.$("#toast-container").isDisplayed()).to.eventually.be.true;
    yield protractor_1.browser.sleep(5000);
}));
cucumber_1.Then('And I can see the resource topic with "Divorce" listed in my profile', () => __awaiter(this, void 0, void 0, function* () {
}));
