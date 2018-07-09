import { Component, Input } from '@angular/core';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { PlanSteps } from '../../guided-assistant/personalized-plan/personalized-plan';

@Component({
  selector: 'app-action-plan-card',
  templateUrl: './action-plan-card.component.html',
  styleUrls: ['./action-plan-card.component.css']
})
export class ActionPlanCardComponent implements OnChanges {
  @Input() planDetails;
  planSteps: Array<PlanSteps>;
  planStep: PlanSteps = { topicId: '', topicName:'', steps: [] };
  displaySteps: boolean = false;

  constructor() { }

  getConditions(): void {
    if (this.planDetails.length === 0) {
      this.planSteps = null;
      this.displaySteps = false;
    }
    else {
      this.planSteps = [];
      if (this.planDetails.planTags) {
        this.planDetails.planTags.forEach(planTag => {
          this.planStep = { topicId: '', topicName: '', steps: [] };
          if (planTag.id[0]) {
            this.planStep.topicName = planTag.id[0].name;
            this.planStep.topicId = planTag.id[0].id;
          }
          this.planStep.steps = planTag.stepTags;
          this.planSteps.push(this.planStep);
          this.displaySteps = true;
        });
      }
      else {
        this.planStep = { topicId: '', topicName: '', steps: [] };
        this.planStep.topicName = this.planDetails.id[0].name;
        this.planStep.topicId = this.planDetails.id[0].id;
        this.planStep.steps = this.planDetails.stepTags;
        this.planSteps.push(this.planStep);
        this.displaySteps = true;
      }
    }
  }

  ngOnChanges() {
    this.getConditions();
  }

}

