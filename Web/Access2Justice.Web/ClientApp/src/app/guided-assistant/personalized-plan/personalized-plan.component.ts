import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../personalized-plan/personalized-plan.service';
import { PlanSteps } from '../personalized-plan/personalized-plan';
import { ActivatedRoute } from '@angular/router';

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

  getTopics(topic): void {
    this.personalizedPlanService.getActionPlanConditions(this.activeActionPlan)
      .subscribe(plan => {
        if (plan) {
          this.topics = plan.planTags;
          this.planDetails = plan;
        }
      });
      //.subscribe(items => {
      //  if (items) {
      //    this.topics = items.planTags;
      //    if (topic === '') {
      //      this.planDetails = items;
      //    }
      //    else {
      //      let i = 0;
      //      this.planDetails = items;
      //      items.planTags.forEach(item => {
      //        if (item.id[0].name === topic) {
      //          this.planDetails = items.planTags[i];
      //        }
      //        i++;
      //      });
      //    }
      //  }
      //});
  }

  //filterSelectedResource(topic): void {
  //  this.getTopics(topic);
  //}

  ngOnInit() {
    this.getTopics(this.topic);
  }

}
