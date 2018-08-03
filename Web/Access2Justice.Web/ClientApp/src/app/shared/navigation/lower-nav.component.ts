import { Component, OnInit, ElementRef, ViewChild, HostListener } from '@angular/core';
import { StaticResourceService } from './static-resource.service';
import { Navigation, Language, Location, ButtonImage, Logo, Button } from './navigation';
import { Privacy } from '../../home/home';
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

  blob: any = environment.blobUrl;
  navigationContent: any;
  navigation: Navigation = {
    id: '',
    language: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, navigationImage: { source: '', altText: '' }, dropDownImage: { source: '', altText: '' } },
    location: { text: '', altText: '', button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    privacyPromise: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    helpAndFAQ: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    login: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } },
    logo: { firstLogo: '', secondLogo: '', link: '' },
    home: { buttonText: '', buttonAltText: '', buttonLink: '' },
    guidedAssistant: { buttonText: '', buttonAltText: '', buttonLink: '' },
    topicAndResources: { buttonText: '', buttonAltText: '', buttonLink: '' },
    about: { buttonText: '', buttonAltText: '', buttonLink: '' },
    search: { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } }
  }
  id: string = 'Navigation';
  language: Language = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, navigationImage: { source: '', altText: '' }, dropDownImage: { source: '', altText: '' } };
  location: Location = { text: '', altText: '', button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };
  privacyPromise: ButtonImage = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };
  helpAndFAQ: ButtonImage = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };
  login: ButtonImage = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };
  logo: Logo = { firstLogo: '', secondLogo: '', link: '' };
  home: Button = { buttonText: '', buttonAltText: '', buttonLink: '' };
  guidedAssistant: Button = { buttonText: '', buttonAltText: '', buttonLink: '' };
  topicAndResources: Button = { buttonText: '', buttonAltText: '', buttonLink: '' };
  about: Button = { buttonText: '', buttonAltText: '', buttonLink: '' };
  search: ButtonImage = { button: { buttonText: '', buttonAltText: '', buttonLink: '' }, image: { source: '', altText: '' } };


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
      this.search.image.source = this.navigation.search.image.source;
    }
  }

  getNavigationContent(): void {
    this.staticResourceService.getStaticContents(this.id)
      .subscribe(content => {
        this.navigation = content[0];
        console.log(this.navigation.id);
        this.filterNavigationContent();
      });
  }

  ngOnInit() {
    this.getNavigationContent();
  }
}
