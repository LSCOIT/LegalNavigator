import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { PersonalizedPlanTopic } from '../guided-assistant/personalized-plan/personalized-plan';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';
import { EventUtilityService } from '../shared/event-utility.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  topics: string;
  planDetails: any = [];
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', PageNumber: 0, Location: '', ResourceIds: '' };
  personalizedResources: { resources: any, topics: any, webResources: any };
  isSavedResources: boolean = false;
  planId: string;
  planDetailTags: any;
  topicsList: Array<PersonalizedPlanTopic> = [];
  tempTopicsList: Array<PersonalizedPlanTopic> = [];
  planTopic: PersonalizedPlanTopic = { topic: {}, isSelected: true };
  userId: string;
  userName: string;
  topicIds: string[] = [];
  resourceIds: string[] = [];
  webResources: any[] = [];
  showRemove: boolean;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private eventUtilityService: EventUtilityService
  ) {

    eventUtilityService.resourceUpdated$.subscribe(response => {
      if (response) {
        this.getpersonalizedResources();
      }
    });

    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      this.userId = profileData["UserId"];
      this.userName = profileData["UserName"];
    }
  }

  getTopics(): void {
    if (this.planId) {
      this.personalizedPlanService.getActionPlanConditions(this.planId)
        .subscribe(plan => {
          if (plan) {
            this.topics = plan.topics;
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
      if (topicDetail.topic.name === topic) {
        this.planTopic = { topic: topicDetail.topic, isSelected: !topicDetail.isSelected };
      } else {
        this.planTopic = { topic: topicDetail.topic, isSelected: topicDetail.isSelected };
      }
      this.topicsList.push(this.planTopic);
    });
  }

  getpersonalizedResources() {
    this.topicIds = [];
    this.resourceIds = [];
    this.webResources = [];
    this.personalizedPlanService.getUserSavedResources(this.userId)
      .subscribe(response => {
        if (response != undefined) {
          response.forEach(property => {
            if (property.resourceTags != undefined) {
              property.resourceTags.forEach(resource => {
                if (resource.resourceType === "Topics") {
                  this.topicIds.push(resource.itemId);
                } else if (resource.resourceType === "WebResources") {
                  this.webResources.push(resource);
                } else {
                  this.resourceIds.push(resource.itemId);
                }
              });
              this.resourceFilter = { TopicIds: this.topicIds, ResourceIds: this.resourceIds, ResourceType: 'ALL', PageNumber: 0, ContinuationToken: null, Location: null, IsResourceCountRequired: false };
              this.getSavedResource(this.resourceFilter);
            }
          });
        }
      });
  }

  getSavedResource(resourceFilter: IResourceFilter) {
    this.personalizedResources = { resources: [], topics: [], webResources: this.webResources };
    this.personalizedPlanService.getPersonalizedResources(resourceFilter)
      .subscribe(response => {
        if (response != undefined) {
          this.personalizedResources = { resources: response["resources"], topics: response["topics"], webResources: this.webResources };
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
          if (response.personalizedActionPlanId) {
            this.planId = response.personalizedActionPlanId;
          }
          this.getTopics();
        });
    }
  }

  ngOnInit() {
    this.getPersonalizedPlan();
    this.showRemove = true;
  }
}
