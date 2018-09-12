import { Component, OnInit, AfterViewInit, ElementRef, HostListener } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location } from '../../shared/navigation/navigation';
import { environment } from '../../../environments/environment';
import { Global } from '../../global';

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
    if (event.target.offsetParent && event.target.offsetParent.id === 'language') {
      if (this.showLanguage) {
        translator.style.display = 'block';
        this.setBgColor = true;
      } else {
        translator.style.display = 'none';
        this.setBgColor = false;
      }
    } else {
      translator.style.display = 'none';
      this.setBgColor = false;
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.width = event.target.innerWidth;
  }

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global,
    private elementRef: ElementRef
  ) { }


  addAttributes() {
    let languageOptions = document.querySelectorAll('select.goog-te-combo')[0];
    languageOptions["classList"].add('form-control');
    languageOptions["size"] = 15;

    //let translator = document.getElementById('google_translate_element');
    //translator.setAttribute("style", "display: none; position: absolute; top: 0; background: #fff; width: 225px; height: 330px; padding: 10px 20px 10px 35px; box-shadow: 0 6px 12px 3px #ddd");

    //if (this.width >= 768) {
    //  translator.style.left = '68px';
    //} else {
    //  translator.style.left = '0';
    //}
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
}
