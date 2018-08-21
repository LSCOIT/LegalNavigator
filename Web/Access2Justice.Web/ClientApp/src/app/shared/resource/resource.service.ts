import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';
import { MapLocation } from '../map/map';
import { ITopicInput } from '../../topics-resources/shared/topic';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class ResourceService {
  topicInput: ITopicInput = { Id: '', Location: '' };
  mapLocation: MapLocation = { state: '', city: '', county: '', zipCode: '' };
  constructor(private http: HttpClient) { }

  loadStateName(): MapLocation {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      return this.mapLocation;
    }
  }

  getResource(id): Observable<any> {
    this.topicInput.Id = id;
    this.topicInput.Location = this.loadStateName();
    return this.http.post<any>(api.resourceUrl, JSON.stringify(this.topicInput), httpOptions);
  }
}
