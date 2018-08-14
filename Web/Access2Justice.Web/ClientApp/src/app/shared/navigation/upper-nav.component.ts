import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { api } from '../../../api/api';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location, Logo, Home, GuidedAssistant, TopicAndResources, About, Search, PrivacyPromise, HelpAndFAQ, Login } from './navigation';
import { environment } from '../../../environments/environment';
import { MapService } from '../map/map.service';

@Component({
  selector: 'app-upper-nav',
  templateUrl: './upper-nav.component.html',
  styleUrls: ['./upper-nav.component.css']
})

export class UpperNavComponent implements OnInit {

  userProfile: string;
  isLoggedIn: boolean = false;
  blobUrl: any = environment.blobUrl;
  navigation: Navigation;
  name: string = 'Navigation';
  language: Language;
  location: Location;
  privacyPromise: PrivacyPromise;
  helpAndFAQ: HelpAndFAQ;
  login: Login;
  subscription: any;
  constructor(private http: HttpClient, private staticResourceService: StaticResourceService, private mapService: MapService) { }

  externalLogin() {
    var form = document.createElement('form');
    form.setAttribute('method', 'POST');
    form.setAttribute('action', api.loginUrl);
    document.body.appendChild(form);
    form.submit();
  }

  logout() {
    sessionStorage.removeItem("profileData");
    let form = document.createElement('form');
    form.setAttribute('method', 'POST');
    form.setAttribute('action', api.logoutUrl);
    document.body.appendChild(form);
    form.submit();
  }

  filterUpperNavigationContent(navigation): void {
    if (navigation) {
      this.name = navigation.name;
      this.privacyPromise = navigation.privacyPromise;
      this.helpAndFAQ = navigation.helpAndFAQ;
      this.login = navigation.login;
    }
  }

  getUpperNavigationContent(): void {
    let homePageRequest = { name: this.name };
    if (!this.staticResourceService.navigation) {
      this.staticResourceService.getStaticContent(homePageRequest)
        .subscribe(content => {
          this.navigation = content[0];
          this.filterUpperNavigationContent(this.navigation);
          this.staticResourceService.navigation = this.navigation;
        });
    }
    else {
      this.navigation = this.staticResourceService.navigation;
      this.filterUpperNavigationContent(this.navigation);
    }
}


  ngOnInit() {
    this.getUpperNavigationContent();
    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      this.isLoggedIn = true;
      this.userProfile = profileData["UserName"];
    }
    
  }
}
