import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';

import ENV from 'environment';
import { Global, UserStatus } from '../../global';
import { Login } from '../navigation/navigation';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
  @Input() login: Login;
  userProfileName: string;
  isLoggedIn = false;
  blobUrl: any = ENV.blobUrl;
  @Output() sendProfileOptionClickEvent = new EventEmitter<string>();
  userProfile: any;
  isAdmin = false;
  roleInformationSubscription;
  isMobile = false;

  constructor(
    private router: Router,
    private global: Global,
    private msalService: MsalService
  ) {
  }

  onProfileOptionClick() {
    this.sendProfileOptionClickEvent.emit();
  }

  profile() {
    if (
      this.global.role === UserStatus.Shared &&
      location.pathname.indexOf(this.global.shareRouteUrl) >= 0
    ) {
      location.href = this.global.profileRouteUrl;
    } else {
      this.router.navigate([this.global.profileRouteUrl]);
    }
  }

  externalLogin(event) {
    event.preventDefault();
    this.global.isLoginRedirect = true;
    this.msalService.loginRedirect(ENV.consentScopes);
  }

  logout() {
    this.msalService.logout();
  }

  checkIfAdmin(roleInformation) {
    roleInformation.forEach(role => {
      if (role.roleName.includes('Admin') || role.roleName === 'Developer') {
        this.isAdmin = true;
      }
    });
  }

  ngOnInit() {
    this.userProfile = this.msalService.getUser();
    if (this.userProfile) {
      this.isLoggedIn = true;
      this.userProfileName = this.userProfile.idToken['name']
        ? this.userProfile.idToken['name']
        : this.userProfile.idToken['preferred_username'];
    }

    this.roleInformationSubscription = this.global.notifyRoleInformation.subscribe(
      value => {
        this.checkIfAdmin(value);
      }
    );

    if (
      document.getElementById('mobile-menu') &&
      document.getElementById('mobile-menu').style.display !== 'none'
    ) {
      this.isMobile = true;
    }
  }

  ngOnDestroy() {
    if (this.roleInformationSubscription) {
      this.roleInformationSubscription.unsubscribe();
    }
  }
}
