"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const protractor_1 = require("protractor");
const chai = require("chai").use(require("chai-as-promised"));
const expect = chai.expect;
const assert = chai.assert;
var request = require('request');
var rp = require('request-promise');
class LinkComponent {
    constructor() {
        this.links = protractor_1.$$("a[href]");
    }
    checkLinks() {
        return this.links.each((link) => {
            return link.getAttribute("href").then(function (url) {
                var options = {
                    method: 'GET',
                    uri: url,
                    resolveWithFullResponse: true
                };
                console.log(options.uri);
                return rp(options).then(function (response) {
                    console.log('The response code for ' + url + ' is ' + response.statusCode);
                    return response.statusCode === 200;
                });
            });
        });
    }
}
exports.LinkComponent = LinkComponent;
