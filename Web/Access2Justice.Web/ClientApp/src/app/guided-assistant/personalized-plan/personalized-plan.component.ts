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
  planDetailTags: PlanDetailTags = { id: '', oId: '', topics: [{}], type: '' };
  isFiltered: boolean = false;
  type: string = "Plan";

  constructor(private personalizedPlanService: PersonalizedPlanService,
    private activeRoute: ActivatedRoute) { }

  getTopics(): void {
    if (!this.isFiltered) {
      this.personalizedPlanService.getActionPlanConditions(this.activeActionPlan)
        .subscribe(plan => {
          if (plan) {
            this.topics = plan.topics; //plan.planTags;
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

  ngOnInit() {
    this.getTopics(); 
  }

}
