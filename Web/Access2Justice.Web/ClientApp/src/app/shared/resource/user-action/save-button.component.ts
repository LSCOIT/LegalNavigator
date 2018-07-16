import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resources, CreatePlan, StepTag, PlanTag, Steps, ProfileResources, SavedResources } from '../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../guided-assistant/personalized-plan/personalized-plan.service';
import { environment } from '../../../../environments/environment';
import { SharedService } from '../../shared.service';
import { Subject } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { api } from '../../../../api/api';

@Component({
  selector: 'app-save-button',
  template: `
<img src="./assets/images/small-icons/save.svg" class="nav-icon" aria-hidden="true" (click) ="savePlanResources(template)" />
<span>Save to profile</span>
<ng-template #template>
<div *ngIf="isSavedPlan">
    <button type="button" class="close" aria-label="Close" (click)="close()">
        <span aria-hidden="true">&times;</span>
    </button>
    <div class="row text-center">
        <h3>Added to Profile</h3>
    </div>
    <div class="row text-center">
        <button type="button" (click)="close()" class="btn btn-primary">Close</button>
    </div>
</div>
</ng-template>
`
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
  notifySavePlan: Subject<boolean> = new Subject<boolean>();
  modalRef: BsModalRef;

  constructor(private router: Router,
    private activeRoute: ActivatedRoute,
    private personalizedPlanService: PersonalizedPlanService,
    private modalService: BsModalService,
    private sharedService: SharedService) {
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
    }
    else {
      if (this.type == "Plan") {
        this.planId = this.id;
        this.savePlanToProfile(template);
      }
      else {
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
            if (!this.sharedService.checkObjectExistInArray(this.profileResources.resourceTags, this.savedResources)) {
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
            console.log("Saved Successfully");
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

  ngOnInit() {
  }

}
