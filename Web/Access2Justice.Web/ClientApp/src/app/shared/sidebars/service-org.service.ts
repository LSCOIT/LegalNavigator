import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders  } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Organization } from '../sidebars/organization';
import { api } from '../../../api/api';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
 };
@Injectable()
export class ServiceOrgService {
  constructor(
    private http: HttpClient
  ) {}

  getOrganizationDetails(location): Observable<any>{
    var objectToSend = JSON.stringify(location);
    return this.http.post<Organization>(api.getOrganizationDetailsUrl, objectToSend, httpOptions);
  }
}
