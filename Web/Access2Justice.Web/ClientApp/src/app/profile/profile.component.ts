import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../profile/personalized-plan/personalized-plan.service';
import { PersonalizedPlanCondition, Resources, PlanSteps } from '../profile/personalized-plan/personalized-plan';
import { NavigateDataService } from '../shared/navigate-data.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  resources: Resources;
  topics: string;
  planDetails: Array<PlanSteps> = [];
  activeActionPlan: string = '';
  topic: string = '';

  constructor(private personalizedPlanService: PersonalizedPlanService,
    private navigateDataService: NavigateDataService) { }

  getSavedResources(): void {
    this.resources = JSON.parse(sessionStorage.getItem("bookmarkedResource"));

    this.personalizedPlanService.getActionPlanConditions("bf8d7e7e-2574-7b39-efc7-83cb94adae07")//Read data from session
      .subscribe(items => {
        if (items) {
          this.topics = items.topicTags;
          this.planDetails = items;
        }
      });
  }

  getTopics(topic): void {
    this.personalizedPlanService.getActionPlanConditions("bf8d7e7e-2574-7b39-efc7-83cb94adae07")
      .subscribe(items => {
        if (items) {
          this.topics = items.topicTags;
          if (topic === '') {
            this.planDetails = items;
          }
          else {
            let i = 0;
            this.planDetails = items;
            items.topicTags.forEach(item => {
              if (item.id[0].name === topic) {
                this.planDetails = items.topicTags[i];
              }
              i++;
            });
          }
        }
      });
  }

  filterSelectedResource(topic): void {
    this.getTopics(topic);
  }

  ngOnInit() {
    this.getTopics(this.topic);
  }

}
