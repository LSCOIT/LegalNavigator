import { Component, AfterViewInit , ViewChild } from '@angular/core';
import { PersonalizedPlanService } from '../../profile/personalized-plan/personalized-plan.service';
import { PersonalizedPlanCondition, Resources, PlanSteps } from '../../profile/personalized-plan/personalized-plan';
import { ActivatedRoute } from '@angular/router';
import { ActionPlanCardComponent } from '../../shared/action-plan/action-plan-card.component';

@Component({
  selector: 'app-personalized-plan',
  templateUrl: './personalized-plan.component.html',
  styleUrls: ['./personalized-plan.component.css']
})
export class PersonalizedPlanComponent implements AfterViewInit  {
  activeActionPlan = 'bf8d7e7e-2574-7b39-efc7-83cb94adae07';//this.activeRoute.snapshot.params['id'];
  topics: any = [];
  tmptopics: any = [];

  constructor(private activeRoute: ActivatedRoute) { }
  @ViewChild(ActionPlanCardComponent) actionPlanTopics;

  filterSelectedResource() {

  }

  ngAfterViewInit() {
    this.tmptopics = this.actionPlanTopics.planSteps;
    this.tmptopics.forEach(item => {
      this.topics.push(this.actionPlanTopics.planSteps.topicId);
    });
    console.log(this.topics);
  }

}
