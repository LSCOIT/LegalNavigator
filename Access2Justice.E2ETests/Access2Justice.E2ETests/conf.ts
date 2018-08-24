// To run this, first transpile it ot JavaScript with `npm run tsc`, then run `protractor conf.js`

/* Because this file imports from Protractor, you'll need to have it as a project dependency.
   Check lib/config.ts for more info.*/
import { Config } from 'protractor';

export let config: Config = {
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