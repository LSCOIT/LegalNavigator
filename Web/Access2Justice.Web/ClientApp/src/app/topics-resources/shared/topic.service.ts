import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Topic } from './topic';

@Injectable()

export class TopicService {  

  siteHostName = "http://localhost:7150/";
  topicUrl = this.siteHostName + "api/topics/gettopics";
  subtopicUrl = this.siteHostName + "api/topics/getsubtopics";
  subtopicDetailUrl = this.siteHostName + "api/topics/getresourcedetails";
  getDocumentUrl = this.siteHostName + "api/topics/getdocument";

  constructor(private http: HttpClient) { }

  getTopics(): Observable<any> {
    return this.http.get<Topic>(this.topicUrl);
  }

  getSubtopics(id): Observable<any> {
    return this.http.get<Topic>(this.subtopicUrl+'/'+ id);
  }

  getSubtopicDetail(id): Observable<any> {
    return this.http.get<Topic>(this.subtopicDetailUrl + '/' + id);
  }

  getDocumentData(id): Observable<any> {
    return this.http.get<Topic>(this.getDocumentUrl + '/' + id);
  }
}
