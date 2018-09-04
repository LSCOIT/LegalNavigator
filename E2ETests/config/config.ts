/*
 * Basic configuration to run cucumber feature files and step definitions with protractor
 * For more info, check https://www.protractortest.org/#/frameworks
 * 
 * To run this test suite (refer to package.json's scripts section):
 * npm run webdriver-start (start selenium server)
 * npm run build (transpile .ts files to .js and store it in the typeScript folder)
 * npm test (launches Chrome Browser and runs the script)
 */

import * as path from "path";
import { browser, Config } from "protractor";
import { Reporter } from "../support/reporter";
const jsonReports = process.cwd() + "/reports/json";

export const config: Config = {

    seleniumAddress: "http://127.0.0.1:4444/wd/hub",

    SELENIUM_PROMISE_MANAGER: false,

    baseUrl: "https://access2justicewebtesting.azurewebsites.net/",

    capabilities: {
        browserName: "chrome",
    },

    framework: "custom", // Use a custom framework adapter and set its relative path
    frameworkPath: require.resolve("protractor-cucumber-framework"),

    specs: [
        "../../features/*.feature", // Cucumber feature files
    ],

    onPrepare: () => {
        browser.ignoreSynchronization = true;
        browser.manage().window().maximize(); // maximize browser before executing feature files
        Reporter.createDirectory(jsonReports);
    },

    cucumberOpts: {
        compiler: "ts:ts-node/register",
        timeout: 20000,
        format: "json:./reports/json/cucumber_report.json", // Output format
        require: ["../../typeScript/stepdefinitions/*.js", "../../typeScript/support/*.js"], // Require step definition files before executing features
        strict: true,   // Fail if there are any undefined or pending steps
        tags: "@SearchScenario", // Only execute the features or scenario with those tags
    },

    onComplete: () => {
        Reporter.createHTMLReport();
    },
};
