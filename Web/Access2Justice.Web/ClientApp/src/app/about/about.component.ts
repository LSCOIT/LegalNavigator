import { About  } from '../about/about';
import { Component, OnInit } from '@angular/core';
import { Global } from '../global';
import { StaticResourceService } from '../shared/static-resource.service';
import { environment } from '../../environments/environment';
import { HelpText } from '../home/home';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  name: string = 'AboutPage';
  aboutContent: About;
  staticContent: any;
  staticContentSubcription: any;
  blobUrl: string = environment.blobUrl;
  helpText: HelpText;

  constructor(
    private staticResourceService: StaticResourceService,
    private global:Global
  ) { }  

  getAboutPageContent(): void {
    if (this.staticResourceService.aboutContent && (this.staticResourceService.aboutContent.location[0].state == this.staticResourceService.getLocation())) {
      this.aboutContent = this.staticResourceService.aboutContent;
      this.helpText = this.staticResourceService.homeContent.helpText;
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.aboutContent = this.staticContent.find(x => x.name === this.name);
        this.helpText = this.staticContent.find(x => x.name === 'HomePage').helpText;
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

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
