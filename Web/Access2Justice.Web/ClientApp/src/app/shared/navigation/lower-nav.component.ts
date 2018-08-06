import { Component, OnInit, ElementRef, ViewChild, HostListener } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location, Logo, Home, GuidedAssistant, TopicAndResources, About, Search, PrivacyPromise, HelpAndFAQ, Login } from './navigation';
import { environment } from '../../../environments/environment';

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
  id: string = 'Navigation';
  language: Language;
  location: Location;
  privacyPromise: PrivacyPromise;
  helpAndFAQ: HelpAndFAQ;
  login: Login;
  logo: Logo;
  home: Home;
  guidedAssistant: GuidedAssistant;
  topicAndResources: TopicAndResources;
  about: About;
  search: Search;


  constructor(
    private staticResourceService: StaticResourceService
  ) { }

  openNav() {
    this.my_Class = "dimmer";
    if (this.width >= 768) {
      this.sidenav.nativeElement.style.width = "33.333%";
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

  filterNavigationContent(): void {
    if (this.navigation) {
      this.id = this.navigation.id;
      this.language = this.navigation.language;
      this.location = this.navigation.location;
      this.privacyPromise = this.navigation.privacyPromise;
      this.helpAndFAQ = this.navigation.helpAndFAQ;
      this.login = this.navigation.login;
      this.logo = this.navigation.logo;
      this.home = this.navigation.home;
      this.guidedAssistant = this.navigation.guidedAssistant;
      this.topicAndResources = this.navigation.topicAndResources;
      this.about = this.navigation.about;
      this.search = this.navigation.search;
    }
  }

  getNavigationContent(): void {
    this.staticResourceService.getStaticContents(this.id)
      .subscribe(content => {
        this.navigation = content[0];
        this.filterNavigationContent();
      });
  }

  ngOnInit() {
    this.getNavigationContent();
  }
}
