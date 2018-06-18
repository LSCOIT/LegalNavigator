import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Topic } from './topic';
import { topicApi } from '../../../api/api';

@Injectable()

export class TopicService {  

  constructor(private http: HttpClient) { }

  getTopics(): Observable<any> {
    return this.http.get<Topic>(topicApi.topicUrl);
  }

  getSubtopics(id): Observable<any> {
    return this.http.get<Topic>(topicApi.subtopicUrl +'/'+ id);
  }

  getSubtopicDetail(id): Observable<any> {
    return this.http.get<Topic>(topicApi.subtopicDetailUrl + '/' + id);
  }

  getDocumentData(id): Observable<any> {
    return this.http.get<Topic>(topicApi.getDocumentUrl + '/' + id);
  }
}
