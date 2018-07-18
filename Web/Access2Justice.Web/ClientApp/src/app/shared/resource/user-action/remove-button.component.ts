import { Component, OnInit, Input } from '@angular/core';
import { PersonalizedPlanService } from '../../../guided-assistant/personalized-plan/personalized-plan.service';
import { RemovePlanTag, UserRemovePlanTag, StepTag, UpdatePlan, UserUpdatePlan, PlanTag } from '../../../guided-assistant/personalized-plan/personalized-plan';
import { ProfileComponent } from '../../../profile/profile.component';
import { environment } from '../../../../environments/environment';
import { PersonalizedPlanComponent } from '../../../guided-assistant/personalized-plan/personalized-plan.component';

@Component({
  selector: 'app-remove-button',
  template: `
  <span (click)="removeSavedResources()">Remove</span>
 `
})
export class RemoveButtonComponent implements OnInit {
  @Input() selectedPlanDetails;
  userId: string = environment.userId;
  isRemoved: boolean;
  removePlanTag: RemovePlanTag = { id: '', oId: '', topicId: '' };
  userRemovePlanTag: UserRemovePlanTag = { id: '', planId: '', oId: '', topicId: '', type: '' };
  updatedStep: StepTag = { id: '', order: '', markCompleted: false };
  updatedSteps: Array<StepTag>;
  planTag: PlanTag = { topicId: '', stepTags: this.updatedSteps };
  planTags: Array<PlanTag>;
  updatePlan: UpdatePlan = { id: '', oId: '', planTags: this.planTags };
  userUpdatePlan: UserUpdatePlan = { id: '', planId: '', oId: '', planTags: this.planTags, type: '' };

  constructor(private personalizedPlanService: PersonalizedPlanService, private profileComponent: ProfileComponent,
    private personalizedPlanComponent: PersonalizedPlanComponent) { }

  removeSavedResources() {
    this.updatedSteps = [];
    this.planTags = [];
    this.selectedPlanDetails.planDetails.planTags.forEach(plan => {
      if (plan.topicId !== this.selectedPlanDetails.topicId) {
        plan.stepTags.forEach(step => {
          this.updatedStep = { id: step.id.id, order: step.order, markCompleted: step.markCompleted };
          this.updatedSteps.push(this.updatedStep);
        });
        this.planTag = { topicId: plan.topicId, stepTags: this.updatedSteps };
        this.planTags.push(this.planTag);
      }
    });
    this.removePersonalizedPlanTag();
  }

  removePersonalizedPlanTag() {
    if (this.selectedPlanDetails.planDetails.planId) {
      this.removePlanTagFromUserProfile();
    } else {
      this.removePlanTagFromPersonalizedPlan();
    }
  }

  removePlanTagFromUserProfile() {
    this.userUpdatePlan = {
      id: this.selectedPlanDetails.planDetails.id, planId: this.selectedPlanDetails.planDetails.planId,
      oId: this.userId, planTags: this.planTags, type: this.selectedPlanDetails.planDetails.type
    };
    this.personalizedPlanService.userPlan(this.userUpdatePlan)
      .subscribe(response => {
        if (response) {
          this.profileComponent.getPersonalizedPlan();
        }
      });
  }

  removePlanTagFromPersonalizedPlan() {
    this.updatePlan = { id: this.selectedPlanDetails.planDetails.id, oId: this.userId, planTags: this.planTags };
    this.personalizedPlanService.getMarkCompletedUpdatedPlan(this.updatePlan)
      .subscribe(response => {
        if (response) {
          this.personalizedPlanComponent.getTopics();
        }
      });
  }

  ngOnInit() {
  }

}
