import { Component, OnInit, Input, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resources, ProfileResources, SavedResources, PersonalizedPlan, PlanTopic, PlanStep } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { ArrayUtilityService } from '../../../array-utility.service';
import { Subject } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { api } from '../../../../../api/api';
import { HttpParams } from '@angular/common/http';
import { ActionPlansComponent } from '../../resource-type/action-plan/action-plans.component';
import { MsalService } from '@azure/msal-angular';
import { Global } from '../../../../global';
import { environment } from '../../../../../environments/environment';

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
  personalizedPlanStep: PlanStep = { stepId: '', title: '', description: '', order: 1, isComplete: false, resources: [], topicIds: [] };
  personalizedPlanSteps: Array<PlanStep>;
  planTopic: PlanTopic = { topicId: '', steps: this.personalizedPlanSteps };
  planTopics: Array<PlanTopic>;
  personalizedPlan: PersonalizedPlan = { id: '', topics: this.planTopics, isShared: false };
  resourceIds: Array<string>;
  topicIds: Array<string>;
  stepIds: Array<string>;
  @Input() addLinkClass: boolean = false;
  planStepCount: number = 0;
  planStepIds: Array<string>;
  planTopicIds: Array<string>;
  tempResourceStorage: any;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private arrayUtilityService: ArrayUtilityService,
    private msalService: MsalService,
    private global: Global) {
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
      this.planId = this.id;
      this.getPlan(this.planId);
    } else {
      this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
      this.tempResourceStorage = [];
      this.tempResourceStorage.push(this.savedResources);
      this.personalizedPlanService.saveResourcesToProfile(this.tempResourceStorage);
    }
  }

  getTopicIds(topics): Array<string> {
    this.topicIds = [];
    topics.forEach(topic => {
      this.topicIds.push(topic.topicId);
    });
    return this.topicIds;
  }

  savePlanToUserProfile(plan) {
    this.planStepCount = 0;
    let params = new HttpParams()
      .set("oid", this.global.userId)
      .set("type", "plan");
    this.personalizedPlanService.getUserSavedResources(params)
      .subscribe(response => {
        if (response && response.length > 0) {
          this.BuildUserPlan(response, plan);
          this.addRemainingTopicsToPlan(plan);
          this.planId = response[0].id;
        }
        this.personalizedPlan = { id: this.planId, topics: this.planTopics, isShared: false };
        const params = {
          "id": this.personalizedPlan.id,
          "topics": this.personalizedPlan.topics,
          "isShared": this.personalizedPlan.isShared
        }
        this.personalizedPlanService.userPlan(params)
          .subscribe(response => {
            if (response) {
              if (this.planStepCount > 0) {
                this.personalizedPlanService.showSuccess('Plan saved to profile');
              } else {
                this.personalizedPlanService.showWarning('Plan already saved to profile');
              }
            }
          });
      });
  }

  BuildUserPlan(response, plan) {
    response.forEach(property => {
      if (property.topics) {
        this.planTopics = [];
        this.planTopic = { topicId: '', steps: this.personalizedPlanSteps };
        this.planTopicIds = this.getTopicIds(plan);
        this.BuildUserPlanWithExistingTopics(property.topics, plan);
        this.topicIds = this.getTopicIds(property.topics);
        this.planTopicIds.forEach(topicId => {
          if (!this.arrayUtilityService.checkObjectExistInArray(this.topicIds, topicId)) {
            this.planStepCount++;
          }
        });
      }
    });
  }

  BuildUserPlanWithExistingTopics(topics, plan) {
    topics.forEach(topic => {
      plan.forEach(planTopic => {
        this.personalizedPlanSteps = [];
        this.stepIds = [];
        this.BuildPlanStepsForExistingTopicId(planTopic, topic);
      });
      if (this.arrayUtilityService.checkObjectExistInArray(this.planTopicIds, topic.topicId)) {
        this.topicTagsForMatchingTopicId(topic);
      } else {
        this.topicTagsForNonMatchingTopicId(topic);
      }
      this.planTopic = { topicId: topic.topicId, steps: this.personalizedPlanSteps };
      this.planTopics.push(this.planTopic);
    });
  }

  BuildPlanStepsForExistingTopicId(planTopic, topic) {
    if (planTopic.topicId === topic.topicId) {
      planTopic.steps.forEach(step => {
        this.personalizedPlanStep = {
          stepId: step.stepId, title: step.title,
          description: step.description, order: step.order, isComplete: step.isComplete,
          resources: step.resources, topicIds: []
        };
        this.stepIds.push(step.stepId);
        this.personalizedPlanSteps.push(this.personalizedPlanStep);
      });
    }
  }

  addRemainingTopicsToPlan(plan) {
    plan.forEach(topic => {
      if (!this.arrayUtilityService.checkObjectExistInArray(this.topicIds, topic.topicId)) {
        this.personalizedPlanSteps = [];
        this.personalizedPlanSteps = topic.steps;
        this.planTopic = { topicId: topic.topicId, steps: this.personalizedPlanSteps };
        this.planTopics.push(this.planTopic);
      }
    });
  }

  topicTagsForMatchingTopicId(topic) {
    this.planStepIds = [];
    this.planStepCount = 0;
    topic.steps.forEach(step => {
      this.planStepIds.push(step.stepId);
      if (!this.arrayUtilityService.checkObjectExistInArray(this.stepIds, step.stepId)) {
        this.buildPlanSteps(step);
      }
    });
    this.stepIds.forEach(stepId => {
      if (!this.arrayUtilityService.checkObjectExistInArray(this.planStepIds, stepId)) {
        this.planStepCount++;
      }
    });
  }

  topicTagsForNonMatchingTopicId(topic) {
    topic.steps.forEach(step => {
      this.buildPlanSteps(step);
    });
  }

  buildPlanSteps(step) {
    this.personalizedPlanStep = {
      stepId: step.stepId, title: step.title,
      description: step.description, order: step.order, isComplete: step.isComplete,
      resources: step.resources, topicIds: []
    };
    this.personalizedPlanSteps.push(this.personalizedPlanStep);
  }

  getPlan(planId) {
    this.personalizedPlanService.getActionPlanConditions(planId)
      .subscribe((plan) => {
        this.planTopics = [];
        this.personalizedPlanSteps = [];
        this.planTopic = { topicId: '', steps: this.personalizedPlanSteps };
        plan.topics.forEach(item => {
          this.personalizedPlanSteps = this.getPersonalizedPlanSteps(item.steps);
          this.planTopic = { topicId: item.topicId, steps: this.personalizedPlanSteps };
          this.planTopics.push(this.planTopic);
        });
        this.savePlanToUserProfile(this.planTopics);
      });
  }

  getPersonalizedPlanSteps(steps): Array<PlanStep> {
    this.personalizedPlanSteps = [];
    steps.forEach(step => {
      this.personalizedPlanStep = {
        stepId: step.stepId, title: step.title, description: step.description,
        order: step.order, isComplete: step.isComplete, resources: this.personalizedPlanService.getResourceIds(step.resources), topicIds: []
      };
      this.personalizedPlanSteps.push(this.personalizedPlanStep);
    });
    return this.personalizedPlanSteps;
  }

  saveBookmarkedPlan() {
    this.planStorage = sessionStorage.getItem(this.global.planSessionKey);
    if (this.planStorage) {
      this.savePlanResources();
      sessionStorage.removeItem(this.global.planSessionKey);
    }
  }

  ngOnInit() {
    //this.saveBookmarkedResource();
    if (this.msalService.getUser()) {
      this.saveBookmarkedPlan();
    }
  }
}
