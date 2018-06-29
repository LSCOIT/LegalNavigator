import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../profile/personalized-plan/personalized-plan.service';
import { PlanSteps } from '../profile/personalized-plan/personalized-plan';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  topics: string;
  planDetails: Array<PlanSteps> = [];
  activeActionPlan: string = '';
  topic: string = '';
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', PageNumber: 0, Location: '', ResourceIds: '' };
  personalizedResources: any;
  isSavedResources: boolean = false;
  planId: string;

  constructor(private personalizedPlanService: PersonalizedPlanService) { }

  getTopics(topic): void {
    if (this.planId) {
      this.personalizedPlanService.getActionPlanConditions(this.planId)
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

  filterSelectedResource(topic): void {
    this.getTopics(topic);
  }

  ngOnInit() {
    this.getPersonalizedPlan();
    this.getTopics(this.topic);
  }

}
