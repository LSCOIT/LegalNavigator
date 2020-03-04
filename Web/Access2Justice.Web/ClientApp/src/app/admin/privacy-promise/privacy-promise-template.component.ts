import { Component, OnInit} from "@angular/core";
import { NgForm } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";

import {ENV} from 'environment';
import { Global } from "../../global";
import { PrivacyContent } from "../../privacy-promise/privacy-promise";
import { MapLocation } from "../../common/map/map";
import { NavigateDataService } from "../../common/services/navigate-data.service";
import { StaticResourceService } from "../../common/services/static-resource.service";
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
  name: string = "PrivacyNoticePage";
  staticContent: any;
  blobUrl: string = ENV.blobUrl;
  state: string;
  location: MapLocation = {
    state: this.activeRoute.snapshot.queryParams["state"]
  };
  editorConfig = {
    editable: true,
    spellcheck: false,
    height: '10rem',
    minHeight: '5rem',
    placeholder: '',
    translate: 'no',
    "toolbar": [
        ["bold", "italic", "underline", "strikeThrough", "superscript", "subscript"],
        [],
        ["justifyLeft", "justifyCenter", "justifyRight", "justifyFull", "indent", "outdent"],
        ["cut", "copy", "delete", "removeFormat", "undo", "redo"],
        ["paragraph", "blockquote", "removeBlockquote", "horizontalLine", "orderedList", "unorderedList"],
        []
    ]
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
        x => x.name === "PrivacyNoticePage"
      );
    } else {
      this.staticResourceService.getStaticContents(this.location).subscribe(
        response => {
          this.staticContent = response;
          this.privacyContent = this.staticContent.find(
            x => x.name === "PrivacyNoticePage"
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
