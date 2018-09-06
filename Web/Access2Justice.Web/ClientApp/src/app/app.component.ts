import { Component, OnInit } from '@angular/core';
import { Global, UserStatus } from './global';
import { StaticResourceService } from './shared/static-resource.service';
import { MapService } from './shared/map/map.service';
import { MsalService } from '@azure/msal-angular';
import { PersonalizedPlanService } from './guided-assistant/personalized-plan/personalized-plan.service';
import { IUserProfile } from './shared/login/user-profile.model';

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
  
  constructor(    
    private global: Global,
    private staticResourceService: StaticResourceService,
    private msalService: MsalService,
    private mapService: MapService,
    private personalizedPlanService: PersonalizedPlanService) { }  

  createOrGetProfile() {
    console.log("I'm into createOrGetProfile method..");
    let userData = this.msalService.getUser();
    this.userProfile = {
      name: userData.idToken['name'], firstName: "", lastName: "", oId: userData.idToken['oid'], eMail: userData.idToken['preferred_username'], isActive: "Yes",
      createdBy: userData.idToken['name'], createdTimeStamp: (new Date()).toUTCString(), modifiedBy: userData.idToken['name'], modifiedTimeStamp: (new Date()).toUTCString()
    }
    this.personalizedPlanService.upsertUserProfile(this.userProfile)
      .subscribe(response => {
        if (response) {
          let profileData = { UserId: response.oId, UserName: response.name }          
          sessionStorage.setItem("profileData", JSON.stringify(profileData));
        }
      });
  }

  onActivate(event) {
    window.scroll(0, 0);
  }

  setStaticContentData() {
    this.staticResourceService.getStaticContents()
      .subscribe(response => {
        this.staticContentResults = response;
        this.global.setData(this.staticContentResults);
      });
  }

  ngOnInit() {
    if (this.msalService.getUser()) {      
      this.createOrGetProfile();
    }
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.setStaticContentData();
      });
    this.setStaticContentData();
  }
}
