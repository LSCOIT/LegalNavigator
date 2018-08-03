import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';

@Injectable()
export class StaticResourceService {

  constructor(
    private http: HttpClient
  ) { }

  getStaticContents(name): Observable<any> {
    return this.http.get<any>(api.getContentUrl + '/' + name);
  }
}
