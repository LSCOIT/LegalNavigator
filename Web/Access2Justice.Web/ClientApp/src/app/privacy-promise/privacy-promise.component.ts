import { Component, OnInit } from '@angular/core';
import { PrivacyContent, Image, Details } from '../privacy-promise/privacy-promise';
import { StaticResourceService } from '../shared/static-resource.service';

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

  constructor(
    private staticResourceService: StaticResourceService
  ) { }

  filterPrivacyContent(): void {
    if (this.privacyContent) {
      this.informationData = this.privacyContent.details;
      this.imageData = this.privacyContent.image;
    }
  }

  getPrivacyPageContent(): void {
    let privacyPageRequest = { name: this.name };
    this.staticResourceService.getStaticContent(privacyPageRequest)
      .subscribe(content => {
        this.privacyContent = content[0];
        this.filterPrivacyContent();
      });
  }
  ngOnInit() {
    this.getPrivacyPageContent();
  }
}
