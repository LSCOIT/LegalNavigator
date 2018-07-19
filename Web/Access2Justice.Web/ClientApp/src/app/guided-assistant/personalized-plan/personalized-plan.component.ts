import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../personalized-plan/personalized-plan.service';
import { PlanSteps, PersonalizedPlanTopic, PlanDetailTags } from '../personalized-plan/personalized-plan';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-personalized-plan',
  templateUrl: './personalized-plan.component.html',
  styleUrls: ['./personalized-plan.component.css']
})
export class PersonalizedPlanComponent implements OnInit {
  activeActionPlan = this.activeRoute.snapshot.params['id'];
  topics: Array<any> = [];
  planTopic: PersonalizedPlanTopic = { topic: {}, isSelected: true };
  topicsList: Array<PersonalizedPlanTopic> = [];
  tempTopicsList: Array<PersonalizedPlanTopic> = [];
  planDetails: Array<PlanSteps> = [];
  planDetailTags: PlanDetailTags ={ id: '', oId: '', planTags: [{}], type:'' };
  tempPlanDetailTags: PlanDetailTags = { id: '', oId: '', planTags: [{}], type:'' };
  tempPlanDetails: any;

  constructor(private personalizedPlanService: PersonalizedPlanService,
    private activeRoute: ActivatedRoute) { }

  getTopics(): void {
    this.personalizedPlanService.getActionPlanConditions(this.activeActionPlan)
      .subscribe(plan => {
        if (plan) {
          this.topics = plan.planTags;
          this.planDetails = plan;
          this.planDetailTags = plan;
        }
        this.createTopicsList();
        this.displayPlanDetails();
      });
  }

  createTopicsList() {
    this.topicsList = [];
    this.topics.forEach(topic => {
      this.planTopic = { topic: topic, isSelected: true };
      this.topicsList.push(this.planTopic);
    });
  }

  displayPlanDetails() {
    this.tempPlanDetailTags = { id: '', oId: '', planTags: [{}], type: '' };
    this.tempPlanDetailTags = this.planDetailTags;
    this.planDetails = [];
    let index = 0;
    this.topicsList.forEach(topic => {
      if (!topic.isSelected) {
        if (index !== -1) {
          this.tempPlanDetailTags.planTags.splice(index++, 1);
        }
    console.log(this.planDetailTags);
      }
    });
    this.tempPlanDetails = this.tempPlanDetailTags;
    this.planDetails = this.tempPlanDetails;
    console.log(this.planDetails);
  }

  filterPlan(event, topic) {
    this.tempTopicsList = this.topicsList;
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
    this.displayPlanDetails();
  }

  ngOnInit() {
    this.getTopics();
  }

}
