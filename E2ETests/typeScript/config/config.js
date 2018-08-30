"use strict";
/*
 * Basic configuration to run cucumber feature files and step definitions with protractor
 * For more info, check https://www.protractortest.org/#/frameworks
 *
 * To run this test suite (refer to package.json's scripts section):
 * npm run webdriver-start (start selenium server)
 * npm run build (transpile .ts files to .js and store it in the typeScript folder)
 * npm test (launches Chrome Browser and runs the script)
 */
Object.defineProperty(exports, "__esModule", { value: true });
const protractor_1 = require("protractor");
const reporter_1 = require("../support/reporter");
const jsonReports = process.cwd() + "/reports/json";
exports.config = {
    seleniumAddress: "http://127.0.0.1:4444/wd/hub",
    SELENIUM_PROMISE_MANAGER: false,
    baseUrl: "https://access2justicewebtesting.azurewebsites.net/",
    capabilities: {
        browserName: "chrome",
    },
    framework: "custom",
    frameworkPath: require.resolve("protractor-cucumber-framework"),
    specs: [
        "../../features/*.feature",
    ],
    onPrepare: () => {
        protractor_1.browser.ignoreSynchronization = true;
        protractor_1.browser.manage().window().maximize(); // maximize browser before executing feature files
        reporter_1.Reporter.createDirectory(jsonReports);
    },
    cucumberOpts: {
        compiler: "ts:ts-node/register",
        format: "json:./reports/json/cucumber_report.json",
        require: ["../../typeScript/stepdefinitions/*.js", "../../typeScript/support/*.js"],
        strict: true,
        tags: "@SearchScenario",
    },
    onComplete: () => {
        reporter_1.Reporter.createHTMLReport();
    },
};
