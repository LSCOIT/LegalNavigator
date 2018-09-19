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
cucumber_1.Given('I am on the staged Access2Justice website', () => __awaiter(this, void 0, void 0, function* () {
    expect(protractor_1.browser.getTitle()).to.eventually.equal("Access to Justice");
}));
cucumber_1.When('I am prompted to set my location', () => __awaiter(this, void 0, void 0, function* () {
    expect(location.bingMap.isDisplayed()).to.eventually.be.true;
}));
cucumber_1.When('I enter {string} as my state name', (stateName) => __awaiter(this, void 0, void 0, function* () {
    yield location.enterLocation(stateName);
}));
cucumber_1.When('I click on the Change button', () => __awaiter(this, void 0, void 0, function* () {
    yield location.changeLocationButton.click();
}));
cucumber_1.Then('I can see {string} on the upper navigation bar', (stateName) => __awaiter(this, void 0, void 0, function* () {
    yield location.confirmUserLocation(stateName);
}));
