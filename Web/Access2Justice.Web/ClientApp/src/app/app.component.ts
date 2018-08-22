import { Component, OnInit } from '@angular/core';
import { Global, UserStatus } from './global';
import { StaticContentDataService } from './shared/static-content-data.service';
import { StaticResourceService } from './shared/static-resource.service';
import { MapService } from './shared/map/map.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  staticContentResults: any;
  subscription: any;
  
  constructor(    
    private global: Global,
    private staticContentDataService: StaticContentDataService,
    private staticResourceService: StaticResourceService,
    private mapService: MapService) {
  }
  getCookie(cookieName: string) {    
    let cookieNameEQ = cookieName + "=";
    let cookies = document.cookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
      let cookie = cookies[i];
      while (cookie.charAt(0) == ' ')
        cookie = cookie.substring(1, cookie.length);
      if (cookie.indexOf(cookieNameEQ) == 0) {
        return cookie.substring(cookieNameEQ.length, cookie.length);
      }
    }
    return null;
  }

  deleteCookie(name: string, value: string, days: number) {
    let expires = "";
    if (days) {
      let date = new Date();
      date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
      expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
  }

  onActivate(event) {
    window.scroll(0, 0);
  }

  setStaticContentData() {
    this.staticResourceService.getStaticContents()
      .subscribe(response => {
        this.staticContentResults = response;
        this.staticContentDataService.setData(this.staticContentResults);
      });
  }

  ngOnInit() {
    let profileData = this.getCookie("profileData");
    if (profileData != undefined) {
      profileData = decodeURIComponent(profileData);
      sessionStorage.setItem("profileData", profileData);      
      this.deleteCookie("profileData", "", -1);
      this.global.role = UserStatus.Authorized;
    }
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.setStaticContentData();
      });
    this.setStaticContentData();
  }
}
