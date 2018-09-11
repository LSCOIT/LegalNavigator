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
const linkComponent_po_1 = require("../pages/linkComponent.po");
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;
// By default, async functions timeout after 5000 ms
// For more info on setting timeout, 
// check https://github.com/cucumber/cucumber-js/blob/master/docs/support_files/timeouts.md
cucumber_1.setDefaultTimeout(20 * 1000);
const link = new linkComponent_po_1.LinkComponent();
cucumber_1.Given('I am on the Access2Justice website of links', () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.sleep(2000); // Wait for page to load
    expect(protractor_1.browser.getTitle()).to.eventually.equal("Access to Justice");
}));
cucumber_1.Then('I can navigate to all the links', () => {
    link.checkLinks();
});
