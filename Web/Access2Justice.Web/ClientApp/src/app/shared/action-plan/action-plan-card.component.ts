import { Component, Input } from '@angular/core';
import { PersonalizedPlanCondition, Resources, PlanSteps } from '../../profile/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../profile/personalized-plan/personalized-plan.service';
import { ActivatedRoute } from '@angular/router';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
  selector: 'app-action-plan-card',
  templateUrl: './action-plan-card.component.html',
  styleUrls: ['./action-plan-card.component.css']
})
export class ActionPlanCardComponent implements OnChanges {
  @Input() planDetails;
  topics: any = [];
  tmptopics: any = [];
  steps: any;
  planSteps: Array<PlanSteps>;
  planStep: PlanSteps = { topicId: '', steps: [] };
  displaySteps: boolean = false;

  constructor(private personalizedPlanService: PersonalizedPlanService) { }

  getConditions(): void {
    if (this.planDetails.length === 0) {
      this.planSteps = null;
      this.displaySteps = false;
    }
    else {
      this.planSteps = [];
      if (this.planDetails.topicTags) {
        this.planDetails.topicTags.forEach(item => {
          this.planStep = { topicId: '', steps: [] };
          this.planStep.topicId = item.id[0].name;
          this.planStep.steps = item.stepTags;
          this.planSteps.push(this.planStep);
          this.displaySteps = true;
        });
      }
      else {
        this.planStep = { topicId: '', steps: [] };
        this.planStep.topicId = this.planDetails.id[0].name;
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

