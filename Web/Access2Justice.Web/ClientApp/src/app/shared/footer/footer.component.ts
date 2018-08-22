import { Component, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { StaticResourceService } from '../static-resource.service';
import { Navigation, PrivacyPromise, HelpAndFAQ, Home, GuidedAssistant, TopicAndResources, About} from '../navigation/navigation';
import { StaticContentDataService } from '../static-content-data.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {

  constructor(private staticResourceService: StaticResourceService,
    private staticContentDataService: StaticContentDataService) { }
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
      //this.staticResourceService.getStaticContent(pageContentRequest)
      //  .subscribe(content => {
      //    this.navigation = content[0];
      //    this.filterNavigationContent(this.navigation);
      //    this.staticResourceService.navigation = this.navigation;
      //  });
      if (this.staticContentDataService.getData()) {
        this.staticContent = this.staticContentDataService.getData();
        this.staticContent.forEach(content => {
          if (content.name === this.name) {
            this.navigation = content;
            this.filterNavigationContent(this.navigation);
            this.staticResourceService.navigation = this.navigation;
          }
        });
      }
    }
  }

  ngOnInit() {
    this.getNavigationContent();
  }
}
