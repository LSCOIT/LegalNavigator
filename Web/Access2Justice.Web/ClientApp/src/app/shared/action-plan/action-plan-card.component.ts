import { Component, OnInit, Input } from '@angular/core';
import { PersonalizedPlanCondition, Resources, PlanSteps } from '../../profile/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../profile/personalized-plan/personalized-plan.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-action-plan-card',
  templateUrl: './action-plan-card.component.html',
  styleUrls: ['./action-plan-card.component.css']
})
export class ActionPlanCardComponent implements OnInit {
  @Input() activeActionPlan;
  conditionsList: Array<PersonalizedPlanCondition>[];
  resources: Resources;
  steps: any;
  topicsSteps: any;
  planSteps: Array<PlanSteps> = [];
  planStep: PlanSteps = { topicId: '', steps: [] };

  constructor(private personalizedPlanService: PersonalizedPlanService, private activeRoute: ActivatedRoute) { }

  getConditions(): void {
    this.personalizedPlanService.getActionPlanConditions(this.activeActionPlan)
      .subscribe(steps => {
        if (steps) {
          this.steps = steps.topicTags;
          this.steps.forEach(item => {
            this.planStep = { topicId: '', steps: [] };
            this.planStep.topicId = item.id[0].name;
            this.planStep.steps = item.stepTags;
            this.topicsSteps = this.planStep.steps;
            this.planSteps.push(this.planStep);
          });
        }
        else {
          this.steps = null;
        }
      });
  }

  ngOnInit() {
    this.getConditions();
  }

}

