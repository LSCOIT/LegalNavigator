import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { MsalService } from '@azure/msal-angular';
import { NgxSpinnerService } from 'ngx-spinner';

import { ENV } from 'environment';
import { Global } from '../global';
import { PersonalizedPlan, PersonalizedPlanTopic } from '../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
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

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
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
    this.filterPlan('');
  }

  getPersonalizedPlan() {
    this.personalizedPlanService.getPersonalizedPlan().subscribe(plan => {
      if (plan) {
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
