import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { PlanSteps, PlanDetailTags, PersonalizedPlanTopic } from '../guided-assistant/personalized-plan/personalized-plan';
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
  planDetailTags: PlanDetailTags = { id: '', oId: '', planTags: [{}], type: '' };
  tempPlanDetailTags: PlanDetailTags = { id: '', oId: '', planTags: [{}], type: '' };
  topicsList: Array<PersonalizedPlanTopic> = [];
  tempTopicsList: Array<PersonalizedPlanTopic> = [];
  planTopic: PersonalizedPlanTopic = { topic: {}, isSelected: true };
  isFiltered: boolean = false;

  constructor(private personalizedPlanService: PersonalizedPlanService) { }

  getTopics(): void {
    if (this.planId) {
      this.personalizedPlanService.getActionPlanConditions(this.planId)
        .subscribe(plan => {
          if (plan) {
            this.topics = plan.planTags;
            this.planDetailTags = plan;
          }
          this.getPlanDetails();
        });
    }
  }
  
  getPlanDetails() {
    this.topicsList = this.personalizedPlanService.createTopicsList(this.topics);
    this.planDetails = this.personalizedPlanService.displayPlanDetails(this.planDetailTags, this.topicsList);
  }

  filterPlan(topic) {
    this.tempTopicsList = this.topicsList;
    this.filterTopicsList(topic);
    this.planDetails = this.personalizedPlanService.displayPlanDetails(this.planDetailTags, this.topicsList);
  }

  filterTopicsList(topic) {
    this.topicsList = [];
    this.tempTopicsList.forEach(topicDetail => {
      this.planTopic = { topic: {}, isSelected: true };
      if (topicDetail.topic.id[0].name === topic) {
        this.planTopic = { topic: topicDetail.topic, isSelected: !topicDetail.isSelected };
      } else {
        this.planTopic = { topic: topicDetail.topic, isSelected: topicDetail.isSelected };
      }
      this.topicsList.push(this.planTopic);
    });
  }

  getpersonalizedResources() {
    this.personalizedPlanService.getPersonalizedResources(this.resourceFilter)
      .subscribe(response => {
        if (response) {
          this.personalizedResources = response;
          this.isSavedResources = true;
        }
      });
  }

  getPersonalizedPlan() {
    if (!this.userId) {
      this.planId = this.personalizedPlanService.getPersonalizedPlan();
      this.getTopics();
    } else {
      this.personalizedPlanService.getUserPlanId(this.userId)
        .subscribe(response => {
          if (response) {
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
