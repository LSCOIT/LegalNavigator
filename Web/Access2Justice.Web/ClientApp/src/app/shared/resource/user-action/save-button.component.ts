import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Resources, CreatePlan, UpdatePlan, StepTag, PlanTag, Steps } from '../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../guided-assistant/personalized-plan/personalized-plan.service';

@Component({
  selector: 'app-save-button',
  template: `
<img src="./assets/images/small-icons/save.svg" class="nav-icon" aria-hidden="true" (click) ="savePlanResources()" />
<span>Save to profile</span>
 `
})
export class SaveButtonComponent implements OnInit {
  resources: Resources;
  @Input() savedFrom: string;
  userId: string = "User Id";
  planId: string;
  createPlan: CreatePlan = { planId: '', oId: '', type: '', planTags: [] };
  planDetails: any;
  updatedSteps: Array<StepTag>;
  planTags: Array<PlanTag> = [];
  updatedStep: StepTag = { id: '', order: '', markCompleted: false };
  steps: Array<Steps>;
  planTag: PlanTag = { topicId: '', stepTags: this.steps };

  constructor(private router: Router,
    private activeRoute: ActivatedRoute,
    private personalizedPlanService: PersonalizedPlanService) { }

  savePlanResources(): void {
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
      }
      if (this.resources.url.startsWith("/subtopics")) {
        //this.resources = '';
      }
      this.savePlanToProfile();
    }
  }

  savePlanToProfile() {
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
          .subscribe()
      });
  }

  ngOnInit() {
  }

}
