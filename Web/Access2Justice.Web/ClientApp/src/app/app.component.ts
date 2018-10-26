import { Component, OnInit, HostListener } from '@angular/core';
import { Global, UserStatus } from './global';
import { StaticResourceService } from './shared/static-resource.service';
import { MapService } from './shared/map/map.service';
import { MsalService } from '@azure/msal-angular';
import { LoginService } from './shared/login/login.service';
import { IUserProfile } from './shared/login/user-profile.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { TopicService } from './topics-resources/shared/topic.service';
import { PersonalizedPlanService } from './guided-assistant/personalized-plan/personalized-plan.service';

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
  sessionKey: string = "bookmarkedResource";
  planSessionKey: string = "bookmarkPlanId";
  resoureStorage: any = [];
  showAlert: boolean = false;

  constructor(
    private global: Global,
    private staticResourceService: StaticResourceService,
    private msalService: MsalService,
    private mapService: MapService,
    private loginService: LoginService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private topicService: TopicService,
    private personalizedPlanService: PersonalizedPlanService
  ) { }

  createOrGetProfile() {
    let userData = this.msalService.getUser();
    this.userProfile = {
      name: userData.idToken['name'], firstName: "", lastName: "", oId: userData.idToken['oid'], eMail: userData.idToken['preferred_username'], isActive: "Yes",
      createdBy: userData.idToken['name'], createdTimeStamp: (new Date()).toUTCString(), modifiedBy: userData.idToken['name'], modifiedTimeStamp: (new Date()).toUTCString()
    }
    this.loginService.upsertUserProfile(this.userProfile)
      .subscribe(response => {
        if (response) {
          this.global.setProfileData(response.oId, response.name, response.eMail);
          this.saveBookmarkedPlan();
          this.saveBookmarkedResource();
        }
      });
  }

  saveBookmarkedPlan() {

  }

  saveBookmarkedResource() {
    this.personalizedPlanService.saveResourcesToUserProfile();
  }

  onActivate(event) {
    window.scroll(0, 0);
  }

  setStaticContentData() {
    this.spinner.show();
    this.staticResourceService.getStaticContents()
      .subscribe(
      response => {
        this.spinner.hide();
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

  @HostListener('window:unload', ['$event'])
  unloadHandler(event) {
    alert('Are you really want to perform the action?');
  }

  @HostListener('window:beforeunload', ['$event'])
  beforeUnloadHander(event) {
    this.showAlert = true;
    if (sessionStorage.getItem(this.sessionKey) || sessionStorage.getItem(this.planSessionKey)) {
      if (confirm("You have unsaved changes! If you leave, your changes will be lost.")) {
        return true;
      } else {
        return false;
      }
    }
  }
}
