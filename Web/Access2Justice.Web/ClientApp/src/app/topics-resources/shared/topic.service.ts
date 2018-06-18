import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Topic } from './topic';
import { api } from '../../../api/api';

@Injectable()

export class TopicService {  

  constructor(private http: HttpClient) { }

  getTopics(): Observable<any> {
    return this.http.get<Topic>(api.topicUrl);
  }

  getSubtopics(id): Observable<any> {
    return this.http.get<Topic>(api.subtopicUrl +'/'+ id);
  }

  getSubtopicDetail(id): Observable<any> {
    return this.http.get<Topic>(api.subtopicDetailUrl + '/' + id);
  }

  getDocumentData(id): Observable<any> {
    return this.http.get<Topic>(api.getDocumentUrl + '/' + id);
  }
}
