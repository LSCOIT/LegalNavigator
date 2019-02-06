import { Component, OnInit } from "@angular/core";

import ENV from 'env';
import { Global } from "../global";
import { Details, Image, PrivacyContent } from './privacy-promise';
import { StaticResourceService } from "../shared/services/static-resource.service";

@Component({
  selector: "app-privacy-promise",
  templateUrl: "./privacy-promise.component.html",
  styleUrls: ["./privacy-promise.component.css"]
})
export class PrivacyPromiseComponent implements OnInit {
  privacyContent: PrivacyContent;
  informationData: Array<Details> = [];
  imageData: Image;
  name: string = "PrivacyPromisePage";
  staticContent: any;
  staticContentSubcription: any;
  blobUrl: string = ENV().blobUrl;

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global
  ) {}

  getPrivacyPageContent(): void {
    if (
      this.staticResourceService.privacyContent &&
      this.staticResourceService.privacyContent.location[0].state ==
        this.staticResourceService.getLocation()
    ) {
      this.privacyContent = this.staticResourceService.privacyContent;
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.privacyContent = this.staticContent.find(
          x => x.name === this.name
        );
        this.staticResourceService.privacyContent = this.privacyContent;
      }
    }
  }

  ngOnInit() {
    this.getPrivacyPageContent();
    this.staticContentSubcription = this.global.notifyStaticData.subscribe(
      value => {
        this.getPrivacyPageContent();
      }
    );
  }

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
