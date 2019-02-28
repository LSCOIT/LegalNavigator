import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

import {ENV} from 'environment';
import { Global } from '../../global';
import { EventUtilityService } from '../services/event-utility.service';
import { StaticResourceService } from '../services/static-resource.service';
import { MapService } from '../map/map.service';
import {
  About,
  GuidedAssistant,
  HelpAndFAQ,
  Home,
  Language,
  Login,
  Logo,
  Navigation,
  PrivacyPromise,
  Search,
  TopicAndResources
} from './navigation';

@Component({
  selector: 'app-lower-nav',
  templateUrl: './lower-nav.component.html',
  styleUrls: ['./lower-nav.component.css']
})
export class LowerNavComponent implements OnInit {
  width = 0;
  showSearch = false;
  myClass = '';
  staticContentSubcription: any;
  blobUrl: any = ENV.blobUrl;
  navigation: Navigation;
  name = 'Navigation';
  language: Language;
  location: any = Location;
  privacyPromise: PrivacyPromise;
  helpAndFAQ: HelpAndFAQ;
  login: Login;
  logo: Logo;
  home: Home;
  guidedAssistant: GuidedAssistant;
  topicAndResources: TopicAndResources;
  about: About;
  search: Search;
  subscription: any;
  staticContent: any;
  @ViewChild('sidenav') sidenav: ElementRef;

  constructor(
    private staticResourceService: StaticResourceService,
    private mapService: MapService,
    private global: Global,
    private eventUtilityService: EventUtilityService
  ) {
    eventUtilityService.upperNavClicked$.subscribe(response => {
      this.closeNav();
    });
  }

  openNav() {
    let windowWidth = window.innerWidth;
    this.myClass = 'dimmer';
    if (windowWidth >= 768) {
      this.sidenav.nativeElement.style.width = '400px';
      this.sidenav.nativeElement.style.height = '100%';
    } else {
      this.sidenav.nativeElement.style.height = '100%';
      this.sidenav.nativeElement.style.width = '100%';
    }
  }

  closeNav() {
    let windowWidth = window.innerWidth;
    this.myClass = '';
    if (windowWidth >= 768) {
      this.sidenav.nativeElement.style.width = '0';
    } else {
      this.sidenav.nativeElement.style.height = '0';
    }
  }

  toggleSearch() {
    this.showSearch = !this.showSearch;
  }

  filterNavigationContent(navigation): void {
    if (navigation) {
      this.name = navigation.name;
      this.language = navigation.language;
      this.location = navigation.location;
      this.privacyPromise = navigation.privacyPromise;
      this.helpAndFAQ = navigation.helpAndFAQ;
      this.login = navigation.login;
      this.logo = navigation.logo;
      this.home = navigation.home;
      this.guidedAssistant = navigation.guidedAssistant;
      this.topicAndResources = navigation.topicAndResources;
      this.about = navigation.about;
      this.search = navigation.search;
    }
  }

  getNavigationContent(): void {
    if (
      this.staticResourceService.navigation &&
      this.staticResourceService.navigation.location[0].state ==
      this.staticResourceService.getLocation()
    ) {
      this.navigation = this.staticResourceService.navigation;
      this.filterNavigationContent(this.staticResourceService.navigation);
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.navigation = this.staticContent.find(x => x.name === this.name);
        this.filterNavigationContent(this.navigation);
        this.staticResourceService.navigation = this.navigation;
      }
    }
  }

  ngOnInit() {
    this.getNavigationContent();
    this.subscription = this.mapService.notifyLocation.subscribe(value => {
      this.getNavigationContent();
    });
    this.staticContentSubcription = this.global.notifyStaticData.subscribe(
      value => {
        this.getNavigationContent();
      }
    );
  }

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
