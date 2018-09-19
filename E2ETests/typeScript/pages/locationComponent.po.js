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
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const assert = chai.assert;
let EC = protractor_1.protractor.ExpectedConditions;
class LocationComponent {
    constructor() {
        this.bingMap = protractor_1.$('modal-container');
        this.inputBox = protractor_1.$('#search-box');
        this.searchButton = protractor_1.$('.search-btn');
        this.updateButton = protractor_1.element(protractor_1.by.buttonText('Update'));
        this.userLocation = protractor_1.$('app-map span');
        this.changeLocationButton = protractor_1.$(".change-location-btn");
    }
    enterLocation(stateName) {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.inputBox.sendKeys(stateName);
            yield this.searchButton.click();
            // Wait for bing map pin to set
            // Cannot find a better way yet to detect pin has been set
            yield protractor_1.browser.sleep(2000);
            yield this.updateButton.click();
            // Wait for bing map to disappear
            yield protractor_1.browser.wait((EC.stalenessOf(this.bingMap)), 5000);
        });
    }
    confirmUserLocation(stateName) {
        return __awaiter(this, void 0, void 0, function* () {
            yield protractor_1.browser.wait(EC.textToBePresentInElement(this.userLocation, stateName), 5000);
        });
    }
}
exports.LocationComponent = LocationComponent;
