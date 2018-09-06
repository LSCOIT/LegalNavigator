import { $, $$, by, element, until, browser, protractor } from "protractor";
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const assert = chai.assert;

export class LoginObject {
    public loginTab = $(".account-menu");
    public emailInput = $("#i0116"); // set by MS engineers
    public passwordInput = $("#i0118");
    public nextButton = $("input[type='submit'][data-bind]")

    public async enterData(type: string, data: string) {
        // Otherwise password input box and sign in button cannot be clicked
        await browser.sleep(2000);

        if (type === "email") {
            await this.emailInput.sendKeys(data);
        } else if (type === 'password') {
            await this.passwordInput.sendKeys(data);
        }    
        
        await this.nextButton.click();
    }

    public async checkSessionStorage(type: string, data: string) {
        
        var returnedprofileData =  browser.executeScript(
            "return window.sessionStorage.getItem('profileData');"
        );
        console.log("returned: " + returnedprofileData);
        var expectedProfileData = '{"UserName":"Random Tester","UserId":"109789E51F15EBE9C5918EF194954759EB0C8B44DE0E92C55AE433000D262EBFF9BCFC13B3C937534C5D280CCE370E6696D2DA52D3695C29D7CE56BECF8257B3"}';
        expect(returnedprofileData).to.eventually.equal(expectedProfileData);

    
    }
} 