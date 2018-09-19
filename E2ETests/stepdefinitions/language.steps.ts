import { browser, $$, $, element, by, ExpectedConditions, protractor } from 'protractor';
import { Given, When, Then, setDefaultTimeout } from 'cucumber';
import { config } from '../config/config';
import { LanguageComponent } from '../pages/languageComponent.po';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;
let EC = protractor.ExpectedConditions;

const language: LanguageComponent = new LanguageComponent();
setDefaultTimeout(30 * 1000);

When('I select a language from the navigation bar', async () => {
    // var isClickable = EC.elementToBeClickable($('#language-dropdown'));
    // await browser.wait(isClickable, 30000); 
    //expect($('.modal-content').isDisplayed()).to.eventually.be.false;


    //await expect($('.black-overlay').isDisplayed()).to.eventually.be.false;

    // let ans = await $('.black-overlay').isDisplayed();
    // console.log("displayed: " + ans);
    await browser.sleep(5000);
    expect($('#language-dropdown').isDisplayed()).to.eventually.be.true;
    await $('#language-dropdown').click();
    // let first =await $$("option").filter(function(elem) {
    //     return elem.getText().then(function(text) {
    //         return text === 'Chinese (Simplified)';
    //     });
    // }).first();

    await $$("option").get(2).click();
});

Then('I should see my page translated', async () => {
    var lang = await $('html').getAttribute('lang');
    return lang === 'af';
});