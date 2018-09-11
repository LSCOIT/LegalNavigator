"use strict";
/*
 * Used to prepare and clean the environment
 * before" and "after" each scenario is executed
 * For more info, check https://github.com/cucumber/cucumber-js/blob/master/docs/support_files/hooks.md
 */
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const { BeforeAll, After, AfterAll, Status } = require("cucumber");
const protractor_1 = require("protractor");
const config_1 = require("../config/config");
BeforeAll({ tags: 'not @SaveToProfileScenario', timeout: 100 * 1000 }, () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.get(config_1.config.baseUrl);
}));
After(function (scenario) {
    return __awaiter(this, void 0, void 0, function* () {
        // Take screenshot if scenario fails
        if (scenario.result.status === Status.FAILED) {
            // screenShot is a base-64 encoded PNG
            const screenShot = yield protractor_1.browser.takeScreenshot();
            this.attach(screenShot, "image/png");
        }
    });
});
AfterAll({ timeout: 100 * 1000 }, () => __awaiter(this, void 0, void 0, function* () {
    yield protractor_1.browser.quit();
}));
