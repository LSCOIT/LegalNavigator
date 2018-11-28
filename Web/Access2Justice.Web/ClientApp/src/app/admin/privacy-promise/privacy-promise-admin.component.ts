import { Component, OnInit } from '@angular/core';
import { PrivacyContent, Details} from "../../privacy-promise/privacy-promise";
import { Global } from "../../global";
import { NgForm } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { StaticResourceService } from '../../shared/static-resource.service';
import { AdminService } from '../admin.service';
import { MapLocation, LocationDetails } from '../../shared/map/map';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

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
  location: MapLocation;
  mapLocation: MapLocation;
  locationDetails: LocationDetails;
  state: string;

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global,
    private adminService: AdminService,
    private spinner: NgxSpinnerService,
    private router: Router
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
    if (sessionStorage.getItem("globalMapLocation")) {
      this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.mapLocation = this.locationDetails.location;
    }
    let params = {
      "description": privacyForm.value.pageDescription,
      "details": this.detailParams,
      "name": "PrivacyPromisePage",
      "location": [this.mapLocation],
      "image": this.privacyContent.image,
      "organizationalUnit": this.privacyContent.organizationalUnit
    }
    this.adminService.savePrivacyData(params).subscribe(
      response => {
        this.spinner.hide();
        if (response) {
          this.privacyContent.description = response["description"];
          this.privacyContent.details = response["details"];
          this.router.navigate(['/admin']);
        }
      });
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

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
