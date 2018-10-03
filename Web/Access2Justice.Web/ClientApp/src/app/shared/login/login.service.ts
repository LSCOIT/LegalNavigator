import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { IUserProfile } from '../../shared/login/user-profile.model';
import { api } from '../../../api/api';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class LoginService {

  constructor(private http: HttpClient) { }

  upsertUserProfile(userProfile: IUserProfile) {
    return this.http.post<any>(api.upsertUserProfileUrl, userProfile, httpOptions);
  }

}
