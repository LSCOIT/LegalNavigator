"use strict";
/*
 * For more info, check https://www.npmjs.com/package/cucumber-html-reporter
 * or check video For more info, check https://www.youtube.com/watch?v=XQVQYKBuYAs
 */
Object.defineProperty(exports, "__esModule", { value: true });
const reporter = require("cucumber-html-reporter");
const fs = require("fs");
const mkdirp = require("mkdirp");
const path = require("path");
const jsonReports = path.join(process.cwd(), "/reports/json");
const htmlReports = path.join(process.cwd(), "/reports/html");
const screenshots = path.join(process.cwd(), "/screenshots/");
const targetJson = jsonReports + "/cucumber_report.json";
const cucumberReporterOptions = {
    theme: "bootstrap",
    jsonFile: targetJson,
    output: htmlReports + "/cucumber_reporter.html",
    reportSuiteAsScenarios: true,
    launchReport: true,
    storeScreenshots: true,
    screenshotsDirectory: screenshots,
    metadata: {
        "App Version": "1.0.0",
        "Test Environment": "Development",
        "Browser": "Chrome  54.0.2840.98",
        "Platform": "Windows 10",
        "Parallel": "Scenarios",
        "Executed": "Local"
    }
};
class Reporter {
    static createDirectory(dir) {
        if (!fs.existsSync(dir)) {
            mkdirp.sync(dir);
        }
    }
    static createHTMLReport() {
        try {
            reporter.generate(cucumberReporterOptions); // invoke cucumber-html-reporter
        }
        catch (err) {
            if (err) {
                throw new Error("Failed to save cucumber test results to json file.");
            }
        }
    }
}
exports.Reporter = Reporter;
