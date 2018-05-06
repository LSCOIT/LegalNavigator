import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Topic } from '../topics-resources/topic';


@Injectable()
export class TopicService {

  topicUrl = "http://localhost:64218/api/Topics/Get";
  topicContentUrl = "http://localhost:64218/api/Topics/GetContent";

  constructor(private http: HttpClient) { }

  getTopics(): Observable<Topic> {
    return this.http.get<Topic>(this.topicUrl);
  }

  getTopicDetail(name): Observable<any> {
    return this.http.get<Topic>(this.topicContentUrl, {params: name});
  }
}
