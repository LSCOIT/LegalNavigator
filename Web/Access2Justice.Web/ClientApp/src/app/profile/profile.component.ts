import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { PlanSteps } from '../guided-assistant/personalized-plan/personalized-plan';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  topics: string;
  planDetails: Array<PlanSteps> = [];
  activeActionPlan: string = '';
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', PageNumber: 0, Location: '', ResourceIds: '' };
  personalizedResources: any;
  isSavedResources: boolean = false;
  planId: string;
  userId: string = environment.userId;

  constructor(private personalizedPlanService: PersonalizedPlanService) { }

  getTopics(): void {
    if (this.planId) {
      this.personalizedPlanService.getActionPlanConditions(this.planId)
        .subscribe(plan => {
          if (plan) {
            this.topics = plan.planTags;
            this.planDetails = plan;
          }
        });
    }
  }

  getpersonalizedResources() {
    this.personalizedPlanService.getPersonalizedResources(this.resourceFilter)
      .subscribe(response => {
        if (response != undefined) {
          this.personalizedResources = response;
          this.isSavedResources = true;
        }
      });
  }

  getPersonalizedPlan() {
    if (!this.userId) {
      this.planId = this.personalizedPlanService.getPersonalizedPlan();
      this.getTopics();
    }
    else {
      this.personalizedPlanService.getUserPlanId(this.userId)
        .subscribe(response => {
          if (response != undefined) {
            response.forEach(property => {
              if (property.planId) {
                this.planId = property.id;
              }
            });
            this.getTopics();
          }
        });
    }
  }

  ngOnInit() {
    this.getPersonalizedPlan();
  }

}
