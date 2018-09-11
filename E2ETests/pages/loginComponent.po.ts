import { $, $$, by, element, until, browser, protractor } from "protractor";
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const assert = chai.assert;

export class LoginComponent {
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
} 