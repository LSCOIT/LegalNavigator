import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { PlanSteps } from '../guided-assistant/personalized-plan/personalized-plan';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';
import { environment } from '../../environments/environment';
import { UpperNavService } from '../shared/navigation/upper-nav.service';

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
  personalizedResources: { resources: any, topics: any, webResources: any };
  isSavedResources: boolean = false;
  planId: string;
  userId: string;
  userName: string;
  topicIds: string[] = [];
  resourceIds: string[] = [];
  webResources: any[] = [];

  constructor(private personalizedPlanService: PersonalizedPlanService, private upperNavService: UpperNavService) {
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
            this.topics = plan.planTags;
            this.planDetails = plan;
          }
        });
    }
  }

  getpersonalizedResources() {
    this.topicIds = [];
    this.resourceIds = [];
    this.personalizedPlanService.getUserPlanId(this.userId)
      .subscribe(response => {
        if (response != undefined) {
          response.forEach(property => {
            if (property.resourceTags != undefined) {
              property.resourceTags.forEach(resource => {
                if (resource.resourceType == "Topics") {
                  this.topicIds.push(resource.itemId);
                } else if (resource.resourceType == "WebResources") {
                  this.webResources.push(resource);
                }
                else {
                  this.resourceIds.push(resource.itemId);
                }
              });
              this.resourceFilter = { TopicIds: this.topicIds, ResourceIds: this.resourceIds, ResourceType: 'ALL', PageNumber: 0, ContinuationToken: null, Location: null };
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
