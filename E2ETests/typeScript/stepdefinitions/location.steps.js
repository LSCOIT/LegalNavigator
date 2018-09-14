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
const locationComponent_po_1 = require("../pages/locationComponent.po");
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;
const location = new locationComponent_po_1.LocationComponent();
cucumber_1.setDefaultTimeout(20 * 1000);
cucumber_1.Given('I am on the staged Access2Justice website', () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.get('https://a2jstageweb.azurewebsites.net/');
    expect(protractor_1.browser.getTitle()).to.eventually.equal("Access to Justice");
}));
cucumber_1.Then('I am prompted to set my location', () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.sleep(5000);
    expect(protractor_1.$('.modal-content').isDisplayed()).to.eventually.be.true;
    yield protractor_1.$('#search-box').sendKeys("Alaska");
    yield protractor_1.$('.search-btn').click();
    yield protractor_1.browser.sleep(2000);
    yield protractor_1.element(protractor_1.by.buttonText('Update')).click();
}));
cucumber_1.Then('I can see my location on the upper navigation bar', () => {
    expect(protractor_1.$('app-map span').getText()).to.eventually.equal("Alaska");
});
cucumber_1.Then('I can update my location by clicking on the Change button the upper navigation bar', () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.sleep(2000);
    yield protractor_1.$(".change-location-btn").click();
    yield protractor_1.$('#search-box').sendKeys("Hawaii");
    yield protractor_1.$('.search-btn').click();
    yield protractor_1.browser.sleep(2000);
    yield protractor_1.element(protractor_1.by.buttonText('Update')).click();
    expect(protractor_1.$('app-map span').getText()).to.eventually.equal("Hawaii");
}));
