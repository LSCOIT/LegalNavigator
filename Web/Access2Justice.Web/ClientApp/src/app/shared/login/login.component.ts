import { Component, OnInit, Input, ElementRef, ViewChild, Output, EventEmitter } from '@angular/core';
import { api } from '../../../api/api';
import { environment } from '../../../environments/environment';
import { Login } from '../navigation/navigation';
import { Router } from '@angular/router';
import { Subscription } from "rxjs/Subscription";
import { Global, UserStatus } from '../../global';
import { MsalService, BroadcastService } from '@azure/msal-angular';
import { IUserProfile } from './user-profile.model';
import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @Input() login: Login;
  userProfileName: string = "Just Testing";
  isLoggedIn: boolean = false;
  blobUrl: any = environment.blobUrl;
  @ViewChild('dropdownMenu') dropdown: ElementRef;
  @Output() sendProfileOptionClickEvent = new EventEmitter<string>();  
  private subscription: Subscription;
  userProfile: IUserProfile;
  isProfileSaved: boolean = false;

  constructor(private router: Router,
              private global: Global,
              private msalService: MsalService,
              private broadcastService: BroadcastService,
              private personalizedPlanService: PersonalizedPlanService) { }

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
    this.msalService.loginPopup(["user.read"]);    
  }

  logout() {
    sessionStorage.removeItem("profileData");
    this.msalService.logout();
  }

  ngOnInit() {

    this.broadcastService.subscribe("msal:loginFailure", (payload) => {
      console.log("login failure");
      this.isLoggedIn = false;

    });

    this.broadcastService.subscribe("msal:loginSuccess", (payload) => {
      console.log("login success");
      this.isLoggedIn = true;
      let userData = this.msalService.getUser();
      this.userProfile = {
        name: userData.idToken['name'], firstName: "", lastName: "", oId: userData.idToken['oid'], eMail: userData.idToken['preferred_username'], isActive: "Yes",
        createdBy: userData.idToken['name'], createdTimeStamp: (new Date()).toUTCString(), modifiedBy: userData.idToken['name'], modifiedTimeStamp: (new Date()).toUTCString()
      }
      if (!this.isProfileSaved && payload.startsWith("idToken")) {
        this.isProfileSaved = true;
        this.personalizedPlanService.upsertUserProfile(this.userProfile)
          .subscribe(response => {
            if (response) {
              let profileData = { UserId: response.oId, UserName: response.name }
              this.userProfileName = response.name;
              sessionStorage.setItem("profileData", JSON.stringify(profileData));
            }
          });
      }
    });
  }

  ngOnDestroy() {
    this.broadcastService.getMSALSubject().next(1);
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

}
