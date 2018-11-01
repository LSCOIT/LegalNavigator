import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { IUserProfile } from '../../shared/login/user-profile.model';
import { api } from '../../../api/api';
import { MsalService } from '@azure/msal-angular';
import { environment } from '../../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class LoginService {

  constructor(
    private http: HttpClient,
    private msalService: MsalService
  ) { }

  getUserProfile() {
    let userData = this.msalService.getUser();
    if (!userData) {
      this.msalService.loginRedirect(environment.consentScopes);
    } else {
      let userProfile = {
        name: userData.idToken['name'], firstName: "", lastName: "", oId: userData.idToken['oid'], eMail: userData.idToken['preferred_username'], isActive: "Yes",
        createdBy: userData.idToken['name'], createdTimeStamp: (new Date()).toUTCString(), modifiedBy: userData.idToken['name'], modifiedTimeStamp: (new Date()).toUTCString()
      }
      return this.http.post<any>(api.upsertUserProfileUrl, userProfile, httpOptions);
    }
  }
}
