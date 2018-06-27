import { Component, OnInit, Input } from '@angular/core';
import { PersonalizedPlanService } from '../profile/personalized-plan/personalized-plan.service';
import { PersonalizedPlanCondition, Resources, PlanSteps } from '../profile/personalized-plan/personalized-plan';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  resources: Resources;
  topics: string;
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', PageNumber: 0, Location: '', ResourceIds: '' };
  personalizedResources: any;
  isSavedResources: boolean = false; 

  constructor(private personalizedPlanService: PersonalizedPlanService) { }

  getSavedResources(): void {
    this.resources = JSON.parse(sessionStorage.getItem("bookmarkedResource"));

    this.personalizedPlanService.getActionPlanConditions("bf8d7e7e-2574-7b39-efc7-83cb94adae07")//Read data from session
      .subscribe(items => {
        if (items) {
          this.topics = items.topicTags;
        }
      });
  }

  getpersonalizedResources(event) {
    this.personalizedPlanService.getPersonalizedResources(this.resourceFilter)
      .subscribe(response => {
        if (response != undefined) {
          this.personalizedResources = response;
          this.isSavedResources = true;
        }
      });
  } 

  ngOnInit() {
    this.getSavedResources();
  }

}
