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
var search_service_1 = require("../services/search.service");
var DemoComponent = (function () {
    function DemoComponent(router, srchServ) {
        this.router = router;
        this.srchServ = srchServ;
        //http.get('https://api.cognitive.microsoft.com/bing/v5.0/search?q=I am beign evictied&count=10&offset=0&mkt=en-us&safesearch=Moderate')
        // Call map on the response observable to get the parsed people object
        //.map(res => res.json())
        // Subscribe to the observable to get the parsed people object and attach it to the
        // component
        //  .subscribe(people => this.people = people);
    }
    return DemoComponent;
}());
DemoComponent = __decorate([
    core_1.Component({
        selector: 'demo',
        templateUrl: './demo.component.html'
    }),
    __metadata("design:paramtypes", [router_1.Router, search_service_1.SearchService])
], DemoComponent);
exports.DemoComponent = DemoComponent;
//# sourceMappingURL=demo.component.js.map