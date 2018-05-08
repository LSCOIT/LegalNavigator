//import { Injectable } from '@angular/core';
//import { Http } from '@angular/http';
//import { Observable } from 'rxjs/Observable';
//import { of } from 'rxjs/observable/of';
//import 'rxjs/add/operator/map';

//import { Topic } from '../topics-resources/topic';
//import { TOPICS } from './mock-topics';
//import { Response } from '@angular/http/src/static_response';

//@Injectable()
//export class TopicService {

//  topicURL = "http://localhost:59704/api/Topics/Get";
//  contentURL = "http://localhost:59704/api/Topics/GetContent";

//  constructor(private http: Http) { }

//  getTopics()
//  {
//    return this.http.get(this.topicURL).map((response: Response) => response.json());
//  }

//  getcontent(name: string) {
//    return this.http.get(this.contentURL + '?name=' + name).map((response: Response) => response.json());
//  }
//}
