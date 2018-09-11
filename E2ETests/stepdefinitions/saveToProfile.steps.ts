import { browser, $ } from 'protractor';
import { Given, When, Then, setDefaultTimeout } from 'cucumber';
import { config } from '../config/config';
import { link } from 'fs';
import { ResourcePage } from '../pages/resourcePage.po';
import { LoginComponent } from '../pages/loginComponent.po';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;

const resourcePage: ResourcePage = new ResourcePage();
const login: LoginComponent = new LoginComponent();

Given('I naviagte to the resource page for {string} at {string}',
    async (topic : string, link : string) => {
        await browser.get(link);
        await browser.sleep(2000);  // wait for page to load
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
        await browser.sleep(5000);
        await resourcePage.saveButton.click();
    }
); 

Then('I see a confirmation "Resource saved to profile"',
    async () => {
        expect($("#toast-container").isDisplayed()).to.eventually.be.true;
        // check text
        
        await browser.sleep(5000);
    }
);

Then('And I can see the resource topic with "Divorce" listed in my profile',
    async () => {

    }
);
