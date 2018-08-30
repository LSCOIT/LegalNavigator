import { Component, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { StaticResourceService } from '../static-resource.service';
import { Navigation, PrivacyPromise, HelpAndFAQ, Home, GuidedAssistant, TopicAndResources, About} from '../navigation/navigation';
import { Global } from '../../global';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {

  constructor(private staticResourceService: StaticResourceService,
    private global: Global) { }
  blobUrl: any = environment.blobUrl;
  navigation: Navigation;
  name: string = 'Navigation';
  privacyPromise: PrivacyPromise;
  helpAndFAQ: HelpAndFAQ;
  home: Home;
  about: About;
  guidedAssistant: GuidedAssistant;
  topicAndResources: TopicAndResources;
  staticContent: any;
  staticContentSubcription: any;

  filterNavigationContent(navigation): void {
    if (navigation) {
      this.privacyPromise = navigation.privacyPromise;
      this.helpAndFAQ = navigation.helpAndFAQ;
      this.home = navigation.home;
      this.about = navigation.about;
      this.guidedAssistant = navigation.guidedAssistant;
      this.topicAndResources = navigation.topicAndResources;
    }
  }

  getNavigationContent(): void {
    let pageContentRequest = { name: this.name };
    if (this.staticResourceService.navigation && (this.staticResourceService.navigation.location[0].state == this.staticResourceService.getLocation())) {
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
    this.staticContentSubcription = this.global.notifyStaticData
      .subscribe((value) => {
        this.getNavigationContent();
      });
  }
}
