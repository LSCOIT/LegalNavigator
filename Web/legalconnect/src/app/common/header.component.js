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
//import { Popup } from 'ng2-opd-popup';
var HeaderComponent = (function () {
    function HeaderComponent(router) {
        this.router = router;
        this.showDiv = false;
    }
    HeaderComponent.prototype.dispalyDiv = function () {
        this.showDiv = true;
    };
    HeaderComponent.prototype.returnToChat = function () {
        localStorage.setItem('returnFlag', 'true');
        // this.router.navigate(['/chat'] , { queryParams: { returnFlag: true } });
    };
    return HeaderComponent;
}());
HeaderComponent = __decorate([
    core_1.Component({
        selector: 'headerDiv',
        templateUrl: './header.component.html'
    }),
    __metadata("design:paramtypes", [router_1.Router])
], HeaderComponent);
exports.HeaderComponent = HeaderComponent;
//# sourceMappingURL=header.component.js.map