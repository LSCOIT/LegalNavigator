import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PersonalizedPlanService } from './personalized-plan.service';
import { PlanSteps } from './personalized-plan';

@Component({
  selector: 'app-personalized-plan',
  templateUrl: './personalized-plan.component.html',
  styleUrls: ['./personalized-plan.component.css']
})
export class PersonalizedPlanComponent implements OnInit {
  activeActionPlan = this.activeRoute.snapshot.params['id'];
  topics: string;
  planDetails: Array<PlanSteps> = [];
  topic: string = '';

  constructor(private personalizedPlanService: PersonalizedPlanService,
    private activeRoute: ActivatedRoute) { }

  getTopics(): void {
    this.personalizedPlanService.getActionPlanConditions(this.activeActionPlan)
      .subscribe(plan => {
        if (plan) {
          this.topics = plan.planTags;
          this.planDetails = plan;
        }
      });
  }

  ngOnInit() {
    this.getTopics();
  }

}
