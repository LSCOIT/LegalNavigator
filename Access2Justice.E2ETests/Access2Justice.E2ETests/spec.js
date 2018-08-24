"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const protractor_1 = require("protractor");
describe('first test', () => {
    beforeEach(() => {
        protractor_1.browser.get(protractor_1.browser.baseUrl);
    });
    it('should display the title', () => {
        let title = protractor_1.element(protractor_1.by.id('first-logo'));
        expect(title.getText()).toEqual('Legal');
    });
});
//# sourceMappingURL=spec.js.map