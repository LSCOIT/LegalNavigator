import { Component, OnInit } from '@angular/core';
import { StaticResourceService } from '../../shared/static-resource.service';
import { Navigation, Language, Location, Logo, Home, GuidedAssistant, TopicAndResources, About, Search, PrivacyPromise, HelpAndFAQ, Login } from '../../shared/navigation/navigation';
import { environment } from '../../../environments/environment';

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
  
  constructor(private staticResourceService: StaticResourceService) { }

  filterLanguagueNavigationContent(): void {
    if (this.navigation) {
      this.name = this.navigation.name;
      this.language = this.navigation.language;
    }
  }

  getLanguagueNavigationContent(): void {    
    let homePageRequest = { name: this.name };
    this.staticResourceService.getStaticContent(homePageRequest)
      .subscribe(content => {
        this.navigation = content[0];
        this.filterLanguagueNavigationContent();
      });
  }

  ngOnInit() {
    this.getLanguagueNavigationContent();
  }
}
