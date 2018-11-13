import { Component, OnInit, Input, ElementRef, ViewChild, Output, EventEmitter } from '@angular/core';
import { api } from '../../../api/api';
import { environment } from '../../../environments/environment';
import { Login } from '../navigation/navigation';
import { Router } from '@angular/router';
import { Subscription } from "rxjs/Subscription";
import { Global, UserStatus } from '../../global';
import { MsalService } from '@azure/msal-angular';
import { IUserProfile } from './user-profile.model';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @Input() login: Login;
  userProfileName: string ;
  isLoggedIn: boolean = false;
  blobUrl: any = environment.blobUrl;
  @ViewChild('dropdownMenu') dropdown: ElementRef;
  @Output() sendProfileOptionClickEvent = new EventEmitter<string>();  
  private subscription: Subscription;
  userProfile: any;
  isProfileSaved: boolean = false;
  isAdmin: boolean = false;
  roleInformationSubscription;

  constructor(private router: Router,
              private global: Global,
              private msalService: MsalService,
              private loginService: LoginService) { }

  onProfileOptionClick() {
    this.sendProfileOptionClickEvent.emit();
  }

  profile() {
    if (this.global.role === UserStatus.Shared
      && location.pathname.indexOf(this.global.shareRouteUrl) >= 0) {
      location.href = this.global.profileRouteUrl;
    }
    else {
      this.router.navigate([this.global.profileRouteUrl]);
    }
  }

  toggleDropdown() {
    this.dropdown.nativeElement.style.display =
      this.dropdown.nativeElement.style.display === 'none' ? 'block' : 'none';
  }

  externalLogin() {
    this.global.isLoginRedirect = true;
    this.msalService.loginRedirect(environment.consentScopes);
  }

  logout() {
    this.msalService.logout();
  }

  checkIfAdmin(roleInformation) {
    roleInformation.forEach(role => {
      if (role.roleName.includes("Admin") || role.roleName === 'Developer') {
        this.isAdmin = true;
      }
    });
  }

  ngOnInit() {
    this.userProfile = this.msalService.getUser();
    if (this.userProfile) {
      this.isLoggedIn = true;
      this.userProfileName = this.userProfile.idToken['name'] ? this.userProfile.idToken['name'] : this.userProfile.idToken['preferred_username'];
    }

    this.roleInformationSubscription = 
      this.global.notifyRoleInformation
      .subscribe(value => {
        this.checkIfAdmin(value);
      });
  }

  ngOnDestroy() {
    if (this.roleInformationSubscription) {
      this.roleInformationSubscription.unsubscribe();
    }
  }
}
