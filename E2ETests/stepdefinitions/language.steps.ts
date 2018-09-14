import { browser, $$, $, element, by } from 'protractor';
import { Given, When, Then, setDefaultTimeout } from 'cucumber';
import { config } from '../config/config';
import { LanguageComponent } from '../pages/languageComponent.po';
const chai = require('chai').use(require('chai-as-promised'));
const expect = chai.expect;

const language: LanguageComponent = new LanguageComponent();

When('I select a language from the navigation bar', async () => {
    //await browser.wait($('#language-dropdown').isDisplayed());
    await browser.sleep(5000);
    await $('#language-dropdown').click();
    await browser.sleep(2000);
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