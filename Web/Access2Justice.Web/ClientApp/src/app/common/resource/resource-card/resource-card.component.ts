import { Component, Input, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { Router } from "@angular/router";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { Global, UserStatus } from "../../../global";
import { StateCodeService } from "../../services/state-code.service";
import { LocationDetails } from "../../map/map";
import {ENV} from 'environment';

@Component({
  selector: "app-resource-card",
  templateUrl: "./resource-card.component.html",
  styleUrls: ["./resource-card.component.scss"]
})
export class ResourceCardComponent implements OnInit {
  @Input() personalizedResources;
  @Input() resource: any;
  @Input() searchResource: any;
  @Input() showRemoveOption: boolean;
  @Input() sharedResources: boolean;
  @Input() sharedToResources: boolean;
  @Input() showUnshareOption: boolean;
  @Input() showSharedOptions: boolean;
  @Input() isIncomingResource: boolean;
  url: any;
  urlOrigin: string;
  applicationUrl: any = window.location.origin;
  location: any;
  locationDetails: LocationDetails = {
    location: {
      state: "",
      city: "",
      county: "",
      zipCode: ""
    },
    displayLocationDetails: {
      locality: "",
      address: ""
    }
  };
  @ViewChild("template") public templateref: TemplateRef<any>;
  modalRef: BsModalRef;
  resourceTypeList = [
    "Articles",
    "Forms",
    "Guided Assistant",
    "Organizations",
    "Topics",
    "Videos",
    "WebResources",
    "Service Providers"
  ];

  constructor(
    private global: Global,
    private modalService: BsModalService,
    private router: Router,
    private stateCodeService: StateCodeService,
  ) {
    if (
      global.role === UserStatus.Shared &&
      location.pathname.indexOf(global.shareRouteUrl) >= 0
    ) {
      global.showShare = false;
      this.showRemoveOption = false;
      global.showDropDown = false;
    } else {
      global.showShare = true;
      this.showRemoveOption = true;
      global.showDropDown = true;
    }
  }

  getLocation(resource): any {
    this.location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    return resource.location[0].state === this.location.location.state;
  }

  openWarningPopup(resource): any {
    if (this.router.url.startsWith("/profile") && !this.getLocation(resource)) {
      this.modalRef = this.modalService.show(this.templateref);
    }
  }

  close() {
    this.modalRef.hide();
  }

  continue() {
    this.modalRef.hide();
    this.locationDetails.location = this.resource.location[0];
    this.stateCodeService
      .getStateName(this.locationDetails.location.state)
      .subscribe(async response => {
        if (response) {
          ENV.map_type = true;
          this.global.topicsData = null;
          const stateName = response.toString();
          this.locationDetails.location.state = stateName;
          this.locationDetails.displayLocationDetails.address = stateName;
          sessionStorage.setItem(
            "globalSearchMapLocation",
            JSON.stringify(this.locationDetails)
          );
          this.global.notifyLocationUpdate(
            JSON.stringify(this.locationDetails)
          );
          await sleep(1000);
          this.router.navigateByUrl(
            "/" +
              this.resource.resourceType.toLowerCase() +
              "/" +
              this.resource.id
          );
        }
      });
  }

  ngOnInit() {
    if (this.searchResource != null || this.searchResource !== undefined) {
      this.resource = this.searchResource;
    }

    if (this.resource.itemId) {
      this.resource.id = this.resource.itemId;
    }
    try {
      this.urlOrigin = new URL(this.resource.url).origin;
    } catch (e) {
      this.urlOrigin = this.resource.url;
    }
    if (this.resource.resourceType === "Videos") {
      var match = this.resource.url.match(/^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/);
      var videoId = match && match[2];
      if (videoId && videoId.length === 11) {
        this.resource.iframeUrl = '//www.youtube.com/embed/' + videoId;
      }
    }
  }
}

function sleep(ms = 0) {
  return new Promise(r => setTimeout(r, ms));
}
