import { Component, OnInit } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location, Logo, Home, GuidedAssistant, TopicAndResources, About, Search, PrivacyPromise, HelpAndFAQ, Login } from '../../shared/navigation/navigation';
import { environment } from '../../../environments/environment';
import { Global } from '../../global';

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css']
})
export class LanguageComponent implements OnInit {
  items: string[] = [
    'Afrikaans', 'Arabic', 'Bangla', 'Bosnian (Latin)', 'Bulgarian', 'Cantonese (Traditional)', 'Catalan', 'Chinese SimplifiedCS', 'Chinese TraditionalCS', 'Croatian', 'Czech', 'Danish', 'Dutch', 'English', 'Estonian', 'Fijian', 'Filipino', 'Finnish', 'French', 'German', 'German', 'Greek', 'Haitian Creole', 'Hebrew', 'Hindi', 'Hmong Daw', 'Hungarian', 'Indonesian', 'Italian', 'Japanese', 'Kiswahili', 'Klingon', 'Klingon (plqaD)', 'Korean', 'Latvian', 'Lithuanian', 'Malagasy', 'Malay', 'Maltese', 'Norwegian', 'Persian', 'Polish', 'Portuguese', 'Queretaro Otomi', 'Romanian', 'Russian', 'Samoan', 'Serbian (Cyrillic)', 'Serbian (Latin)', 'Slovak', 'Slovenian', 'Spanish', 'Swedish', 'Tahitian', 'Tamil', 'Thai', 'Tongan', 'Turkish', 'Ukrainian', 'Urdu', 'Vietnamese', 'Welsh', 'Yucatec Maya'];
  blobUrl: any = environment.blobUrl;
  navigation: Navigation;
  name: string = 'Navigation';
  language: Language;
  location: Location;
  staticContent: any;
  staticContentSubcription: any;
  
  constructor(private staticResourceService: StaticResourceService,
    private global: Global) { }

  filterLanguagueNavigationContent(navigation): void {
    if (navigation) {
      this.name = navigation.name;
      this.language = navigation.language;
    }
  }

  getLanguagueNavigationContent(): void {
    let homePageRequest = { name: this.name };
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
}
