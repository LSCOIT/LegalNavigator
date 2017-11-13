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
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
var SearchService = (function () {
    function SearchService(_http) {
        this._http = _http;
        //private searchUrl = 'https://api.cognitive.microsoft.com/bing/v5.0/search';  
        this.searchUrl = 'http://contentsextractionapi.azurewebsites.net/api/ExtractContents';
        this.chatMessageUrl = 'http://contentsextractionapi.azurewebsites.net/api/ExtractContents';
        this.chatRefUrl = 'http://contentsextractionapi.azurewebsites.net/api/ExtractSubTopics';
        this.chatFileUploadUrl = 'http://contentsextractionapi.azurewebsites.net/api/ExtractTextsFromHttpFileBase';
        this.tokenUrl = 'https://api.cognitive.microsoft.com/sts/v1.0/issueToken';
        this.translateUrl = 'https://api.microsofttranslator.com/V2/Http.svc/Translate';
    }
    SearchService.prototype.getChatMessages = function (query, lang, country) {
        return this._http.post(this.chatMessageUrl, { Topic: query, Title: query, State: country }).map(function (res) { return (res.json()); });
    };
    SearchService.prototype.getChatReferences = function (query, country) {
        console.log(country);
        return this._http.post(this.chatRefUrl, { Sentence: query, State: country }).map(function (res) { return (res.json()); });
    };
    SearchService.prototype.getfileUpload = function (data) {
        var headers = new http_1.Headers();
        headers.append('contentType', 'false');
        headers.append('processData', 'false');
        return this._http.post(this.chatFileUploadUrl, data, { headers: headers }).map(function (res) { return (res); });
    };
    SearchService.prototype.TranslateToken = function () {
        var headers = new http_1.Headers();
        headers.append('Ocp-Apim-Subscription-Key', 'f79c9411ee6d4daba6bb9aff008fe2eb');
        return '12345'; // this._http.post(this.tokenUrl, { headers:headers }).map((res: Response) => (res.json()));
    };
    SearchService.prototype.TranslateText = function (query, lang) {
        var token = this.TranslateToken();
        console.log('token', token);
        var appid = "Bearer" + token;
        this.translateUrl = this.translateUrl + '?Text=' + query + '&from=en&to=' + lang + '&appid=' + appid;
        //return this._http.get(this.translateUrl, { params: params }).map((res: Response) => (res.json()));
        return this._http.get(this.translateUrl).map(function (res) { return (res.json()); });
    };
    SearchService.prototype.getLocation = function () {
        return this._http.get("http://ipinfo.io").map(function (res) { return res.json(); });
    };
    return SearchService;
}());
SearchService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], SearchService);
exports.SearchService = SearchService;
//# sourceMappingURL=search.service.js.map