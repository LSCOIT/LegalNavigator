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
class LoginObject {
    constructor() {
        this.loginTab = protractor_1.$(".account-menu");
        this.emailInput = protractor_1.$("#i0116"); // set by MS engineers
        this.passwordInput = protractor_1.$("#i0118");
        this.nextButton = protractor_1.$("input[type='submit'][data-bind]");
    }
    enterData(type, data) {
        return __awaiter(this, void 0, void 0, function* () {
            // Otherwise password input box and sign in button cannot be clicked
            yield protractor_1.browser.sleep(2000);
            if (type === "email") {
                yield this.emailInput.sendKeys(data);
            }
            else if (type === 'password') {
                yield this.passwordInput.sendKeys(data);
            }
            yield this.nextButton.click();
        });
    }
    checkSessionStorage(type, data) {
        return __awaiter(this, void 0, void 0, function* () {
            var returnedprofileData = protractor_1.browser.executeScript("return window.sessionStorage.getItem('profileData');");
            console.log("returned: " + returnedprofileData);
            var expectedProfileData = '{"UserName":"Random Tester","UserId":"109789E51F15EBE9C5918EF194954759EB0C8B44DE0E92C55AE433000D262EBFF9BCFC13B3C937534C5D280CCE370E6696D2DA52D3695C29D7CE56BECF8257B3"}';
            expect(returnedprofileData).to.eventually.equal(expectedProfileData);
        });
    }
}
exports.LoginObject = LoginObject;
