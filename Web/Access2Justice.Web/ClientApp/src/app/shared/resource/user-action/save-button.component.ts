import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resources, CreatePlan, StepTag, PlanTag, Steps, ProfileResources } from '../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../guided-assistant/personalized-plan/personalized-plan.service';
import { environment } from '../../../../environments/environment';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';

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
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  userId: string = environment.userId;
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

  constructor(private router: Router,
    private activeRoute: ActivatedRoute,
    private personalizedPlanService: PersonalizedPlanService,
    private modalService: BsModalService) { }

  savePlanResources(template: TemplateRef<any>): void {
    this.resources = { url: '', itemId: '' };
    this.resources.url = this.router.url;
    if (this.activeRoute.snapshot.params['topic']) {
      this.resources.itemId = this.activeRoute.snapshot.params['topic'];
    } else if (this.activeRoute.snapshot.params['id']) {
      this.resources.itemId = this.activeRoute.snapshot.params['id'];
    }
    if (!this.userId) {
      this.personalizedPlanService.saveResourcesToSession(this.resources);
    }
    else {
      if (this.resources.url.startsWith("/plan")) {
        this.planId = this.resources.itemId;
        this.savePlanToProfile(template);
      }
      if (this.resources.url.startsWith("/subtopics")) {
        this.profileResources.resourceTags.push(this.resources.itemId);
        this.saveResourceToProfile(this.profileResources.resourceTags);
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
    this.router.navigate(['profile']);
  }

 saveResourceToProfile(resourceTags) {
      this.profileResources = { oId: environment.userId, resourceTags: resourceTags, type: 'resources' };
      this.personalizedPlanService.userPlan(this.profileResources)
        .subscribe();
    }

  ngOnInit() {
  }

}
