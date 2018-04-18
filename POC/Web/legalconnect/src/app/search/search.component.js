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
var SearchComponent = (function () {
    function SearchComponent(router) {
        this.router = router;
        this.arrayOfStrings = ["I am being evicted",
            "How to fight for eviction",
            "what to do when you get an eviction notice",
            "can an eviction be reversed",
            "how can i prolong an eviction",
            "how to drag out an eviction",
            "getting evicted nowhere to go",
            "getting evicted for not paying rent",
            "stop eviction process",
            "how an eviction works",
            "how long does it take to evict someone",
            "how an eviction affects you",
            "apartment eviction process",];
    }
    SearchComponent.prototype.redirect = function () {
        localStorage.setItem('returnFlag', 'false');
        this.router.navigateByUrl("demo");
    };
    SearchComponent.prototype.valueChanged = function (newVal) {
        this.searchText = newVal;
        console.log("Case 2: value is changed to ", newVal);
    };
    return SearchComponent;
}());
SearchComponent = __decorate([
    core_1.Component({
        selector: 'search',
        templateUrl: './search.component.html',
        styles: ["\n    ng2-auto-complete, input {\n      display: block; border: 1px solid #ccc; width: 300px;\n    }\n  "]
    }),
    __metadata("design:paramtypes", [router_1.Router])
], SearchComponent);
exports.SearchComponent = SearchComponent;
//# sourceMappingURL=search.component.js.map