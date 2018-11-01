import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AdminAuthService } from './admin-auth.service';
import { of } from 'rxjs/observable/of';
import { MsalService } from '@azure/msal-angular';
import { LoginService } from '../../shared/login/login.service';
import { Subject } from 'rxjs';

@Injectable()
export class AdminAuthGuard implements CanActivate {
  userProfile;
  isAdmin;
  userData;

  constructor(
    private adminAuthService: AdminAuthService,
    private router: Router,
    private msalService: MsalService,
    private loginService: LoginService) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot):Observable<any> {
    let url: string = state.url;
    return this.checkAdminStatus(url).map(value => {
      console.log(value);
      return value;
    });
  }

  checkAdminStatus(url: string) {
    let userData = this.msalService.getUser();
    console.log(userData);
    let subject = new Subject<boolean>();

    if (userData) {
      let userProfile = {
        name: userData.idToken['name'],
        firstName: "",
        lastName: "",
        oId: userData.idToken['oid'],
        eMail: userData.idToken['preferred_username'],
        isActive: "Yes",
        createdBy: userData.idToken['name'],
        createdTimeStamp: (new Date()).toUTCString(),
        modifiedBy: userData.idToken['name'],
        modifiedTimeStamp: (new Date()).toUTCString()
      }

      this.loginService.upsertUserProfile(userProfile)
        .subscribe(response => {
            let findAdmin = response.roleInformation.find(role => {
              return role.roleName.includes("Admin") || role.roleName === 'Developer';
            });
            if (findAdmin) {
              subject.next(true);
            } else {
              subject.next(false);
            }
          },
          error => subject.next(false)
        );
    } else {
      console.log("hitting else statement");
      subject.next(false);
    }
    return subject;
  }
}
