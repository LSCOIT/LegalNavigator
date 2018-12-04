import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { api } from '../../api/api';
import { Global } from '../global';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class StateCodeService {

  constructor(private httpClient: HttpClient) { }

  getStateCodes() {
    return this.httpClient.get(api.getStateCodesUrl, httpOptions);
  }

  getStateCode(stateName) {
    let params = new HttpParams()
      .set("stateName", stateName);
    return this.httpClient.get(api.getStateCodeUrl + '?' + params, httpOptions);
  }
}
