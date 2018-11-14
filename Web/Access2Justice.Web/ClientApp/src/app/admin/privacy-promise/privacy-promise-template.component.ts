import { Component, OnInit } from '@angular/core';
import { PrivacyContent, Details} from "../../privacy-promise/privacy-promise";
import { Global } from "../../global";
import { NgForm } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { StaticResourceService } from '../../shared/static-resource.service';
import { AdminService } from '../admin.service';
import { MapLocation } from '../../shared/map/map';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { NavigateDataService } from '../../shared/navigate-data.service';

@Component({
  selector: 'app-privacy-promise-template',
  templateUrl: './privacy-promise-template.component.html',
  styleUrls: ['../admin-styles.css']
})

export class PrivacyPromiseTemplateComponent implements OnInit {
  detailParams: any;
  newPrivacyContent: any;
  privacyContent: PrivacyContent;
  name: string = 'PrivacyPromisePage';
  staticContent: any;
  blobUrl: string = environment.blobUrl;
  state: string;
  location: MapLocation = {
    state: this.activeRoute.snapshot.queryParams["state"]
  }

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global,
    private adminService: AdminService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService
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
    this.spinner.show();
    this.newPrivacyContent = privacyForm.value;
    this.mapSectionDescription(privacyForm.value);

    let params = {
      "description": privacyForm.value.pageDescription,
      "details": this.detailParams,
      "name": "PrivacyPromisePage",
      "location": [this.location],
      "image": this.privacyContent.image,
      "organizationalUnit": this.privacyContent.organizationalUnit
    }
    this.adminService.savePrivacyData(params).subscribe(
      response => {
        this.spinner.hide();
        if (response) {
          this.privacyContent.description = response["description"];
          this.privacyContent.details = response["details"];
          this.router.navigate(['/privacy']);
        }
      });
  }

  getPrivacyPageContent(): void {
    if (this.navigateDataService.getData()) {
      this.staticContent = this.navigateDataService.getData();
      this.privacyContent = this.staticContent.find(x => x.name === "PrivacyPromisePage");
    } else {
      console.log(this.location);
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => this.privacyContent = response.find(x => x.name === "PrivacyPromisePage"),
        error => this.router.navigateByUrl('error'));
    }
  }

  ngOnInit() {
    this.getPrivacyPageContent();
  }

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
