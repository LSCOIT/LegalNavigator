import { Component, OnInit } from '@angular/core';
import { PrivacyContent } from "../../privacy-promise/privacy-promise";
import { Global } from "../../global";
import { NgForm } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { StaticResourceService } from '../../shared/static-resource.service';
import { AdminService } from '../admin.service';
import { MapLocation } from '../../shared/map/map';
import { Router, ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-privacy-promise-template',
  templateUrl: './privacy-promise-template.component.html',
  styleUrls: ['../admin-styles.css']
})

export class PrivacyPromiseTemplateComponent implements OnInit {
  detailParams: any;
  privacyContent: PrivacyContent;
  newPrivacyContent;
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
    private navigateDataService: NavigateDataService,
    private toastr: ToastrService,
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
    this.mapSectionDescription(privacyForm.value);

    this.newPrivacyContent = {
      "description": privacyForm.value.pageDescription,
      "details": this.detailParams,
      "name": "PrivacyPromisePage",
      "location": [this.location],
      "image": this.privacyContent.image,
      "organizationalUnit": this.privacyContent.organizationalUnit
    }
    this.adminService.savePrivacyData(this.newPrivacyContent).subscribe(
      response => {
        this.spinner.hide();
        this.toastr.success("Page updated successfully");
      }, error => {
        this.toastr.error("Page was not updated. Please try again later");
      });
  }

  getPrivacyPageContent(): void {
    if (this.navigateDataService.getData()) {
      this.staticContent = this.navigateDataService.getData();
      this.privacyContent = this.staticContent.find(x => x.name === "PrivacyPromisePage");
    } else {
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => {
          console.log(response);
          this.staticContent = response;
          this.privacyContent = this.staticContent.find(x => x.name === "PrivacyPromisePage");
        },
        error => this.router.navigateByUrl('error'));
    }
  }

  ngOnInit() {
    this.getPrivacyPageContent();
  }
}
