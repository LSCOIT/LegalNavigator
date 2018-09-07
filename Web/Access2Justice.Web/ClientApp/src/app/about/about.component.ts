import { About  } from '../about/about';
import { Component, OnInit } from '@angular/core';
import { Global } from '../global';
import { StaticResourceService } from '../shared/static-resource.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {

  constructor(
    private staticResourceService: StaticResourceService,
    private global:Global
  ) { }
  
  name: string = 'AboutPage';
  aboutContent: About;
  aboutContentData: About;
  staticContent: any;
  staticContentSubcription: any;

  filterAboutContent(aboutContent): void {
    if (aboutContent) {
      this.aboutContentData = aboutContent;
    }
  }

  getAboutPageContent(): void {
    if (this.staticResourceService.aboutContent && (this.staticResourceService.aboutContent.location[0].state == this.staticResourceService.getLocation())) {
      this.aboutContent = this.staticResourceService.aboutContent;
      this.filterAboutContent(this.staticResourceService.aboutContent);
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.aboutContent = this.staticContent.find(x => x.name === this.name);
        this.filterAboutContent(this.aboutContent);
        this.staticResourceService.aboutContent = this.aboutContent;
      }
    }
  }

  ngOnInit() {
    this.getAboutPageContent();
    this.staticContentSubcription = this.global.notifyStaticData
      .subscribe(() => {
          this.getAboutPageContent();
        });
  }
}
