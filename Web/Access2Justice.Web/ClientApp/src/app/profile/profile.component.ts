import { Component, OnInit, OnDestroy } from '@angular/core';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { PersonalizedPlanTopic } from '../guided-assistant/personalized-plan/personalized-plan';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';
import { EventUtilityService } from '../shared/event-utility.service';
import { HttpParams } from '@angular/common/http';
import { Global } from '../global';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {
    
  topics: string;
  planDetails: any = [];
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: [], PageNumber: 0, Location: '', ResourceIds: [], IsResourceCountRequired: false };
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
  profileData: any;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private eventUtilityService: EventUtilityService,
    private global: Global,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {

    eventUtilityService.resourceUpdated$.subscribe(response => {
      if (response) {
        this.getpersonalizedResources();
      }
    });

    this.profileData = sessionStorage.getItem("profileData");
    if (this.profileData != undefined) {
      this.profileData = JSON.parse(this.profileData);
      this.userId = this.profileData["UserId"];
      this.userName = this.profileData["UserName"];
    }
  }

  getTopics(): void {
    if (this.planId) {
      this.spinner.show();
      this.personalizedPlanService.getActionPlanConditions(this.planId)
        .subscribe(plan => {
          this.spinner.hide();
          if (plan) {
            this.topics = plan.topics;
            this.planDetailTags = plan;
          }
          this.topicsList = this.personalizedPlanService.createTopicsList(this.topics);
          this.planDetails = this.personalizedPlanService.getPlanDetails(this.topics, this.planDetailTags);
        }, error => {
          this.spinner.hide();
          this.router.navigate(['/error']);
        });
    }
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

  filterTopics(event) {
    this.topics = event.plan.topics;
    this.planDetailTags = event.plan;
    this.topicsList = event.topicsList;
    this.filterPlan("");
  }

  getpersonalizedResources() {
    this.topicIds = [];
    this.resourceIds = [];
    this.webResources = [];
    let params = new HttpParams()
      .set("oid", this.userId)
      .set("type", "resources");
    this.personalizedPlanService.getUserSavedResources(params)
      .subscribe(response => {
        if (response != undefined) {
          response.forEach(property => {
            if (property.resources != undefined) {
              property.resources.forEach(resource => {
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
    let params = new HttpParams()
      .set("oid", this.userId)
      .set("type", "plan");
    this.personalizedPlanService.getUserSavedResources(params)
      .subscribe(response => {
        if (response) {
          this.planId = response[0].id;
        }
        this.getTopics();
      });
  }

  ngOnInit() {
    if (this.profileData == undefined) {
      this.global.externalLogin();
    }

    this.getPersonalizedPlan();
    this.showRemove = true;
  }

  ngOnDestroy(): void {
    if (this.profileData["IsShared"]) {
      sessionStorage.removeItem("profileData");
    }
  }
}
