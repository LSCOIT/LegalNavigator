"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const protractor_1 = require("protractor");
class SearchPageObject {
    constructor() {
        this.searchTextBox = protractor_1.$("#lst-ib");
        this.searchButton = protractor_1.$("input[value='Google Search']");
        this.logo = protractor_1.$('div#logocont');
    }
}
exports.SearchPageObject = SearchPageObject;
