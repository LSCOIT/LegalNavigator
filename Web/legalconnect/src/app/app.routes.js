"use strict";
var router_1 = require("@angular/router");
var search_component_1 = require("./search/search.component");
var demo_component_1 = require("./demo/demo.component");
var general_component_1 = require("./tabs/general/general.component");
var guided_component_1 = require("./tabs/guided/guided.component");
var doc_component_1 = require("./tabs/doc/doc.component");
var chat_component_1 = require("./tabs/chat/chat.component");
var timeline_component_1 = require("./tabs/timeline/timeline.component");
var routes = [
    { path: 'search', component: search_component_1.SearchComponent },
    { path: 'demo', component: demo_component_1.DemoComponent },
    { path: 'general', component: general_component_1.GeneralComponent },
    { path: 'guided', component: guided_component_1.GuidedComponent },
    { path: 'general/:id', component: general_component_1.GeneralComponent },
    { path: 'general/QandA/:id', component: general_component_1.GeneralComponent },
    { path: 'guided/:id', component: guided_component_1.GuidedComponent },
    { path: 'guided/QandA/:id', component: guided_component_1.GuidedComponent },
    { path: 'guided/assist/:id', component: guided_component_1.GuidedComponent },
    { path: 'doc', component: doc_component_1.DocComponent },
    { path: 'chat', component: chat_component_1.ChatComponent },
    { path: 'chat/:id', component: chat_component_1.ChatComponent },
    { path: 'chat/QandA/:id', component: chat_component_1.ChatComponent },
    { path: 'timeline', component: timeline_component_1.TLComponent },
    { path: '', redirectTo: '/search', pathMatch: 'full' },
    { path: '**', component: search_component_1.SearchComponent },
];
exports.routing = router_1.RouterModule.forRoot(routes);
//# sourceMappingURL=app.routes.js.map