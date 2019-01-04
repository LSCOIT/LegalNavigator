import { Injectable } from '@angular/core';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { PersonalizedPlan } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { Global } from '../../../../global';
import { ProfileComponent } from '../../../../profile/profile.component';

@Injectable()
export class SaveButtonService {
  personalizedPlan: PersonalizedPlan;
  userPlan: any = { personalizedPlan: this.personalizedPlan, oId:'' };

  constructor(private personalizedPlanService: PersonalizedPlanService,
    private global: Global,
    private profileComponent: ProfileComponent) { }

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
            this.profileComponent.getPersonalizedPlan();
        }
      });
  }
}
