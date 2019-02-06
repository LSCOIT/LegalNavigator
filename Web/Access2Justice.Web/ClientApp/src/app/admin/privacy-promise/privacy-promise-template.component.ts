import { Component, OnInit } from "@angular/core";
import { NgForm } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";

import ENV from 'env';
import { Global } from "../../global";
import { PrivacyContent } from "../../privacy-promise/privacy-promise";
import { MapLocation } from "../../shared/map/map";
import { NavigateDataService } from "../../shared/services/navigate-data.service";
import { StaticResourceService } from "../../shared/services/static-resource.service";
import { AdminService } from "../admin.service";

@Component({
  selector: "app-privacy-promise-template",
  templateUrl: "./privacy-promise-template.component.html",
  styleUrls: ["../admin-styles.css"]
})
export class PrivacyPromiseTemplateComponent implements OnInit {
  detailParams: any;
  privacyContent: PrivacyContent;
  newPrivacyContent;
  name: string = "PrivacyPromisePage";
  staticContent: any;
  blobUrl: string = ENV().blobUrl;
  state: string;
  location: MapLocation = {
    state: this.activeRoute.snapshot.queryParams["state"]
  };

  constructor(
    private staticResourceService: StaticResourceService,
    private global: Global,
    private adminService: AdminService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService,
    private toastr: ToastrService
  ) {}

  mapSectionDescription(form) {
    let numOfTitles = Object.keys(form).filter(
      key => key.indexOf("title") > -1
    );
    this.detailParams = numOfTitles.map((key, i) => {
      return {
        title: form[`title${i}`],
        description: form[`description${i}`]
      };
    });
  }

  createPrivacyParams(privacyForm) {
    this.newPrivacyContent = {
      description:
        privacyForm.value.pageDescription || this.privacyContent.description,
      details: this.detailParams || this.privacyContent.details,
      name: this.name,
      location: [this.location],
      image: this.privacyContent.image,
      organizationalUnit: this.privacyContent.organizationalUnit
    };
  }

  onSubmit(privacyForm: NgForm) {
    this.spinner.show();
    this.mapSectionDescription(privacyForm.value);
    this.createPrivacyParams(privacyForm);
    this.adminService.savePrivacyData(this.newPrivacyContent).subscribe(
      response => {
        this.spinner.hide();
        this.toastr.success("Page updated successfully");
      },
      error => {
        console.log(error);
        this.toastr.warning("Page was not updated. Please check the console");
      }
    );
  }

  getPrivacyPageContent(): void {
    if (this.navigateDataService.getData()) {
      this.staticContent = this.navigateDataService.getData();
      this.privacyContent = this.staticContent.find(
        x => x.name === "PrivacyPromisePage"
      );
    } else {
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => {
          this.staticContent = response;
          this.privacyContent = this.staticContent.find(
            x => x.name === "PrivacyPromisePage"
          );
        },
        error => this.router.navigateByUrl("error")
      );
    }
  }

  ngOnInit() {
    this.getPrivacyPageContent();
  }
}
