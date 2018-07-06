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
  createPlan: CreatePlan = { planId: '', oId: '', type:'', planTags:[] };
  planDetails: any;
  updatedSteps: Array<StepTag>;
  planTags: Array<PlanTag> =[];
  updatedStep: StepTag = { id: '', order: '', markCompleted: false };
  //updatePlan: UpdatePlan = { id: '', oId: '', planTags: this.planTags };
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
        //this.createPlan = { planId: this.planId, oId: this.userId }
      }
      this.planTags = [];
     // this.updatePlan = { id: '', oId: '', planTags: this.planTags };
      this.personalizedPlanService.getActionPlanConditions(this.planId)
        .subscribe(planDetails => {
          planDetails.planTags.forEach(item => {
            this.updatedSteps = [];
            item.stepTags.forEach(step => {
              this.updatedStep = { id: step.id.id, order: step.order, markCompleted: step.markCompleted }
              this.updatedSteps.push(this.updatedStep);
            })
            this.planTag = { topicId: item.id[0].id, stepTags: this.updatedSteps };
            this.planTags.push(this.planTag);
          });
            
          this.createPlan.planId = planDetails.id;
          this.createPlan.oId = this.userId;
          this.createPlan.planTags = this.planTags;
          this.createPlan.type = planDetails.type;
          this.personalizedPlanService.userPlan(this.createPlan)
            .subscribe(response => {
              console.log(response);
            })
        });
    }
  }

  ngOnInit() {
  }

}
