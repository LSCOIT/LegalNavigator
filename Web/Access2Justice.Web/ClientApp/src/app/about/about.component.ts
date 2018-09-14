import { About  } from '../about/about';
import { Component, OnInit } from '@angular/core';
import { Global } from '../global';
import { StaticResourceService } from '../shared/static-resource.service';
import { environment } from '../../environments/environment';

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
  staticContent: any;
  staticContentSubcription: any;
  blobUrl: string = environment.blobUrl;

  getAboutPageContent(): void {
    if (this.staticResourceService.aboutContent && (this.staticResourceService.aboutContent.location[0].state == this.staticResourceService.getLocation())) {
      this.aboutContent = this.staticResourceService.aboutContent;
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.aboutContent = this.staticContent.find(x => x.name === this.name);
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
