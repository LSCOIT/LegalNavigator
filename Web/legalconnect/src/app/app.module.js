"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/http");
var app_routes_1 = require("./app.routes");
var ng2_auto_complete_1 = require("ng2-auto-complete");
var app_component_1 = require("./app.component");
var search_component_1 = require("./search/search.component");
var demo_component_1 = require("./demo/demo.component");
var header_component_1 = require("./common/header.component");
var mrs_component_1 = require("./common/mrs.component");
var footer_component_1 = require("./common/footer.component");
var general_component_1 = require("./tabs/general/general.component");
var guided_component_1 = require("./tabs/guided/guided.component");
var doc_component_1 = require("./tabs/doc/doc.component");
var chat_component_1 = require("./tabs/chat/chat.component");
var timeline_component_1 = require("./tabs/timeline/timeline.component");
var search_service_1 = require("./services/search.service");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [
            platform_browser_1.BrowserModule,
            forms_1.FormsModule,
            http_1.HttpModule,
            http_1.JsonpModule,
            app_routes_1.routing,
            ng2_auto_complete_1.Ng2AutoCompleteModule
        ],
        declarations: [app_component_1.AppComponent,
            search_component_1.SearchComponent,
            demo_component_1.DemoComponent,
            header_component_1.HeaderComponent,
            mrs_component_1.MRSComponent,
            footer_component_1.FooterComponent,
            general_component_1.GeneralComponent,
            guided_component_1.GuidedComponent,
            doc_component_1.DocComponent,
            chat_component_1.ChatComponent,
            timeline_component_1.TLComponent
        ],
        providers: [
            search_service_1.SearchService
        ],
        bootstrap: [app_component_1.AppComponent]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map