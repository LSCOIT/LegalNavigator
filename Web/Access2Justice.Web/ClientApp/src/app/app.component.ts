import { Component, OnInit, HostListener } from '@angular/core';
import { Global } from './global';
import { StaticResourceService } from './shared/services/static-resource.service';
import { MapService } from './shared/map/map.service';
import { MsalService } from '@azure/msal-angular';
import { LoginService } from './shared/login/login.service';
import { IUserProfile } from './shared/login/user-profile.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { PersonalizedPlanService } from './guided-assistant/personalized-plan/personalized-plan.service';
import { SaveButtonService } from './shared/resource/user-action/save-button/save-button.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  staticContentResults: any;
  subscription: any;
  userProfile: IUserProfile;
  resoureStorage: any = [];
  showAlert: boolean = false;

  @HostListener('window:beforeunload', ['$event'])
  beforeUnloadHander(event) {
    if ((sessionStorage.getItem(this.global.sessionKey)
      || sessionStorage.getItem(this.global.planSessionKey)
      || sessionStorage.getItem(this.global.topicsSessionKey))
      && (!this.global.isLoginRedirect) && !(this.global.userId)) {
      this.showAlert = true;
      if (confirm("You have unsaved changes! If you leave, your changes will be lost.")) {
        return true;
      } else {
        return false;
      }
    }
  };

  constructor(
    private global: Global,
    private staticResourceService: StaticResourceService,
    private msalService: MsalService,
    private mapService: MapService,
    private loginService: LoginService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private personalizedPlanService: PersonalizedPlanService,
    private saveButtonService: SaveButtonService
  ) { }

  createOrGetProfile() {
    this.loginService.getUserProfile()
      .subscribe(response => {
        if (response) {
          this.global.setProfileData(response.oId, response.name, response.eMail, response.roleInformation);
          this.saveBookmarkedResource();
          this.saveBookmarkedPlan();
        }
      });
  }

  saveBookmarkedPlan() {
    if (sessionStorage.getItem(this.global.planSessionKey)) {
      let plan = JSON.parse(sessionStorage.getItem(this.global.planSessionKey));
      this.saveButtonService.savePlanToUserProfile(plan);
    }
  }

  saveBookmarkedResource() {
    if (sessionStorage.getItem(this.global.sessionKey) || sessionStorage.getItem(this.global.topicsSessionKey)) {
      this.personalizedPlanService.saveResourcesToUserProfile();
    }
  }

  onActivate(event) {
    window.scroll(0, 0);
  }

  setStaticContentData() {
    this.spinner.show();
    let location = this.staticResourceService.loadStateName();
    this.staticResourceService.getStaticContents(location)
      .subscribe(
        response => {
          if (!(sessionStorage.getItem(this.global.sessionKey))) {
            this.spinner.hide();
          }
          this.staticContentResults = response;
          this.global.setData(this.staticContentResults);
        }, error => {
          this.spinner.hide();
          this.router.navigate(['/error']);
        });
  }

  ngOnInit() {
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.setStaticContentData();
      });
    this.setStaticContentData();
    if (this.msalService.getUser() && !this.global.userId) {
      this.createOrGetProfile();
    }
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
