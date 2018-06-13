import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Organization } from '../sidebars/organization';

@Injectable()
export class ServiceOrgService {

  siteHostName = "http://localhost:59704/";
  getOrganizationDetailsUrl = this.siteHostName + "api/topics/getorganizationdetails";


  constructor(private http: HttpClient) { }

  getOrganizationDetail(value): Observable<any> {
    return this.http.get<Organization>(this.getOrganizationDetailsUrl + '/' + value);
  }

}
