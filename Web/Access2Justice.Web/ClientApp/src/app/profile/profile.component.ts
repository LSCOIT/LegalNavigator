import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { map } from "rxjs/operators";
import { MsalService } from "@azure/msal-angular";
import { NgxSpinnerService } from "ngx-spinner";

import { ENV } from "environment";
import { Global } from "../global";
import {
  PersonalizedPlan,
  PersonalizedPlanTopic,
} from "../guided-assistant/personalized-plan/personalized-plan";
import { PersonalizedPlanService } from "../guided-assistant/personalized-plan/personalized-plan.service";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.css"],
})
export class ProfileComponent implements OnInit {
  topics: any;
  planDetails: any = [];
  planId: string;
  plan: PersonalizedPlan;
  planDetailTags: any;
  topicsList: Array<PersonalizedPlanTopic> = [];
  tempTopicsList: Array<PersonalizedPlanTopic> = [];
  planTopic: PersonalizedPlanTopic;
  userId: string;
  userName: string;
  sharedResourceId: string;
  type = "Plan";

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private global: Global,
    private spinner: NgxSpinnerService,
    private router: Router,
    private msalService: MsalService,
    private route: ActivatedRoute
  ) {
    if (this.msalService.getUser()) {
      this.route.data.pipe(map((data) => data.cres)).subscribe((response) => {
        if (response) {
          this.userId = response.oId;
          this.sharedResourceId = response.sharedResourceId;
          this.global.setProfileData(
            response.oId,
            response.name,
            response.eMail,
            response.roleInformation
          );
          this.getPersonalizedPlan();
        }
      });
    }
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
    this.tempTopicsList.forEach((topicDetail) => {
      if (topicDetail.topic.name === topic) {
        this.planTopic = {
          topic: topicDetail.topic,
          isSelected: !topicDetail.isSelected,
        };
      } else {
        this.planTopic = {
          topic: topicDetail.topic,
          isSelected: topicDetail.isSelected,
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
  handleClick($event) {
    if ($event.target.innerText == "My Plan") {
      this.personalizedPlanService.planDetails = this.planDetailTags;
      this.personalizedPlanService.topicsList = this.topicsList;
    }
  }
  getPersonalizedPlan() {
    this.personalizedPlanService.getPersonalizedPlan().subscribe((plan) => {
      if (plan) {
        //iterate each description, find <a>, insert target="_blank" there
        var topics = plan.topics;
        topics.forEach((topic) => {
          var steps = topic.steps;
          var position = 0;
          steps.forEach((step) => {
            position = step.description.indexOf("<a");
            while (position != -1) {
              position += 2;
              step.description =
                step.description.substring(0, position) +
                " target='_blank'" +
                step.description.substring(position);
              position = step.description.indexOf("<a", position);
            }
          });
        });
        this.planId = plan.id;
        this.plan = plan;
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
    }
  }
}
