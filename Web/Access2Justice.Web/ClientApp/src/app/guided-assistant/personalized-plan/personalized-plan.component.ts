import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../personalized-plan/personalized-plan.service';
import { PersonalizedPlanTopic, PersonalizedPlan } from '../personalized-plan/personalized-plan';
import { ActivatedRoute } from '@angular/router';
import { NavigateDataService } from '../../shared/navigate-data.service';

@Component({
  selector: 'app-personalized-plan',
  templateUrl: './personalized-plan.component.html',
  styleUrls: ['./personalized-plan.component.css']
})
export class PersonalizedPlanComponent implements OnInit {
  activeActionPlan = this.activeRoute.snapshot.params['id'];
  personalizedPlan: PersonalizedPlan;

  topics: Array<any> = [];
  planTopic: PersonalizedPlanTopic;
  topicsList: Array<PersonalizedPlanTopic> = [];
  tempTopicsList: Array<PersonalizedPlanTopic> = [];
  planDetails: any = [];
  planDetailTags: any;
  type: string = "Plan";

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService
  ) { }

  getTopics(): void {
    this.personalizedPlan = this.navigateDataService.getData();
    if (this.personalizedPlan != undefined) {
      this.topics = this.personalizedPlan.topics;
      this.planDetailTags = this.personalizedPlan;
      this.topicsList = this.personalizedPlanService.createTopicsList(this.topics);
      this.planDetails = this.personalizedPlanService.getPlanDetails(this.topics, this.planDetailTags);
    } else {
      this.personalizedPlanService.getActionPlanConditions(this.activeActionPlan)
        .subscribe(plan => {
          if (plan) {
            this.topics = plan.topics;
            this.planDetailTags = plan;
          }
          this.topicsList = this.personalizedPlanService.createTopicsList(this.topics);
          this.planDetails = this.personalizedPlanService.getPlanDetails(this.topics, this.planDetailTags);
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

  ngOnInit() {
    this.getTopics();
  }
}
