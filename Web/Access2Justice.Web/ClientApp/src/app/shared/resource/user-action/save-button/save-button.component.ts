import { Component, OnInit, Input, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resources, ProfileResources, SavedResources, PersonalizedPlan, PlanTopic, PlanStep } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { Subject } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { api } from '../../../../../api/api';
import { HttpParams } from '@angular/common/http';
import { ActionPlansComponent } from '../../resource-type/action-plan/action-plans.component';
import { MsalService } from '@azure/msal-angular';
import { Global } from '../../../../global';
import { environment } from '../../../../../environments/environment';
import { SaveButtonService } from './save-button.service';
import { NavigateDataService } from '../../../navigate-data.service';

@Component({
  selector: 'app-save-button',
  templateUrl: './save-button.component.html',
  styleUrls: ['./save-button.component.css']
})
export class SaveButtonComponent implements OnInit {
  @Input() showIcon = true;
  resources: Resources;
  savedResources: SavedResources;
  resourceTags: Array<SavedResources> = [];
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  @Input() id: string;
  @Input() type: string;
  @Input() resourceDetails: any = {};
  planId: string;
  resourceStorage: any;
  planStorage: any;
  //personalizedPlanStep: PlanStep = { stepId: '', title: '', description: '', order: 1, isComplete: false, resources: [], topicIds: [] };
  //personalizedPlanSteps: Array<PlanStep>;
  ////planTopic: PlanTopic = { topicId: '', steps: this.personalizedPlanSteps };
  //planTopics: Array<PlanTopic>;
  //personalizedPlan: PersonalizedPlan = { id: '', topics: this.planTopics, isShared: false };
  //resourceIds: Array<string>;
  //topicIds: Array<string>;
  //stepIds: Array<string>;
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
    private navigateDataService: NavigateDataService) {
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
    if (this.type === "Plan") {
      sessionStorage.setItem(this.global.planSessionKey, JSON.stringify(this.id));
    } else {
      this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
      this.personalizedPlanService.saveBookmarkedResource(this.savedResources);
    }
  }

  savePlanResourcesPostLogin() {
    if (this.type === "Plan") {
      //this.planId = this.id;
      //let generatedPersonalizedPlan = this.navigateDataService.getData();
      //if (generatedPersonalizedPlan != undefined) {
      //  this.topics = generatedPersonalizedPlan.topics;
      //  this.planDetailTags = generatedPersonalizedPlan;
      //  this.topicsList = this.personalizedPlanService.createTopicsList(this.topics);
      //  this.planDetails = this.personalizedPlanService.getPlanDetails(this.topics, this.planDetailTags);
      //}
      this.saveButtonService.savePlanToUserProfile(this.plan);
      //this.saveButtonService.getPlan(this.planId);
    } else {
      this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
      this.tempResourceStorage = [];
      this.tempResourceStorage.push(this.savedResources);
      this.personalizedPlanService.saveResourcesToProfile(this.tempResourceStorage);
    }
  }

  saveBookmarkedPlan() {
    this.planStorage = sessionStorage.getItem(this.global.planSessionKey);
    if (this.planStorage) {
      this.savePlanResources();
      sessionStorage.removeItem(this.global.planSessionKey);
    }
  }

  ngOnInit() {
    if (this.msalService.getUser()) {
      this.saveBookmarkedPlan();
    }
  }
}
