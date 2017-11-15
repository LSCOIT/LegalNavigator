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
var search_service_1 = require("../../services/search.service");
var ChatComponent = (function () {
    function ChatComponent(srchServ, router, route) {
        this.srchServ = srchServ;
        this.router = router;
        this.route = route;
        this.lang = "en";
        this.selectedCountry = " ";
        this.isVisible = true;
        this.showMessage = false;
        this.returnFlag = 'false';
        this.rightboxes = [{
                "url": "",
                "text": "",
                "self": false
            }];
        this.messages = [{
                "text": "",
                "self": false,
                "time": "",
                "class": "",
            }];
        this.returnMessages = [{
                "text": "",
                "self": false,
                "time": "",
                "class": "",
            }];
        this.replyMessage = "";
        this.rightmessMessage = "";
    }
    ChatComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.returnFlag = localStorage.getItem('returnFlag');
        var dt = new Date();
        var time = dt.getHours() + ":" + dt.getMinutes();
        setTimeout(function () {
            time = dt.getHours() + ":" + dt.getMinutes();
            _this.returnMessages.push({
                "text": "I have an eviction notice",
                "self": false,
                "time": time,
                "class": "receive",
            });
        }, 500);
        setTimeout(function () {
            time = dt.getHours() + ":" + dt.getMinutes();
            _this.returnMessages.push({
                "text": "do you need assistance with it",
                "self": true,
                "time": time,
                "class": "sent",
            });
        }, 500);
        setTimeout(function () {
            time = dt.getHours() + ":" + dt.getMinutes();
            _this.returnMessages.push({
                "text": "yes, I don’t understand it",
                "self": false,
                "time": time,
                "class": "receive",
            });
        }, 500);
        setTimeout(function () {
            time = dt.getHours() + ":" + dt.getMinutes();
            _this.returnMessages.push({
                "text": "use the camera icon to take a picture of it",
                "self": true,
                "time": time,
                "class": "sent",
            });
        }, 500);
    };
    ChatComponent.prototype.onChange = function (files) {
        var _this = this;
        this.files = files;
        var file;
        for (var i = 0; i < files.length; i++) {
            file = files[i];
            this.rightboxes.push({
                "url": "",
                "text": files[i].name,
                "self": true
            });
            this.showMessage = true;
            var reader = new FileReader();
            var data = new FormData();
            data.append('file-' + i, file);
            this.srchServ.getfileUpload(data).subscribe(function (res) {
                console.log(res._body);
                var dt = new Date();
                var time = dt.getHours() + ":" + dt.getMinutes();
                _this.messages.push({
                    "text": res._body,
                    "self": true,
                    "time": time,
                    "class": "receive"
                });
                _this.showMessage = false;
            }, function (err) {
                _this.showMessage = false;
                console.log(err);
            });
        }
    };
    ChatComponent.prototype.reply = function () {
        //  document.getElementById('chat_msgs').scrollIntoView();
        var _this = this;
        //document.getElementById('resources').scrollIntoView();
        this.isVisible = false;
        var dt = new Date();
        var time = dt.getHours() + ":" + dt.getMinutes();
        this.messages.push({
            "text": this.replyMessage,
            "self": true,
            "time": time,
            "class": "sent",
        });
        var query = this.replyMessage;
        this.showMessage = true;
        this.srchServ.getLocation().subscribe(function (resCountry) {
            _this.geoCountry = resCountry.region;
            _this.srchServ.getChatMessages(query, _this.lang, _this.selectedCountry == " " ? _this.geoCountry : _this.selectedCountry).subscribe(function (res) {
                var dt = new Date();
                var time = dt.getHours() + ":" + dt.getMinutes();
                _this.messages.push({
                    "text": res,
                    "self": true,
                    "time": time,
                    "class": "receive"
                });
                _this.showMessage = false;
                localStorage.setItem('scrolled', 'false');
                localStorage.setItem('resScrolled', 'false');
            }, function (err) {
                _this.showMessage = false;
                alert('No data found for location ' + _this.geoCountry + ' choose country from dropdown');
            });
            _this.rightboxes = [];
            _this.srchServ.getChatReferences(query, _this.selectedCountry == " " ? _this.geoCountry : _this.selectedCountry).subscribe(function (res) {
                for (var i = 0; i < res.length; i++) {
                    _this.rightboxes.push({
                        "url": res[i].Url,
                        "text": res[i].Name,
                        "self": true
                    });
                }
            });
        });
        this.isVisible = true;
        this.replyMessage = "";
        setInterval(this.updateScroll, 100);
        setInterval(this.updateResScroll, 100);
    };
    ChatComponent.prototype.updateScroll = function () {
        if (localStorage.getItem('scrolled') == 'false') {
            var element = document.getElementById('chat_msgs');
            element.scrollTop = element.scrollHeight;
        }
    };
    ChatComponent.prototype.updateResScroll = function () {
        if (localStorage.getItem('resScrolled') == 'false') {
            var element = document.getElementById('chat_resources');
            element.scrollTop = element.scrollHeight;
        }
    };
    ChatComponent.prototype.onChatScroll = function () {
        localStorage.setItem('scrolled', 'true');
    };
    ChatComponent.prototype.onResScroll = function () {
        localStorage.setItem('resScrolled', 'true');
    };
    ChatComponent.prototype.displayText = function (lang) {
        this.lang = lang;
    };
    ChatComponent.prototype.openPop = function (Url) {
        this.location = Url;
    };
    ChatComponent.prototype.closePop = function () {
        window.open(this.location, '_blank');
    };
    return ChatComponent;
}());
ChatComponent = __decorate([
    core_1.Component({
        selector: 'chat',
        templateUrl: './chat.component.html'
    }),
    __metadata("design:paramtypes", [search_service_1.SearchService, router_1.Router, router_1.ActivatedRoute])
], ChatComponent);
exports.ChatComponent = ChatComponent;
//# sourceMappingURL=chat.component.js.map