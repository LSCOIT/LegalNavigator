import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Topic } from './topic';


@Injectable()
export class TopicService {
  
  topicUrl = "http://localhost:64218/api/topics/get";
  subtopicUrl = "http://localhost:64218/api/topics/getsubtopics";
  subtopicDetailUrl = "http://localhost:64218/api/topics/getsubtopicdetails";

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
}
