import { Component, OnInit } from '@angular/core';
import { StaticResourceService } from '../shared/static-resource.service';
import { About, Mission, Service, PrivacyPromise } from '../about/about';
import { Image } from '../home/home';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {

  constructor(private staticResourceService: StaticResourceService) { }
  name: string = 'AboutPage';
  aboutContent: About;
  aboutContentData: About;

  filterAboutContent(aboutContent): void {
    if (aboutContent) {
      this.aboutContentData = aboutContent;
    }
  }

  getAboutPageContent(): void {
    let aboutPageRequest = { name: this.name };
    if (!this.staticResourceService.aboutContent) {
      this.staticResourceService.getStaticContent(aboutPageRequest)
        .subscribe(content => {
          this.aboutContent = content[0];
          this.filterAboutContent(this.aboutContent);
          this.staticResourceService.aboutContent = this.aboutContent;
        });
    }
    else {
      this.filterAboutContent(this.staticResourceService.aboutContent);
    }
  }

  ngOnInit() {
    this.getAboutPageContent();
  }
}
