import { Component, OnInit } from '@angular/core';
import { PrivacyContent, Details} from "../../privacy-promise/privacy-promise";
import { Global } from "../../global";
import { NgForm } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { StaticResourceService } from '../../shared/static-resource.service';

@Component({
  selector: 'app-privacy-promise-admin',
  templateUrl: './privacy-promise-admin.component.html',
  styleUrls: ['../admin-styles.css']
})

export class PrivacyPromiseAdminComponent implements OnInit {
  detailParams: any;
  newPrivacyContent: any;
  privacyContent: PrivacyContent;
  informationData: Array<Details> = [];
  name: string = 'PrivacyPromisePage';
  staticContent: any;
  staticContentSubcription: any;
  blobUrl: string = environment.blobUrl;

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global
  ) { }

  mapSectionDescription(form) {
    let numOfTitles = Object.keys(form).filter((key) => key.indexOf("title") > -1);

    this.detailParams = numOfTitles.map((key, i) => {
        return {
          title: form[`title${i}`],
          description: form[`description${i}`]
      }
    });

  }

  onSubmit(privacyForm: NgForm) {
    this.newPrivacyContent = privacyForm.value;
    this.mapSectionDescription(privacyForm.value);
    let params = {
      "description": privacyForm.value.pageDescription,
      "details": this.detailParams
    }
    console.log(params);
  }

  getPrivacyPageContent(): void {
    if ((this.staticResourceService.privacyContent) && (this.staticResourceService.privacyContent.location[0].state == this.staticResourceService.getLocation())) {
      this.privacyContent = this.staticResourceService.privacyContent;
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.privacyContent = this.staticContent.find(x => x.name === this.name);
        this.staticResourceService.privacyContent = this.privacyContent;
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
