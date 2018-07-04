import { Component, OnInit } from '@angular/core';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { PlanSteps } from '../guided-assistant/personalized-plan/personalized-plan';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  topics: string;
  planDetails: Array<PlanSteps> = [];
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', PageNumber: 0, Location: '', ResourceIds: '' };
  personalizedResources: any;
  isSavedResources: boolean = false;
  planId: string;

  constructor(private personalizedPlanService: PersonalizedPlanService) { }

  getTopics(): void {
    this.personalizedPlanService.getActionPlanConditions(this.planId)
      .subscribe(plan => {
        if (plan) {
          this.topics = plan.planTags;
          this.planDetails = plan;
        }
      });
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
    this.planId = this.personalizedPlanService.getPersonalizedPlan();
  }

  ngOnInit() {
    this.getPersonalizedPlan();
    this.getTopics();
  }

}
