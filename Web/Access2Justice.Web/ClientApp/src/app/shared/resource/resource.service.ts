import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';

@Injectable()
export class ResourceService {

  constructor(private http: HttpClient) { }

  getResource(params): Observable<any> {
    return this.http.get<any>(api.resourceUrl + '/' + params);
  }
}
