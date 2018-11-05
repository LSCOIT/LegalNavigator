import { Component, OnInit } from '@angular/core';
import { Global, UserStatus } from './global';
import { StaticResourceService } from './shared/static-resource.service';
import { MapService } from './shared/map/map.service';
import { MsalService } from '@azure/msal-angular';
import { LoginService } from './shared/login/login.service';
import { IUserProfile } from './shared/login/user-profile.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { TopicService } from './topics-resources/shared/topic.service';

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
    private loginService: LoginService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private topicService: TopicService
  )
  { }

  createOrGetProfile() {    
    this.loginService.getUserProfile()
      .subscribe(response => {
        if (response) {
          this.global.setProfileData(response.oId, response.name, response.eMail, response.roleInformation);
        }
      });
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
}
