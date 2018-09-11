import { browser, $ } from 'protractor';
import { Given, When, Then, setDefaultTimeout } from 'cucumber';
import { LoginComponent } from '../pages/loginComponent.po';
import { config } from '../config/config';
import { link } from 'fs';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;

const login: LoginComponent = new LoginComponent();

Given('I am on the resource page for {string} at {url}',
    async (topic, link) => {  
        await browser.get(link);
        expect(browser.getCurrentUrl()).to.eventually.equal('https://access2justicewebtesting.azurewebsites.net/topics/d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef');
        //expect($("h3").getText()).to.equal('Divorce');
    }
);

// When(/^I set the '([^']*)' '([^']*)' and click Next button$/, 
//     async (type: string, email: string) => await login.enterData("email", email)
// ); 

// When(/^I set the '([^']*)' '([^']*)' and click Sign In button$/, 
//     async (type: string, password: string) => await login.enterData("password", password)
// );


// Then(/^I am redirected to home page and shown as signed in with username '([^']*)'$/, 
//     async (userName: string) => {
//         await browser.waitForAngular();
//         expect(await browser.getCurrentUrl()).to.equal(config.baseUrl);
//         expect(await $("#signin-dropdown a").getText()).to.have.string(userName);
//     }
// );
