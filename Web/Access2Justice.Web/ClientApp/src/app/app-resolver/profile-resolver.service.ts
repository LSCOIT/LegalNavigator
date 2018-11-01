import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { IUserProfile } from '../shared/login/user-profile.model';
import { Global } from '../global';
import { MsalService } from '@azure/msal-angular';
import { Observable } from 'rxjs/Observable';
import { api } from '../../api/api';
import { LoginService } from '../shared/login/login.service';


@Injectable()
export class ProfileResolver implements Resolve<any> {

  userProfile: IUserProfile;

  constructor(private global: Global,
              private msalService: MsalService,
              private loginService: LoginService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    return this.loginService.getUserProfile();
  }
}
