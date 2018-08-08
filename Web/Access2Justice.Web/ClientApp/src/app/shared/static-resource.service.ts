import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../api/api';
import { MapLocation } from './location/location';

@Injectable()
export class StaticResourceService {

  constructor(private httpClient: HttpClient) { }

  mapLocation: MapLocation;
  state: string;

  loadStateName(): MapLocation {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.state = this.mapLocation.address;
      return this.mapLocation;
    }
  }

  getStaticContent(pageRequest) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    pageRequest.location = this.loadStateName(); 
    return this.httpClient.post(api.getContentUrl, pageRequest, httpOptions);
  }
}
