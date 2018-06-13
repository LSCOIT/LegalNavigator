import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()

export class MapResultsService  {
  constructor(private http: HttpClient) {
  }

  getLocationDetails(address, credentials): any {
    let searchRequest = 'https://dev.virtualearth.net/REST/v1/Locations/' + encodeURI(address) + '?output=json&key=' + credentials;
    return this.http.get(searchRequest);
  }
}


