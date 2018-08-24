import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Topic, ITopicInput } from './topic';
import { api } from '../../../api/api';
import { MapLocation } from '../../shared/map/map';
import { Location } from '../.././shared/navigation/navigation';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class TopicService {
  constructor(private http: HttpClient) { }

  topicInput: ITopicInput = { Id:'', Location: '' };
  mapLocation: MapLocation = { state: '', city: '', county: '', zipCode: '' };

  loadStateName(): MapLocation {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      return this.mapLocation;
    }
  }
  getTopics(): Observable<any> {
    this.mapLocation = this.loadStateName();
    return this.http.post<Topic>(api.topicUrl, JSON.stringify(this.mapLocation), httpOptions);
  }
  getSubtopics(id): Observable<any> {
    this.topicInput.Id = id;
    this.topicInput.Location = this.loadStateName();
    return this.http.post<Topic>(api.subtopicUrl, JSON.stringify(this.topicInput), httpOptions);
  
  }
  getSubtopicDetail(id): Observable<any> {
    this.topicInput.Id = id;
    this.topicInput.Location = this.loadStateName();
    return this.http.post<Topic>(api.subtopicDetailUrl, JSON.stringify(this.topicInput), httpOptions);   
  }

  getDocumentData(id): Observable<any> {
    this.topicInput.Id = id;
    this.topicInput.Location = this.loadStateName();
    return this.http.post<Topic>(api.getDocumentUrl, JSON.stringify(this.topicInput), httpOptions);
  }
}
