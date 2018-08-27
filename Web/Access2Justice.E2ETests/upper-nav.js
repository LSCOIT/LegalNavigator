
describe('Upper-Nav Smoke Tests', function () {
    // Expected Conditions library
    // Each condition returns a function that evaluates to a promises
    // https://github.com/angular/protractor/blob/5.4.0/lib/expectedConditions.ts
    // http://seleniumhq.github.io/selenium/docs/api/java/org/openqa/selenium/support/ui/ExpectedConditions.html#urlContains-java.lang.String-
    var EC = protractor.ExpectedConditions;

    beforeEach(function () {

        browser.get('http://localhost:5150/');

    });

    it('should open language dropdown menu', function () {

        var dropDownToggle = element(by.id('testDropDownToggle'));
        dropDownToggle.click();

        var dropDownMenu = element(by.id('language-dropdown'));
        expect(dropDownMenu.getAttribute('class')).toMatch('open');

    });

    // Test location selector 

    it('should go to privacy page', function () {

        var privacyPromiseTab = $('#testprivacyPromiseTab');
        var isClickable = EC.elementToBeClickable(privacyPromiseTab);

        browser.wait(isClickable, 20000); //wait for an element to become clickable
        privacyPromiseTab.click();

        var urlChanged = function () {
            return browser.getCurrentUrl().then(function (url) {
                return url.indexOf('/privacy') > -1;
            });
        };

    });

    it('should go to help page', function () {

        var helpTab = $('#testHelpTab');
        var isClickable = EC.elementToBeClickable(helpTab);

        browser.wait(isClickable, 20000);
        helpTab.click();

        var urlChanged = function () {
            return browser.getCurrentUrl().then(function (url) {
                return url.indexOf('/help') > -1;
            });
        };

    });

    // Test Login 
    fit('should direct to login page if not logged in', function () {

        var logInTab = $('#testLogInTab');
        var isClickable = EC.elementToBeClickable(logInTab);

        browser.wait(isClickable, 20000);
        logInTab.click();

        // New session => user has not signed in yet
        browser.driver.executeScript('sessionStorage.getItem("profileData");').then(function (retValue) {
            expect(retValue == null);
        });

        var urlChanged = function () {
            return browser.getCurrentUrl().then(function (url) {
                // external login url
                return url.indexOf('login.microsoftonlinne.com') > -1;
            });
        };

    });
});