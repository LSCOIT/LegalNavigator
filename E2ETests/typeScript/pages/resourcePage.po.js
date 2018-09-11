"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const protractor_1 = require("protractor");
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const assert = chai.assert;
class ResourcePage {
    constructor() {
        this.saveButton = protractor_1.$("#save");
    }
}
exports.ResourcePage = ResourcePage;
