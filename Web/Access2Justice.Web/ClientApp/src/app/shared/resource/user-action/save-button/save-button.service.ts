import { Injectable } from '@angular/core';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { HttpParams } from '@angular/common/http';
import { PersonalizedPlan } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { Global } from '../../../../global';
import { Router } from '@angular/router';
import { ProfileComponent } from '../../../../profile/profile.component';
import { PersonalizedPlanComponent } from '../../../../guided-assistant/personalized-plan/personalized-plan.component';

@Injectable()
export class SaveButtonService {
  personalizedPlan: PersonalizedPlan;
  userPlan: any = { personalizedPlan: this.personalizedPlan, oId:'' };

  constructor(private personalizedPlanService: PersonalizedPlanService,
    private global: Global,
    private profileComponent: ProfileComponent,
    private personalizedPlanComponent: PersonalizedPlanComponent) { }

  savePlanToUserProfile(plan) {
    this.personalizedPlan = plan;
    const params = {
      "personalizedPlan": this.personalizedPlan,
      "oId": this.global.userId
    }
    this.personalizedPlanService.userPlan(params)
      .subscribe(response => {
        if (response) {
          this.personalizedPlanService.showSuccess('Plan saved to profile');
          sessionStorage.removeItem(this.global.planSessionKey);
          if (this.global.userId) {
            this.profileComponent.getPersonalizedPlan();
          } else {
            this.personalizedPlanComponent.getTopics();
          }
        }
      });
  }
}
