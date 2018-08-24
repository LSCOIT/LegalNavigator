"use strict";
// To run this, first transpile it ot JavaScript with `npm run tsc`, then run `protractor conf.js`
Object.defineProperty(exports, "__esModule", { value: true });
exports.config = {
    framework: 'jasmine',
    seleniumAddress: 'http://localhost:4444/wd/hub',
    specs: ['spec.js'],
    baseUrl: 'https://access2justicewebtesting.azurewebsites.net',
    multiCapabilities: [{
            browserName: 'chrome',
            chromeOptions: {
                args: ['--window-size=1280,1000']
            }
        }],
    jasmineNodeOpts: {
        showColors: true,
        defaultTimeoutInterval: 30000
    }
};
//# sourceMappingURL=conf.js.map