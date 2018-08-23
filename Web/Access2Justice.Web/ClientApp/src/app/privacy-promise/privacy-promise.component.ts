import { Component, OnInit } from '@angular/core';
import { PrivacyContent, Image, Details } from '../privacy-promise/privacy-promise';
import { StaticResourceService } from '../shared/static-resource.service';
import { Global } from '../global';

@Component({
  selector: 'app-privacy-promise',
  templateUrl: './privacy-promise.component.html',
  styleUrls: ['./privacy-promise.component.css']
})
export class PrivacyPromiseComponent implements OnInit {
  privacyContent: PrivacyContent;
  informationData: Array<Details> = [];
  imageData: Image;
  name: string = 'PrivacyPromisePage';
  staticContent: any;
  staticContentSubcription: any;

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global
  ) { }

  filterPrivacyContent(privacyContent): void {
    if (privacyContent) {
      this.informationData = privacyContent.details;
      this.imageData = privacyContent.image;
    }
  }

  getPrivacyPageContent(): void {
    let privacyPageRequest = { name: this.name };
    if ((this.staticResourceService.privacyContent) && (this.staticResourceService.privacyContent.location[0].state == this.staticResourceService.getLocation())) {
      this.privacyContent = this.staticResourceService.privacyContent;
      this.filterPrivacyContent(this.staticResourceService.privacyContent);
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.staticContent.forEach(content => {
          if (content.name === this.name) {
            this.privacyContent = content;
            this.filterPrivacyContent(this.privacyContent);
            this.staticResourceService.privacyContent = this.privacyContent;
          }
        });
      }
    }
  }

  ngOnInit() {
    this.getPrivacyPageContent();
    this.staticContentSubcription = this.global.notifyStaticData
      .subscribe((value) => {
        this.getPrivacyPageContent();
      });
  }
}
