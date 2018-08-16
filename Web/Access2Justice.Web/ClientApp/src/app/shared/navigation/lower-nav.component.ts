import { Component, OnInit, ElementRef, ViewChild, HostListener } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location, Logo, Home, GuidedAssistant, TopicAndResources, About, Search, PrivacyPromise, HelpAndFAQ, Login } from './navigation';
import { environment } from '../../../environments/environment';
import { MapService } from '../map/map.service';

@Component({
  selector: 'app-lower-nav',
  templateUrl: './lower-nav.component.html',
  styleUrls: ['./lower-nav.component.css']
})
export class LowerNavComponent implements OnInit {
  width = 0;
  showSearch = false;
  showMenu = false;
  my_Class = '';

  @ViewChild('sidenav') sidenav: ElementRef;
  @HostListener('window:resize')
  onResize() {
    this.width = window.innerWidth;
  }

  blobUrl: any = environment.blobUrl;
  navigation: Navigation;
  name: string = 'Navigation';
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

  constructor(
    private staticResourceService: StaticResourceService, private mapService: MapService
  ) { }

  openNav() {
    this.my_Class = "dimmer";
    if (this.width >= 768) {
      this.sidenav.nativeElement.style.width = "400px";
      this.sidenav.nativeElement.style.height = "100%";
    } else {
      this.sidenav.nativeElement.style.height = "100%";
      this.sidenav.nativeElement.style.width = "100%";
    }
  }

  closeNav() {
    this.my_Class = "";
    if (this.width >= 768) {
      this.sidenav.nativeElement.style.width = "0";
    } else {
      this.sidenav.nativeElement.style.height = "0";
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
    let homePageRequest = { name: this.name };
    if (this.staticResourceService.navigation && (this.staticResourceService.navigation.location[0].state == this.staticResourceService.loadStateName())) {
      this.navigation = this.staticResourceService.navigation;
      this.filterNavigationContent(this.staticResourceService.navigation);
    } else {
      this.staticResourceService.getStaticContent(homePageRequest)
        .subscribe(content => {
          this.navigation = content[0];
          this.filterNavigationContent(this.navigation);
          this.staticResourceService.navigation = this.navigation;
        });
    }
  }

  ngOnInit() {
    this.getNavigationContent();
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.getNavigationContent();
      });
  }
}
