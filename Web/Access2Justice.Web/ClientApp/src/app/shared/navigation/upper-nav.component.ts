import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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
  blobUrl: any = environment.blobUrl;
  navigation: Navigation;
  id: string = 'Navigation';
  language: Language;
  location: Location;
  privacyPromise: PrivacyPromise;
  helpAndFAQ: HelpAndFAQ;
  login: Login;
  subscription: any;  
  constructor(private http: HttpClient, private staticResourceService: StaticResourceService, private mapService: MapService) { }

  filterUpperNavigationContent(): void {
    if (this.navigation) {
      this.id = this.navigation.name;
      this.privacyPromise = this.navigation.privacyPromise;
      this.helpAndFAQ = this.navigation.helpAndFAQ;
      this.login = this.navigation.login;     
    }
  }

  getUpperNavigationContent(): void {
    let homePageRequest = { name: this.id, location: this.location };
    this.staticResourceService.getStaticContent(homePageRequest)
      .subscribe(content => {
        this.navigation = content[0];
        this.filterUpperNavigationContent();        
      });
  }

  ngOnInit() {
    this.getUpperNavigationContent();
  }
}
