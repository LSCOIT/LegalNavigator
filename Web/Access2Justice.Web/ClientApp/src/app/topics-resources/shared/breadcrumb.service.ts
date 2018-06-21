import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';

@Injectable()

export class BreadcrumbService {

  constructor(private http: HttpClient) { }
  
  getBreadcrumbs(breadcrumbId: string): Observable<any> {
    return this.http.get(api.breadcrumbsUrl + '/' +  breadcrumbId);
  }
}
