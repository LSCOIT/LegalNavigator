import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';

@Injectable()

export class BreadCrumbService {

  constructor(private http: HttpClient) { }
  
  getBreadCrumbs(breadCrumbId: string): Observable<any> {
    return this.http.get(api.breadCrumbsUrl + '/' +  breadCrumbId);
  }
}
