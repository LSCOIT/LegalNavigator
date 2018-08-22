import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location, Logo, Home, GuidedAssistant, TopicAndResources, About, Search, PrivacyPromise, HelpAndFAQ, Login } from './navigation';
import { environment } from '../../../environments/environment';
import { MapService } from '../map/map.service';
import { StaticContentDataService } from '../static-content-data.service';

@Component({
  selector: 'app-upper-nav',
  templateUrl: './upper-nav.component.html',
  styleUrls: ['./upper-nav.component.css']
})

export class UpperNavComponent implements OnInit {
  blobUrl: any = environment.blobUrl;
  navigation: Navigation;
  name: string = 'Navigation';
  language: Language;
  location: Location;
  privacyPromise: PrivacyPromise;
  helpAndFAQ: HelpAndFAQ;
  login: Login;
  subscription: any;
  staticContent: any;

  constructor(private http: HttpClient,
    private staticResourceService: StaticResourceService,
    private mapService: MapService,
    private staticContentDataService: StaticContentDataService) { }

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
    if (this.staticResourceService.navigation && (this.staticResourceService.navigation.location[0].state == this.staticResourceService.getLocation())) {
      this.navigation = this.staticResourceService.navigation;
      this.filterUpperNavigationContent(this.navigation);
    } else {
      //this.staticResourceService.getStaticContent(homePageRequest)
      //  .subscribe(content => {
      //    this.navigation = content[0];
      //    this.filterUpperNavigationContent(this.navigation);
      //    this.staticResourceService.navigation = this.navigation;
      //  });
      if (this.staticContentDataService.getData()) {
        this.staticContent = this.staticContentDataService.getData();
        this.staticContent.forEach(content => {
          if (content.name === this.name) {
            this.navigation = content;
            this.filterUpperNavigationContent(this.navigation);
            this.staticResourceService.navigation = this.navigation;
          }
        });
      }
    }
  }

  ngOnInit() {
    this.getUpperNavigationContent();
  }
}
