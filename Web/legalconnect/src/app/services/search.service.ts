import { Injectable } from '@angular/core';
import { Http, Jsonp, URLSearchParams, Headers, RequestOptions, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class SearchService {
    constructor(private _http: Http) {

    }
    //private searchUrl = 'https://api.cognitive.microsoft.com/bing/v5.0/search';  
    private searchUrl = 'http://contentsextractionapiother.azurewebsites.net/api/ExtractCrawledContents';
    private chatMessageUrl = 'http://contentsextractionapiother.azurewebsites.net/api/ExtractCrawledContents';
    private chatRefUrl = 'http://contentsextractionapiother.azurewebsites.net/api/ExtractSubTopics';
    private chatFileUploadUrl = 'http://contentsextractionapiother.azurewebsites.net/api/ExtractTextsFromHttpFileBase';
    private curatedContentsUrl = 'http://contentsextractionapiother.azurewebsites.net/api/ExtractCuratedContents';
    private getCuratedScenarios = 'http://contentsextractionapiother.azurewebsites.net/api/ExtractCuratedContents'
    private tokenUrl = 'https://api.cognitive.microsoft.com/sts/v1.0/issueToken';
    private translateUrl = 'https://api.microsofttranslator.com/V2/Http.svc/Translate';
    private qnaMakerUrl = 'https://westus.api.cognitive.microsoft.com/qnamaker/v2.0/knowledgebases/afd490ef-3a14-4284-a8d8-a46d05829f35/generateAnswer'
    geoCountry: any;

    output: any;

    public getChatMessages(query: string, lang: string, country: string): Observable<any> {


        //return this._http.post(this.chatMessageUrl, { Topic: query, Title: query, State: country }).map((res: Response) => (res.json()));
        return this._http.post(this.chatMessageUrl, { Topic: query.trim(), Title: query.trim(), State: country }).map((res: Response) => (res.json()));
    }


    public getChatReferences(query: string, country: string): Observable<any> {

        return this._http.post(this.chatRefUrl, { Sentence: query.trim(), State: country}).map((res: Response) => (res.json()));

    }

    public getfileUpload(data: FormData): Observable<any> {
        var headers = new Headers();
        headers.append('contentType', 'false');
        headers.append('processData', 'false');
        return this._http.post(this.chatFileUploadUrl, data, { headers: headers }).map((res: Response) => (res));

    }

    public getCuratedContents(query: string, state: string): Observable<any> {

        return this._http.post(this.curatedContentsUrl, { Sentence: query.trim(), State: state }).map((res: Response) => (res.json()));

    }
    public TranslateToken() {
        var headers = new Headers();
        headers.append('Ocp-Apim-Subscription-Key', 'f79c9411ee6d4daba6bb9aff008fe2eb');
        return '12345';// this._http.post(this.tokenUrl, { headers:headers }).map((res: Response) => (res.json()));
    }
    public TranslateText(query: string, lang: string): Observable<any> {

        var token = this.TranslateToken();
        console.log('token', token);
        var appid = "Bearer" + token;
        this.translateUrl = this.translateUrl + '?Text=' + query + '&from=en&to=' + lang + '&appid=' + appid;
        //return this._http.get(this.translateUrl, { params: params }).map((res: Response) => (res.json()));
        return this._http.get(this.translateUrl).map((res: Response) => (res.json()));
    }

    public getLocation(): Observable<any> {
        return this._http.get("http://ipinfo.io").map((res: Response) => res.json());

    }

    public getCurScenarios(ScenarioId: string, State: string): Observable<any> {
        return this._http.get(this.getCuratedScenarios + '?scenarioId=' + ScenarioId + '&state=' + State).map((res: Response) => res.json());

    }
    public getQnAMaker(query: string): Observable<any> {

        var headers = new Headers();
        headers.append('Ocp-Apim-Subscription-Key', '90b5ba1264c54b78ae57d7d6095b2f0b');
        headers.append('Content-Type', 'application/json');
        return this._http.post(this.qnaMakerUrl, { question: query.trim() }, { headers: headers }).map((res: Response) => (res.json()));

    }

}
