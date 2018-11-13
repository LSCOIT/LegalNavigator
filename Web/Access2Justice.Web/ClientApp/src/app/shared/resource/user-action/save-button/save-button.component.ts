import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { environment } from '../../../../../environments/environment';
import { Global } from '../../../../global';
import { SavedResources } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { NavigateDataService } from '../../../navigate-data.service';
import { SaveButtonService } from './save-button.service';

@Component({
  selector: 'app-save-button',
  templateUrl: './save-button.component.html',
  styleUrls: ['./save-button.component.css']
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
    private router: Router) {
  }

  externalLogin() {
    this.msalService.loginRedirect(environment.consentScopes);
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
        sessionStorage.setItem(this.global.planSessionKey, JSON.stringify(this.navigateDataService.getData()));
      } else if (document.URL.indexOf("/share/") !== -1) {
        sessionStorage.setItem(this.global.planSessionKey, JSON.stringify(this.personalizedPlanService.planDetails));
      }
      this.savePersonalizationPlan();
    } else {
      this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
      this.personalizedPlanService.saveBookmarkedResource(this.savedResources);
    }
  }

  savePersonalizationPlan() {
    if (this.router.url.indexOf("/plan") !== -1) {
      const params = {
        "personalizedPlan": this.navigateDataService.getData() ? this.navigateDataService.getData() : this.personalizedPlanService.planDetails,
        "oId": this.global.userId,
        "saveActionPlan": true
      }
      this.personalizedPlanService.userPlan(params)
        .subscribe(response => {
          if (response) {
            this.personalizedPlanService.showSuccess("Plan Added to Session");
          }
        });
    }
  }

  savePlanResourcesPostLogin() {
    if (this.router.url.indexOf("/plan") !== -1) {
      //this.planId = this.id;
      //let generatedPersonalizedPlan = this.navigateDataService.getData();
      //if (generatedPersonalizedPlan != undefined) {
      //  this.topics = generatedPersonalizedPlan.topics;
      //  this.planDetailTags = generatedPersonalizedPlan;
      //  this.topicsList = this.personalizedPlanService.createTopicsList(this.topics);
      //  this.planDetails = this.personalizedPlanService.getPlanDetails(this.topics, this.planDetailTags);
      //}
      this.saveButtonService.savePlanToUserProfile(this.navigateDataService.getData());
      //this.saveButtonService.getPlan(this.planId);
    } else {
      this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
      this.tempResourceStorage = [];
      this.tempResourceStorage.push(this.savedResources);
      this.personalizedPlanService.saveResourcesToProfile(this.tempResourceStorage);
    }
  }

  //saveBookmarkedPlan() {
  //  this.planStorage = sessionStorage.getItem(this.global.planSessionKey);
  //  if (this.planStorage) {
  //    this.savePlanResources();
  //    sessionStorage.removeItem(this.global.planSessionKey);
  //  }
  //}

  ngOnInit() {
    //if (this.msalService.getUser()) {
    //  this.saveBookmarkedPlan();
    //}
  }
}
