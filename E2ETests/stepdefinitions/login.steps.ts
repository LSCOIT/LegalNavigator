import { browser, $ } from 'protractor';
import { Given, When, Then, setDefaultTimeout } from 'cucumber';
import { LoginObject } from '../pages/login.po';
import { config } from '../config/config';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;

const login: LoginObject = new LoginObject();

// By default, async functions timeout after 5000 ms
// For more info on setting timeout, 
// check https://github.com/cucumber/cucumber-js/blob/master/docs/support_files/timeouts.md
setDefaultTimeout(20 * 1000);

Given('I am on the Access2Justice website and I clicked the login tab',
    async () => {  
        await browser.sleep(2000);  // Wait for page to load
        expect(browser.getTitle()).to.eventually.equal("Access to Justice");
        await login.loginTab.click();
    }
);

When(/^I set the '([^']*)' '([^']*)' and click Next button$/, 
    async (type: string, email: string) => await login.enterData("email", email)
); 

When(/^I set the '([^']*)' '([^']*)' and click Sign In button$/, 
    async (type: string, password: string) => await login.enterData("password", password)
);


Then(/^I am redirected to home page and shown as signed in with username '([^']*)'$/, async (userName: string) => {
    await browser.waitForAngular();
    expect(await browser.getCurrentUrl()).to.equal(config.baseUrl);
    expect(await $("#signin-dropdown a").getText()).to.have.string(userName);
});
 
Then(/^A session storage with name profileData is created with the UserName equal to '([^']*)' and UserId equal to '([^']*)'$/, async (type: string, data: string) => {
    await login.checkSessionStorage(type, data);
});