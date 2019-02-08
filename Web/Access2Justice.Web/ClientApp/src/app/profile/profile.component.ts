import { HttpParams } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import {map} from 'rxjs/operators';
import { MsalService } from "@azure/msal-angular";
import { NgxSpinnerService } from "ngx-spinner";

import {ENV} from 'environment';
import { Global } from "../global";
import { PersonalizedPlan, PersonalizedPlanTopic } from "../guided-assistant/personalized-plan/personalized-plan";
import { PersonalizedPlanService } from "../guided-assistant/personalized-plan/personalized-plan.service";
import { IResourceFilter } from "../shared/search/search-results/search-results.model";
import { EventUtilityService } from "../shared/services/event-utility.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.css"]
})
export class ProfileComponent implements OnInit {
  topics: any;
  planDetails: any = [];
  resourceFilter: IResourceFilter = {
    ResourceType: "",
    ContinuationToken: "",
    TopicIds: [],
    PageNumber: 0,
    Location: "",
    ResourceIds: [],
    IsResourceCountRequired: false,
    IsOrder: false,
    OrderByField: "",
    OrderBy: ""
  };
  personalizedResources: {
    resources: any;
    topics: any;
    webResources: any;
  };
  isSavedResources = false;
  planId: string;
  plan: PersonalizedPlan;
  planDetailTags: any;
  topicsList: Array<PersonalizedPlanTopic> = [];
  tempTopicsList: Array<PersonalizedPlanTopic> = [];
  planTopic: PersonalizedPlanTopic;
  userId: string;
  userName: string;
  topicIds: string[] = [];
  resourceIds: string[] = [];
  webResources: any[] = [];
  showRemove: boolean;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private eventUtilityService: EventUtilityService,
    private global: Global,
    private spinner: NgxSpinnerService,
    private router: Router,
    private msalService: MsalService,
    private route: ActivatedRoute
  ) {
    if (this.msalService.getUser()) {
      this.route.data.pipe(map(data => data.cres)).subscribe(response => {
          if (response) {
            this.userId = response.oId;
            this.global.setProfileData(
              response.oId,
              response.name,
              response.eMail,
              response.roleInformation
            );
            this.getPersonalizedPlan();
            this.showRemove = true;
          }
      });
    }

    eventUtilityService.resourceUpdated$.subscribe(response => {
      if (response) {
        this.getpersonalizedResources();
      }
    });
  }

  getTopics(): void {
    this.spinner.show();
    if (this.plan) {
      this.topics = this.plan.topics;
      this.planDetailTags = this.plan;
      this.topicsList = this.personalizedPlanService.createTopicsList(
        this.topics
      );
      this.planDetails = this.personalizedPlanService.getPlanDetails(
        this.topics,
        this.planDetailTags
      );
    }
    this.spinner.hide();
  }

  filterPlan(topic) {
    this.tempTopicsList = this.topicsList;
    this.filterTopicsList(topic);
    this.planDetails = this.personalizedPlanService.displayPlanDetails(
      this.planDetailTags,
      this.topicsList
    );
  }

  filterTopicsList(topic) {
    this.topicsList = [];
    this.tempTopicsList.forEach(topicDetail => {
      if (topicDetail.topic.name === topic) {
        this.planTopic = {
          topic: topicDetail.topic,
          isSelected: !topicDetail.isSelected
        };
      } else {
        this.planTopic = {
          topic: topicDetail.topic,
          isSelected: topicDetail.isSelected
        };
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
    const params = new HttpParams()
      .set("oid", this.userId)
      .set("type", "resources");
    this.personalizedPlanService
      .getUserSavedResources(params)
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
              this.resourceFilter = {
                TopicIds: this.topicIds,
                ResourceIds: this.resourceIds,
                ResourceType: "ALL",
                PageNumber: 0,
                ContinuationToken: null,
                Location: null,
                IsResourceCountRequired: false,
                IsOrder: false,
                OrderByField: "",
                OrderBy: ""
              };
              this.getSavedResource(this.resourceFilter);
            }
          });
        }
      });
  }

  getSavedResource(resourceFilter: IResourceFilter) {
    this.personalizedResources = {
      resources: [],
      topics: [],
      webResources: this.webResources
    };
    this.personalizedPlanService
      .getPersonalizedResources(resourceFilter)
      .subscribe(response => {
        if (response != undefined) {
          this.personalizedResources = {
            resources: response["resources"],
            topics: response["topics"],
            webResources: this.webResources
          };
          this.isSavedResources = true;
        }
      });
  }

  getPersonalizedPlan() {
    let params = new HttpParams().set("oid", this.userId).set("type", "plan");
    this.personalizedPlanService
      .getUserSavedResources(params)
      .subscribe(response => {
        if (response) {
          this.planId = response[0].id;
          this.plan = response[0];
        }
        this.getTopics();
      });
  }

  ngOnInit() {
    if (!this.msalService.getUser() && !this.global.isShared) {
      this.msalService.loginRedirect(ENV.consentScopes);
    }
    if (this.global.isLoggedIn && !this.global.isShared) {
      this.userId = this.global.userId;
      this.userName = this.global.userName;
    } else if (this.global.isShared) {
      this.userId = this.global.sharedUserId;
      this.userName = this.global.sharedUserName;
      this.getPersonalizedPlan();
      this.showRemove = true;
    }
  }
}
