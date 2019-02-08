import { Component, Input, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { MsalService } from "@azure/msal-angular";

import {ENV} from 'environment';
import { Global } from "../../../../global";
import { SavedResources } from "../../../../guided-assistant/personalized-plan/personalized-plan";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { NavigateDataService } from "../../../services/navigate-data.service";
import { SaveButtonService } from "./save-button.service";

@Component({
  selector: "app-save-button",
  templateUrl: "./save-button.component.html",
  styleUrls: ["./save-button.component.css"]
})
export class SaveButtonComponent implements OnInit {
  @Input() showIcon = true;
  savedResources: SavedResources;
  resourceTags: Array<SavedResources> = [];
  @Input() id: string;
  @Input() type: string;
  @Input() resourceDetails: any = {};
  plan: string;
  resourceStorage: any;
  planStorage: any;
  @Input() addLinkClass: boolean = false;
  planStepCount: number = 0;
  planStepIds: Array<string>;
  planTopicIds: Array<string>;
  tempResourceStorage: any;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private msalService: MsalService,
    private global: Global,
    private saveButtonService: SaveButtonService,
    private navigateDataService: NavigateDataService,
    private router: Router
  ) {}

  externalLogin() {
    this.msalService.loginRedirect(ENV.consentScopes);
  }

  savePlanResources(): void {
    if (!this.msalService.getUser()) {
      this.savePlanResourcesPreLogin();
    } else {
      this.savePlanResourcesPostLogin();
    }
  }

  savePlanResourcesPreLogin() {
    if (this.router.url.indexOf("/plan") !== -1) {
      if (this.navigateDataService.getData()) {
        sessionStorage.setItem(
          this.global.planSessionKey,
          JSON.stringify(this.navigateDataService.getData())
        );
      } else if (document.URL.indexOf("/share/") !== -1) {
        sessionStorage.setItem(
          this.global.planSessionKey,
          JSON.stringify(this.personalizedPlanService.planDetails)
        );
      }
      this.savePersonalizationPlan();
    } else {
      this.savedResources = {
        itemId: this.id,
        resourceType: this.type,
        resourceDetails: this.resourceDetails
      };
      this.personalizedPlanService.saveBookmarkedResource(this.savedResources);
    }
  }

  savePersonalizationPlan() {
    const params = {
      personalizedPlan: this.navigateDataService.getData()
        ? this.navigateDataService.getData()
        : this.personalizedPlanService.planDetails,
      oId: this.global.userId,
      saveActionPlan: true
    };
    this.personalizedPlanService.userPlan(params).subscribe(response => {
      if (response) {
        this.personalizedPlanService.showSuccess("Plan Added to Session");
      }
    });
  }

  savePlanResourcesPostLogin() {
    if (this.router.url.indexOf("/plan") !== -1) {
      this.savePlanPostLogin();
    } else {
      this.saveResourcesPostLogin();
    }
  }

  savePlanPostLogin() {
    if (this.navigateDataService.getData()) {
      this.saveButtonService.savePlanToUserProfile(
        this.navigateDataService.getData()
      );
    } else {
      this.personalizedPlanService
        .getActionPlanConditions(this.id)
        .subscribe(plan => {
          if (plan) {
            this.saveButtonService.savePlanToUserProfile(plan);
          }
        });
    }
  }

  saveResourcesPostLogin() {
    this.savedResources = {
      itemId: this.id,
      resourceType: this.type,
      resourceDetails: this.resourceDetails
    };
    this.tempResourceStorage = [];
    this.tempResourceStorage.push(this.savedResources);
    this.personalizedPlanService.saveResourceToProfilePostLogin(
      this.tempResourceStorage
    );
  }

  ngOnInit() {}
}
