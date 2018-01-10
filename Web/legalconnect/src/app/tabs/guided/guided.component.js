"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var GuidedComponent = (function () {
    function GuidedComponent(router, _el) {
        this.router = router;
        this._el = _el;
        this.text = "In this section you will find general legal information, self - help packets and videos on your rights as a<br />renter and what to do if you have a problem with your landlord.<a > http://www.washingtonlawhelp.org/issues/housing/tenants-rights</a>";
        this.aciveBdg = 'tenant';
        this.OneClass = "collapse";
        this.TwoClass = "collapse";
        this.ThreeClass = "collapse";
        this.FourClass = "collapse";
        this.routeUrl = "";
        this.currentUrl = '';
        this.activeSld = "Slide1";
        this.myClass = "aciveSlide";
        this.ctrType = "button";
    }
    GuidedComponent.prototype.ngOnInit = function () {
        this.currentUrl = this.router.url; // this will give you current url
        if (this.currentUrl == '/guided/assist/evictionnotice')
            this.activeSld = "Slide2";
        else if (this.currentUrl == '/guided/assist/housingproblem')
            this.activeSld = "Slide5";
        else if (this.currentUrl == '/guided/assist/begin')
            this.activeSld = "Slide1";
        else if (this.currentUrl == '/guided/assist/aboutnotice')
            this.activeSld = "Slide3";
        else if (this.currentUrl == '/guided/assist/assistnotice')
            this.activeSld = "Slide4";
    };
    GuidedComponent.prototype.ShowNextNavigation = function (sld, title) {
        this.activeSld = sld;
        this.router.navigate(['/guided/assist', title]);
    };
    GuidedComponent.prototype.ShowSkipNavigation = function (sld, title) {
        this.activeSld = sld;
        this.router.navigate(['/guided/assist', title]);
    };
    GuidedComponent.prototype.ValidateContent = function (param) {
        this.myClass = param;
    };
    GuidedComponent.prototype.redirectToForms = function () {
        document.getElementById('forms_steps').scrollIntoView();
        this.router.navigate(['/guided', 'forms']);
    };
    return GuidedComponent;
}());
GuidedComponent = __decorate([
    core_1.Component({
        selector: 'guided',
        templateUrl: './guided.component.html',
    }),
    __metadata("design:paramtypes", [router_1.Router, core_1.ElementRef])
], GuidedComponent);
exports.GuidedComponent = GuidedComponent;
//# sourceMappingURL=guided.component.js.map