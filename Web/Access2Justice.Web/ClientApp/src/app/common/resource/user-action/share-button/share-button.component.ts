import {
  Component,
  HostListener,
  Input,
  OnInit,
  TemplateRef,
  ViewChild,
} from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { MsalService } from "@azure/msal-angular";
import { BsModalService } from "ngx-bootstrap/modal";
import { BsModalRef } from "ngx-bootstrap/modal/bs-modal-ref.service";

import { ENV } from "environment";
import { Global, UserStatus } from "../../../../global";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { NavigateDataService } from "../../../services/navigate-data.service";
import { Share } from "./share.model";
import { ShareService } from "./share.service";
import { MapLocation } from "../../../map/map";

@Component({
  selector: "app-share-button",
  templateUrl: "./share-button.component.html",
  styleUrls: ["./share-button.component.css"],
  // tslint:disable-next-line
  host: {
    "[class.app-resource-button]": "true",
  },
})
export class ShareButtonComponent implements OnInit {
  @Input() showIcon = true;
  modalRef: BsModalRef;
  @Input() id: string;
  @Input() type: string;
  @Input() url: string;
  @Input() topicsList: any[];
  @Input() webResourceUrl: string;
  @ViewChild("template") public modalTemplate: TemplateRef<any>;
  userId: string;
  sessionKey = "showModal";
  shareInput: Share = {
    ResourceId: "",
    UserId: "",
    Url: "",
    Location: null,
    ResourceType: "",
  };
  shareView: any;
  blank = "";
  permaLink = "";
  showGenerateLink = true;
  resourceUrl: string =
    window.location.protocol + "//" + window.location.host + "/share/";
  emptyId = "{00000000-0000-0000-0000-000000000000}";
  @Input() addLinkClass = false;
  location: MapLocation;

  constructor(
    private modalService: BsModalService,
    private shareService: ShareService,
    private activeRoute: ActivatedRoute,
    public global: Global,
    private msalService: MsalService,
    private navigateDataService: NavigateDataService,
    private router: Router,
    private personalizedPlanService: PersonalizedPlanService
  ) {
    if (
      global.role === UserStatus.Shared &&
      location.pathname.indexOf(global.shareRouteUrl) >= 0
    ) {
      global.showShare = false;
    } else {
      global.showShare = true;
    }
  }

  @HostListener("click")
  openModal() {
    this.savePersonalizationPlan();
    if (!this.global.userId) {
      sessionStorage.setItem(this.sessionKey, "true");
      this.externalLogin();
    } else {
      this.checkPermalink();
    }
  }

  formSharedResources = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
  });

  close() {
    this.modalRef.hide();
    this.formSharedResources.reset();
  }

  checkPermalink() {
    this.buildParams();
    this.shareService.checkLink(this.shareInput).subscribe((response) => {
      if (response) {
        this.shareView = response;
        if (this.shareView.permaLink) {
          this.showGenerateLink = false;
          this.permaLink = this.getPermaLink();
        }
      }
      this.modalRef = this.modalService.show(this.modalTemplate);
    });
  }

  savePersonalizationPlan() {
    if (this.router.url.indexOf("/plan") !== -1) {
      const params = {
        personalizedPlan: this.navigateDataService.getData(),
        oId: this.global.userId,
        saveActionPlan: true,
      };
      this.personalizedPlanService.userPlan(params).subscribe((response) => {
        if (response) {
          console.log("Plan Added to Session");
        }
      });
    }
  }

  generateLink() {
    debugger;
    this.buildParams();
    this.shareService.generateLink(this.shareInput).subscribe((response) => {
      if (response != undefined) {
        this.shareView = response;
        this.permaLink = this.getPermaLink();
        this.showGenerateLink = false;
      }
    });
  }

  removeLink(): void {
    this.buildParams();
    this.shareService.removeLink(this.shareInput).subscribe((response) => {
      if (response != undefined) {
        this.close();
        this.showGenerateLink = true;
        this.permaLink = this.blank;
      }
    });
  }

  getActiveParam() {
    if (this.activeRoute.snapshot.params["topic"] != null) {
      return this.activeRoute.snapshot.params["topic"];
    }
    if (this.activeRoute.snapshot.params["id"] != null) {
      return this.activeRoute.snapshot.params["topic"];
    }
  }

  copyLink(inputElement) {
    inputElement.select();
    document.execCommand("copy");
    inputElement.setSelectionRange(0, 0);
  }

  getPermaLink() {
    if (this.shareView.permaLink) {
      return this.resourceUrl + this.shareView.permaLink;
    } else {
      return this.blank;
    }
  }

  externalLogin() {
    this.msalService.loginRedirect(ENV.consentScopes);
  }

  submitShareLink() {
    debugger;
    let topicIds = this.personalizedPlanService.topicsList
      .filter((x) => x.isSelected)
      .map((x) => x.topic.topicId);
    let plan = { id: this.personalizedPlanService.planDetails.id, topicIds };
    this.shareService
      .shareLinkToUser({
        Oid: this.global.userId,
        Email: this.formSharedResources.value.email,
        ItemId: this.id,
        ResourceType: this.type,
        ResourceDetails: {},
        Plan: plan,
      })
      .subscribe(() => {
        this.formSharedResources.reset();
      });
  }

  ngOnInit() {
    if (this.global.userId) {
      const hasLoggedIn = sessionStorage.getItem(this.sessionKey);
      if (hasLoggedIn) {
        sessionStorage.removeItem(this.sessionKey);
        this.openModal();
      }
    }
  }

  buildParams() {
    debugger;
    this.location = JSON.parse(
      sessionStorage.getItem("globalMapLocation")
    ).location;
    this.shareInput.Location = this.location;
    if (this.type === "Plan") {
      let topicIds = this.personalizedPlanService.topicsList
        .filter((x) => x.isSelected)
        .map((x) => x.topic.topicId);
      let plan = { id: this.personalizedPlanService.planDetails.id, topicIds };
      this.shareInput.Plan = plan;
      this.shareInput.Url = this.buildUrl();
      this.shareInput.ResourceId = this.id;
    } else if (this.id || this.type === "WebResources") {
      this.shareInput.Url = this.buildUrl();
      this.shareInput.ResourceId = this.id;
    } else {
      this.shareInput.Url = location.pathname;
      this.shareInput.ResourceId = this.getActiveParam();
    }
    this.shareInput.UserId = this.global.userId;
    this.shareInput.ResourceType = this.type;
  }

  buildUrl() {
    if (this.type === "Topics") {
      return "/topics/" + this.id;
    }
    if (this.type === "Guided Assistant") {
      return "/guidedassistant/" + this.id;
    }
    if (this.type === "Plan") {
      return "/plan/" + this.id;
    }
    if (this.type === "Forms" || this.type === "Related Readings") {
      return this.url;
    }
    if (this.type === "WebResources") {
      this.id = this.emptyId;
      if (this.url) {
        return this.url;
      } else if (this.webResourceUrl) {
        return this.webResourceUrl;
      }
    } else {
      return "/resource/" + this.id;
    }
  }
}
