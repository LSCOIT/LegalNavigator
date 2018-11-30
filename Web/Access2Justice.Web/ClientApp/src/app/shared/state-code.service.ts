import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { api } from '../../api/api';
import { Global } from '../global';

@Injectable()
export class StateCodeService {

  constructor(private httpClient: HttpClient,
    private global: Global) { }

  getStateCodes() {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.httpClient.get(api.getStateCodesUrl, httpOptions);
  }
}
