import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MsalService } from "@azure/msal-angular";

import ENV from 'env';
import { api } from "../../../api/api";

const httpOptions = {
  headers: new HttpHeaders({ "Content-Type": "application/json" })
};

@Injectable()
export class LoginService {
  constructor(private http: HttpClient, private msalService: MsalService) {}

  getUserProfile() {
    let userData = this.msalService.getUser();
    if (!userData) {
      this.msalService.loginRedirect(ENV().consentScopes);
    } else {
      let userProfile = {
        name: userData.idToken["name"],
        firstName: "",
        lastName: "",
        oId: userData.idToken["oid"],
        eMail: userData.idToken["preferred_username"],
        isActive: "Yes",
        createdBy: userData.idToken["name"],
        createdTimeStamp: new Date().toUTCString(),
        modifiedBy: userData.idToken["name"],
        modifiedTimeStamp: new Date().toUTCString()
      };
      return this.http.post<any>(
        api.upsertUserProfileUrl,
        userProfile,
        httpOptions
      );
    }
  }
}
