import { browser, element, by, By, $, $$, ExpectedConditions } from 'protractor';

describe('first test', () => {
    beforeEach(() => {
        browser.get(browser.baseUrl);
    });
    
    it('should display the title', () => {
        let title = element(by.id('first-logo'));
        expect(title.getText()).toEqual('Legal');
    });
});