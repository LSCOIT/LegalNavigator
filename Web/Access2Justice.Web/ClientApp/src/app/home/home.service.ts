import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../api/api';

@Injectable()
export class HomeService {

  constructor(
    private http: HttpClient
  ) { }

  getHomeContent(): Observable<any> {
    return this.http.get<any>('./assets/SampleJsons/HomeStaticResource.json');
  }
}
