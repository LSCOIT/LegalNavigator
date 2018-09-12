import { browser, $, $$, element, by } from 'protractor';
import { Given, When, Then, setDefaultTimeout } from 'cucumber';
import { config } from '../config/config';
import { link } from 'fs';
import { ResourcePage } from '../pages/resourcePage.po';
import { LoginComponent } from '../pages/loginComponent.po';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;
const should = chai.should;


const resourcePage: ResourcePage = new ResourcePage();
const login: LoginComponent = new LoginComponent();

Given('I naviagte to the resource page for {string} at {string}',
    async (topic : string, link : string) => {
        await browser.get(link);
        await browser.sleep(2000);
        expect(browser.getCurrentUrl()).to.eventually.equal(link);
        expect($(".subtopic-heading").getText()).to.eventually.equal(topic);
    }
);

Given('I have not saved this resource topic before', 
    () => {
        // Might want to delete that resource if it exists
        return true;
    }
);

When('I click the Save to profile button', 
    async () => {
        await browser.sleep(2000);
        await resourcePage.saveButton.click();
        await browser.sleep(2000);
    }
); 

Then('I see a confirmation "Resource saved to profile"',
    () => {
        browser.wait($(".toast-message").isDisplayed()).then(() => {
            expect($(".toast-message").getText()).to.eventually.equal("Resource saved to profile");
        });
    }
);

Then('I can see the resource topic with {string} listed in my profile',
    async (topic: string) => {
        await $("#signin-dropdown a").click();
        await browser.wait($(".dropdown-menu").isDisplayed());
        await $$(".dropdown-item").get(0).click();
        expect(browser.getCurrentUrl()).to.eventually.equal('https://access2justicewebtesting.azurewebsites.net/profile');
        
        await $$(".nav-link").get(1).click( );
        await browser.sleep(5000);
        //var elem = await $$("h4").filter(elem => elem.innerHTML === 'Divorce').count();
        
        var count = await $$("h4").filter(function(elem) {
            return elem.getText().then(function(text) {
                return text === 'Divorce';
            });
        }).count();
        expect(count).to.equal(1);
    }
);

Then('I delete the resource',
    async () => {
        await $$(".btn-group").get(3).click();  // will break if there is more than 1 saved resource
        await browser.wait($("#dropdown-disabled-item").isDisplayed());
        await $("a app-remove-button").click();
    }
);