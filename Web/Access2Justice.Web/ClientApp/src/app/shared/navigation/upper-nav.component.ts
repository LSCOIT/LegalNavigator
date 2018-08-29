import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location, Logo, Home, GuidedAssistant, TopicAndResources, About, Search, PrivacyPromise, HelpAndFAQ, Login } from './navigation';
import { environment } from '../../../environments/environment';
import { MapService } from '../map/map.service';
import { Global } from '../../global';

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
  staticContentSubcription: any;

  constructor(private http: HttpClient,
    private staticResourceService: StaticResourceService,
    private mapService: MapService,
    private global: Global) { }

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
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.navigation = this.staticContent.find(x => x.name === this.name);
        this.filterUpperNavigationContent(this.navigation);
        this.staticResourceService.navigation = this.navigation;
      }
    }
  }

  ngOnInit() {
    this.getUpperNavigationContent();
    this.staticContentSubcription = this.global.notifyStaticData
      .subscribe((value) => {
        this.getUpperNavigationContent();
      });
  }
}
