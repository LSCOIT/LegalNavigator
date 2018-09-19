import { browser } from 'protractor';
import { Given, When, Then } from 'cucumber';
import { LocationComponent } from '../pages/locationComponent.po';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;

const location: LocationComponent = new LocationComponent();

Given('I am on the staged Access2Justice website', async () => {
    expect(browser.getTitle()).to.eventually.equal("Access to Justice");
});

When('I am prompted to set my location', async () => {
    expect(location.bingMap.isDisplayed()).to.eventually.be.true;
});

When('I enter {string} as my state name', async (stateName: string) => {
    await location.enterLocation(stateName);
});

When('I click on the Change button', async () => {
    await location.changeLocationButton.click();
})

Then('I can see {string} on the upper navigation bar', async (stateName: string) => {
    await location.confirmUserLocation(stateName);
});

