import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { api } from '../../../api/api';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location, Logo, Home, GuidedAssistant, TopicAndResources, About, Search, PrivacyPromise, HelpAndFAQ, Login } from './navigation';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-upper-nav',
  templateUrl: './upper-nav.component.html',
  styleUrls: ['./upper-nav.component.css']
})

export class UpperNavComponent implements OnInit {

  userProfile: string;
  isLoggedIn: boolean = false;
  contentUrl: any = environment.blobUrl;
  navigation: Navigation = {
    id: '',
    language: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, navigationImage: { source: '', altText: '' }, dropDownImage: { source: '', altText: '' } },
    location: { text: '', altText: '', button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    privacyPromise: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    helpAndFAQ: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    login: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    logo: { firstLogo: '', secondLogo: '', link: '' },
    home: { button: { buttonText: '', buttonAltText: '', buttonLink: '' } },
    guidedAssistant: { button: { buttonText: '', buttonAltText: '', buttonLink: '' } },
    topicAndResources: { button: { buttonText: '', buttonAltText: '', buttonLink: '' } },
    about: { button: { buttonText: '', buttonAltText: '', buttonLink: '' } },
    search: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } }
  }
  id: string = 'Navigation';
  language: Language = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, navigationImage: { source: '', altText: '' }, dropDownImage: { source: '', altText: '' } };
  location: Location = { text: '', altText: '', button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };
  privacyPromise: PrivacyPromise = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };
  helpAndFAQ: HelpAndFAQ = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };
  login: Login = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };

  constructor(private http: HttpClient, private staticResourceService: StaticResourceService) { }

  externalLogin() {
    var form = document.createElement('form');
    form.setAttribute('method', 'GET');
    form.setAttribute('action', api.loginUrl);
    document.body.appendChild(form);
    form.submit();
  }

  logout() {
    sessionStorage.removeItem("profileData");
    let form = document.createElement('form');
    form.setAttribute('method', 'GET');
    form.setAttribute('action', api.logoutUrl);
    document.body.appendChild(form);
    form.submit();
  }

  filterUpperNavigationContent(): void {
    if (this.navigation) {
      this.id = this.navigation.id;
      this.privacyPromise = this.navigation.privacyPromise;
      this.helpAndFAQ = this.navigation.helpAndFAQ;
      this.login = this.navigation.login;
    }
  }

  getUpperNavigationContent(): void {
    this.staticResourceService.getStaticContents(this.id)
      .subscribe(content => {
        this.navigation = content[0];
        this.filterUpperNavigationContent();
      });
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
