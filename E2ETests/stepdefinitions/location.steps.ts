import { browser, $$, $, element, by, protractor } from 'protractor';
import { Given, When, Then, setDefaultTimeout } from 'cucumber';
import { config } from '../config/config';
import { LocationComponent } from '../pages/locationComponent.po';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;

const location: LocationComponent = new LocationComponent();
setDefaultTimeout(30 * 1000);
let EC = protractor.ExpectedConditions;

Given('I am on the staged Access2Justice website', async () => {
    expect(browser.getTitle()).to.eventually.equal("Access to Justice");
});

Then('I am prompted to set my location', async () => {
    expect($('.modal-content').isDisplayed()).to.eventually.be.true;
    await $('#search-box').sendKeys("Alaska");
    expect($('.search-btn').isPresent()).to.eventually.be.true;
    await $('.search-btn').click();
    await browser.sleep(5000);
    await element(by.buttonText('Update')).click();

    await browser.wait((EC.stalenessOf($('modal-container'))), 5000);
    console.log(EC.stalenessOf($('modal-container')));
});

Then('I can see my location on the upper navigation bar', async () => {
    
    EC.textToBePresentInElement($('app-map span'), "Alaska");
    expect($('app-map span').getText()).to.eventually.equal("Alaska");
});

Then('I can update my location by clicking on the Change button the upper navigation bar', async() => {
    await $(".change-location-btn").click();
    await $('#search-box').sendKeys("Hawaii");
    await $('.search-btn').click();
    await browser.sleep(2000);
    await element(by.buttonText('Update')).click();
    expect($('app-map span').getText()).to.eventually.equal("Hawaii");
});
