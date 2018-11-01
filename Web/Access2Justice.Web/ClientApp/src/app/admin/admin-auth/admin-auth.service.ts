import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Global } from '../../global';
import { MsalService } from '@azure/msal-angular';
import { LoginService } from '../../shared/login/login.service';

@Injectable({
  providedIn: 'root',
})
export class AdminAuthService {
  isAdmin;
  redirectUrl: string;

  constructor(
    private global: Global,
    private msalService: MsalService,
    private loginService: LoginService
  ) { }

  checkIfAdmin() {
    let userData = this.msalService.getUser();
    let userProfile = {
      name: userData.idToken['name'], firstName: "", lastName: "", oId: userData.idToken['oid'], eMail: userData.idToken['preferred_username'], isActive: "Yes",
      createdBy: userData.idToken['name'], createdTimeStamp: (new Date()).toUTCString(), modifiedBy: userData.idToken['name'], modifiedTimeStamp: (new Date()).toUTCString()
    }
    let subject = new Subject<boolean>();

    this.loginService.upsertUserProfile(userProfile)
      .subscribe(response => {
          subject.next(true);
        },
        error => subject.next(false)
      );
    return subject;
  }
}
