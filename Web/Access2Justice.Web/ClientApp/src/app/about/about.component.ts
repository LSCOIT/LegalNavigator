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

  filterAboutContent(): void {
    if (this.aboutContent) {
      this.aboutContentData = this.aboutContent;
    }
  }

  getAboutPageContent(): void {
    let aboutPageRequest = { name: this.name };
    this.staticResourceService.getStaticContent(aboutPageRequest)
      .subscribe(content => {
        this.aboutContent = content[0];
        this.filterAboutContent();
      });
  }

  ngOnInit() {
    this.getAboutPageContent();
  }
}
