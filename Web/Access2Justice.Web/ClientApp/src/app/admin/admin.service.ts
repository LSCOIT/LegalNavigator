import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { api } from '../../api/api';

@Injectable()
export class AdminService {

  constructor(private httpClient: HttpClient) { }

  savePrivacyData(input: any) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.httpClient.post(api.updatePrivacyData, input, httpOptions);
  }

}
