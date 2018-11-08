import { Component, OnInit, AfterViewInit, ElementRef, HostListener } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location } from '../../shared/navigation/navigation';
import { environment } from '../../../environments/environment';
import { Global } from '../../global';
import { EventUtilityService } from '../event-utility.service';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css'],
})
export class LanguageComponent implements OnInit, AfterViewInit {
  blobUrl: any = environment.blobUrl;
  navigation: Navigation;
  name: string = 'Navigation';
  language: Language;
  location: Location;
  staticContent: any;
  staticContentSubcription: any;
  showLanguage: boolean = false;
  setBgColor: boolean = false;
  width: number;

  @HostListener('document:click', ['$event'])
  onClick(event) {
    let translator = document.getElementById('google_translate_element');
    this.showLanguage = !this.showLanguage;
    if ((event.srcElement.parentElement && event.srcElement.parentElement.id === 'language-dropdown') || (event.target && event.target.id === 'language-dropdown')) {
      if (this.showLanguage) {
        this.eventUtilityService.closeSideNav(event);
        translator.style.display = 'block';
        this.setBgColor = true;
        document.getElementsByTagName("select").item(0).focus();
      } else {
        translator.style.display = 'none';
        this.setBgColor = false;
      }
    } else {
      translator.style.display = 'none';
      this.setBgColor = false;
    }
  }

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global,
    private elementRef: ElementRef,
    private eventUtilityService: EventUtilityService
  ) { }


  addAttributes() {
    let languageOptions = document.querySelectorAll('select.goog-te-combo')[0];
    languageOptions["classList"].add('form-control');
    languageOptions["size"] = 15;
  }

  filterLanguagueNavigationContent(navigation): void {
    if (navigation) {
      this.name = navigation.name;
      this.language = navigation.language;
    }
  }

  getLanguagueNavigationContent(): void {
    if (this.staticResourceService.navigation && (this.staticResourceService.navigation.location[0].state == this.staticResourceService.getLocation())) {
        this.navigation = this.staticResourceService.navigation;
        this.filterLanguagueNavigationContent(this.staticResourceService.navigation);
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.navigation = this.staticContent.find(x => x.name === this.name);
        this.filterLanguagueNavigationContent(this.navigation);
        this.staticResourceService.navigation = this.navigation;
      }
    }
  }

  ngOnInit() {
    this.getLanguagueNavigationContent();
    this.staticContentSubcription = this.global.notifyStaticData
      .subscribe((value) => {
        this.getLanguagueNavigationContent();
      });
  }

  ngAfterViewInit() {
    setTimeout(() => this.addAttributes(), 1000);
  }
  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  } 
}
