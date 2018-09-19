import { $, by, element, browser, protractor } from "protractor";
let EC = protractor.ExpectedConditions;

export class LocationComponent {
    public bingMap = $('modal-container');
    public inputBox = $('#search-box');
    public searchButton = $('.search-btn');
    public updateButton = element(by.buttonText('Update'));
    public userLocation = $('app-map span');
    public changeLocationButton = $(".change-location-btn");

    public async enterLocation(stateName: string) {
        await this.inputBox.sendKeys(stateName);
        await this.searchButton.click();
        // Wait for bing map pin to set
        // Cannot find a better way yet to detect pin has been set
        await browser.sleep(2000);
        await this.updateButton.click();
        // Wait for bing map to disappear
        await browser.wait((EC.stalenessOf(this.bingMap)), 5000);
    }

    public async confirmUserLocation(stateName: string) {
        await browser.wait(EC.textToBePresentInElement(this.userLocation, stateName), 5000);
    }
} 