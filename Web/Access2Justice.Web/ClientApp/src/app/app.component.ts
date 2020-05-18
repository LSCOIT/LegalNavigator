import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { MsalService } from "@azure/msal-angular";
import { NgxSpinnerService } from "ngx-spinner";
import { Global } from "./global";
import { PersonalizedPlanService } from "./guided-assistant/personalized-plan/personalized-plan.service";
import { LoginService } from "./common/login/login.service";
import { IUserProfile } from "./common/login/user-profile.model";
import { MapService } from "./common/map/map.service";
import { SaveButtonService } from "./common/resource/user-action/save-button/save-button.service";
import { StaticResourceService } from "./common/services/static-resource.service";
import { HeapIoService } from './heap/heap.io-service';

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit, OnDestroy {
  title = "app";
  staticContentResults: any;
  subscription: any;
  userProfile: IUserProfile;
  resoureStorage: any = [];
  showAlert: boolean = false;
  @HostListener("window:beforeunload", ["$event"])
  beforeUnloadHander() {
    if (
      (sessionStorage.getItem(this.global.sessionKey) ||
        sessionStorage.getItem(this.global.planSessionKey) ||
        sessionStorage.getItem(this.global.topicsSessionKey)) &&
      !this.global.isLoginRedirect &&
      !this.global.userId
    ) {
      this.showAlert = true;
      if (
        confirm(
          "You have unsaved changes! If you leave, your changes will be lost."
        )
      ) {
        return true;
      } else {
        return false;
      }
    }
  }

  constructor(
    private global: Global,    
    private staticResourceService: StaticResourceService,
    private msalService: MsalService,
    private mapService: MapService,
    private loginService: LoginService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private personalizedPlanService: PersonalizedPlanService,
    private saveButtonService: SaveButtonService,
    private heapIo: HeapIoService
  ) {}

  createOrGetProfile() {
    this.loginService.getUserProfile().subscribe(response => {
      if (response) {
        this.heapIo.load({ app_id: 1517532960, force_ssl: true, secure_cookie: true, disable_text_capture: true, cookie_path: '/' });
        this.heapIo.addUserProperties({'email': response.eMail, 'Name': response.name});
        this.global.setProfileData(
          response.oId,
          response.name,
          response.eMail,
          response.roleInformation
        );
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
    if (
      sessionStorage.getItem(this.global.sessionKey) ||
      sessionStorage.getItem(this.global.topicsSessionKey)
    ) {
      this.personalizedPlanService.saveResourcesToUserProfile();
    }
  }

  onActivate() {
    window.scroll(0, 0);
  }

  setStaticContentData() {
    this.spinner.show();
    let location = this.staticResourceService.loadStateName();
    this.staticResourceService.getStaticContents(location).subscribe(
      response => {
        this.staticContentResults = response;
        this.global.setData(this.staticContentResults);
        if (!sessionStorage.getItem(this.global.sessionKey)) {
          this.spinner.hide();
        }
      },
      () => {
        this.spinner.hide();
        this.router.navigate(["/error"]);
      }
    );
  }

  ngOnInit() {
    this.subscription = this.mapService.notifyLocation.subscribe(() => {
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
