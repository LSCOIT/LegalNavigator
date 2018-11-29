import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';
import { MapLocation, LocationDetails } from '../map/map';
import { ITopicInput } from '../../topics-resources/shared/topic';
import { Global } from '../../global';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class ResourceService {
  topicInput: ITopicInput = { Id: '', Location: '', IsShared: false };
  mapLocation: MapLocation = { state: '', city: '', county: '', zipCode: '' };
  locationDetails: LocationDetails;

  constructor(private http: HttpClient, private global:Global) { }

  loadStateName(): MapLocation {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.mapLocation = this.locationDetails.location;
      return this.mapLocation;
    }
  }

  getResource(id): Observable<any> {
    this.topicInput.Id = id;
    this.topicInput.Location = this.loadStateName();
    if (location.pathname.indexOf(this.global.shareRouteUrl) >= 0) {
      this.topicInput.IsShared = true;
    }
    return this.http.post<any>(api.resourceUrl, JSON.stringify(this.topicInput), httpOptions);
  }
}
