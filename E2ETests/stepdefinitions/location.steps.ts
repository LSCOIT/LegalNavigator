import { browser, $$, $, element, by } from 'protractor';
import { Given, When, Then, setDefaultTimeout } from 'cucumber';
import { config } from '../config/config';
import { LocationComponent } from '../pages/locationComponent.po';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;

const location: LocationComponent = new LocationComponent();

Given('I am on the staged Access2Justice website', async () => {
    await browser.get('http://a2jstageweb.azurewebsites.net/');
    expect(browser.getTitle()).to.eventually.equal("Access to Justice");
});

Then('I am prompted to set my location', async () => {
    expect($('.modal-content').isDisplayed()).to.eventually.be.true;
    await $('#search-box').sendKeys("Alaska");
    await $('.search-btn').click();
    await browser.sleep(2000);
    await element(by.buttonText('Update')).click();
});

Then('I can see my location on the upper navigation bar', () => {
    expect($('app-map span').getText()).to.eventually.equal("Alaska");
});

Then('I can update my location by clicking on the Change button the upper navigation bar', async() => {
    await browser.sleep(2000);
    await $(".change-location-btn").click();
    await $('#search-box').sendKeys("Hawaii");
    await $('.search-btn').click();
    await browser.sleep(2000);
    await element(by.buttonText('Update')).click();
    expect($('app-map span').getText()).to.eventually.equal("Hawaii");
});
