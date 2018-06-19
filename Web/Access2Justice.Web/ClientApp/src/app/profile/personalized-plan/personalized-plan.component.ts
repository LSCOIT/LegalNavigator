import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from './personalized-plan.service';
import { PersonalizedPlanCondition } from './personalized-plan';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-personalized-plan',
  templateUrl: './personalized-plan.component.html',
  styleUrls: ['./personalized-plan.component.css']
})
export class PersonalizedPlanComponent implements OnInit {
  conditionsList: Array<PersonalizedPlanCondition>[];
  activeActionPlan = this.activeRoute.snapshot.params['id'];
  constructor(private personalizedPlanService: PersonalizedPlanService, private activeRoute: ActivatedRoute) { }

  getConditions(): void {
    this.personalizedPlanService.getActionPlanConditions(this.activeActionPlan)
      .subscribe(conditions => {
        this.conditionsList = conditions[0].conditions;
      });
  }

  ngOnInit() {
    this.getConditions();
  }

}
