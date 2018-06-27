import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../../profile/personalized-plan/personalized-plan.service';
import { PersonalizedPlanCondition, Resources, PlanSteps } from '../../profile/personalized-plan/personalized-plan';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-personalized-plan',
  templateUrl: './personalized-plan.component.html',
  styleUrls: ['./personalized-plan.component.css']
})
export class PersonalizedPlanComponent implements OnInit {
  conditionsList: Array<PersonalizedPlanCondition>[];
  activeActionPlan = 'bf8d7e7e-2574-7b39-efc7-83cb94adae07';//this.activeRoute.snapshot.params['id'];
  resources: Resources;
  steps: any;
  planSteps: Array<PlanSteps> = [];// [{ topicId: '', steps: [] }];
  planStep: PlanSteps = { topicId: '', steps: [] };

  constructor(private personalizedPlanService: PersonalizedPlanService, private activeRoute: ActivatedRoute) { }

  getConditions(): void {
    this.personalizedPlanService.getActionPlanConditions(this.activeActionPlan)
      .subscribe(steps => {
        console.log(steps)
        if (steps) {
          this.steps = steps.topicTags;
          this.steps.forEach(item => {
            this.planStep.topicId = item.id[0].name;
            this.planStep.steps = item.stepTags;
            this.planSteps.push(this.planStep);
            console.log(this.planSteps);
          });

          //for (let i = 0; i < this.steps.length; i++) {
          //  this.planSteps[i].topicId = this.steps[i].id[0].name;
          //  this.planSteps[i].steps = this.steps[i].stepTags;
          //}
        }
        else {
          this.steps = null;
        }
        console.log(this.steps);
        console.log(this.planSteps);
      });
  }

  ngOnInit() {
    this.getConditions();
  }

}
