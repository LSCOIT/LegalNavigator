import { Component, OnInit, Input, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resources, CreatePlan, StepTag, PlanTag, Steps, ProfileResources, SavedResources } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { UtilityService } from '../../../utility.service';
import { Subject } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { api } from '../../../../../api/api';

@Component({
  selector: 'app-save-button',
  templateUrl: './save-button.component.html',
  styleUrls: ['./save-button.component.css']
})
export class SaveButtonComponent implements OnInit {
  resources: Resources;
  savedResources: SavedResources;
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  @Input() id: string;
  @Input() type: string;
  @Input() resourceDetails: any = {};
  userId: string;
  planId: string;
  createPlan: CreatePlan = { planId: '', oId: '', type: '', planTags: [] };
  planDetails: any;
  updatedSteps: Array<StepTag>;
  planTags: Array<PlanTag> = [];
  updatedStep: StepTag = { id: '', order: '', markCompleted: false };
  steps: Array<Steps>;
  planTag: PlanTag = { topicId: '', stepTags: this.steps };
  isSavedPlan: boolean = false;
  modalRef: BsModalRef;
  @ViewChild('template') public templateref: TemplateRef<any>;
  sessionKey: string = "bookmarkedResource";
  resourceStorage: any;

  constructor(private router: Router,
    private activeRoute: ActivatedRoute,
    private personalizedPlanService: PersonalizedPlanService,
    private modalService: BsModalService,
    private utilityService: UtilityService) {
    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      this.userId = profileData["UserId"];
    }
  }

  externalLogin() {
    var form = document.createElement('form');
    form.setAttribute('method', 'GET');
    form.setAttribute('action', api.loginUrl);
    document.body.appendChild(form);
    form.submit();
  }

  savePlanResources(template: TemplateRef<any>): void {
    if (!this.userId) {
      this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails : this.resourceDetails };
      this.personalizedPlanService.saveResourcesToSession(this.savedResources);
      this.externalLogin();
    } else {
      if (this.type === "Plan") {
        this.planId = this.id;
        this.savePlanToProfile(template);
      } else {
        this.profileResources.resourceTags = [];
        this.savedResources = { itemId: '', resourceType: '', resourceDetails: '' };
        this.personalizedPlanService.getUserPlanId(this.userId)
          .subscribe(response => {
            if (response != undefined) {
              response.forEach(property => {
                if (property.resourceTags) {
                  property.resourceTags.forEach(resource => {
                    this.profileResources.resourceTags.push(resource);
                  })
                }
              });
            }
            this.savedResources = { itemId: this.id, resourceType: this.type, resourceDetails: this.resourceDetails };
            if (!this.utilityService.checkObjectExistInArray(this.profileResources.resourceTags, this.savedResources)) {
              this.profileResources.resourceTags.push(this.savedResources);
              this.saveResourceToProfile(this.profileResources.resourceTags, template);
            }
          });
      }
    }
  }

  savePlanToProfile(template) {
    this.planTags = [];
    this.personalizedPlanService.getActionPlanConditions(this.planId)
      .subscribe(planDetails => {
        planDetails.planTags.forEach(plan => {
          this.updatedSteps = [];
          plan.stepTags.forEach(step => {
            this.updatedStep = { id: step.id.id, order: step.order, markCompleted: step.markCompleted }
            this.updatedSteps.push(this.updatedStep);
          })
          this.planTag = { topicId: plan.id[0].id, stepTags: this.updatedSteps };
          this.planTags.push(this.planTag);
        });
        this.createPlan = { planId: planDetails.id, oId: this.userId, type: planDetails.type, planTags: this.planTags }
        this.personalizedPlanService.userPlan(this.createPlan)
          .subscribe(() => {
            this.isSavedPlan = true;
            this.modalRef = this.modalService.show(template);
          });
      });
  }

  close() {
    this.modalRef.hide();
  }

  saveResourceToProfile(resourceTags, template) {
    this.profileResources = { oId: this.userId, resourceTags: resourceTags, type: 'resources' };
    this.personalizedPlanService.userPlan(this.profileResources)
      .subscribe(() => {
        this.isSavedPlan = true;
        this.modalRef = this.modalService.show(template);
      });
  }

  saveBookmarkedResource() {
    this.resourceStorage = sessionStorage.getItem(this.sessionKey);
    if (this.resourceStorage && this.resourceStorage.length > 0) {
      this.resourceStorage = JSON.parse(this.resourceStorage);
      this.id = this.resourceStorage[0].itemId;
      this.type = this.resourceStorage[0].resourceType;
      this.resourceDetails = this.resourceStorage[0].resourceDetails;
      this.savePlanResources(this.templateref);
      sessionStorage.removeItem(this.sessionKey);
    }
  }

  ngOnInit() {
    this.saveBookmarkedResource();
  }

}

