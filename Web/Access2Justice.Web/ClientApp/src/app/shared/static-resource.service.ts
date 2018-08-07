import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../api/api';

@Injectable()
export class StaticResourceService {

  constructor(private httpClient: HttpClient) { }

  getStaticContents(name): Observable<any> {
    return this.httpClient.get<any>(api.getContentUrl + '/' + name);
  }

  getStaticContent(homePageRequest) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.httpClient.post(api.getContentUrl, homePageRequest, httpOptions);
  }

}
