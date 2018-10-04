import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { IUserProfile } from '../shared/login/user-profile.model';
import { Global } from '../global';
import { MsalService } from '@azure/msal-angular';
import { Observable } from 'rxjs/Observable';
import { api } from '../../api/api';
import { LoginService } from '../shared/login/login.service';


@Injectable()
export class ProfileResolverService implements Resolve<any> {

  userProfile: IUserProfile;

  constructor(private global: Global,
              private msalService: MsalService,
              private loginService: LoginService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {

    let userData = this.msalService.getUser();
    if (userData) {
      this.userProfile = {
        name: userData.idToken['name'], firstName: "", lastName: "", oId: userData.idToken['oid'], eMail: userData.idToken['preferred_username'], isActive: "Yes",
        createdBy: userData.idToken['name'], createdTimeStamp: (new Date()).toUTCString(), modifiedBy: userData.idToken['name'], modifiedTimeStamp: (new Date()).toUTCString()
      }
      return this.loginService.upsertUserProfile(this.userProfile);
    } else {
      return;
    }
  }

}
