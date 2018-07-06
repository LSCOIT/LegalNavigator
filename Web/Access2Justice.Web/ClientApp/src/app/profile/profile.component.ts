import { Component, OnInit } from '@angular/core';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { PlanSteps } from '../guided-assistant/personalized-plan/personalized-plan';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  topics: string;
  planDetails: Array<PlanSteps> = [];
  activeActionPlan: string = '';
  //topic: string = '';
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', PageNumber: 0, Location: '', ResourceIds: '' };
  personalizedResources: any;
  isSavedResources: boolean = false;
  planId: string;
  userId: string= "User Id";

  constructor(private personalizedPlanService: PersonalizedPlanService) { }

  getTopics(): void {
    //this.planId = "bf8d7e7e-2574-7b39-efc7-83cb94adae07";
    if (this.planId) {
      this.personalizedPlanService.getActionPlanConditions(this.planId)
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
        //      console.log("plan Details:");
        //      console.log(this.planDetails);
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
  }

  getpersonalizedResources() {
    this.personalizedPlanService.getPersonalizedResources(this.resourceFilter)
      .subscribe(response => {
        if (response != undefined) {
          this.personalizedResources = response;
          this.isSavedResources = true;
        }
      });
  }

  getPersonalizedPlan() {
    if (!this.userId) {
      this.planId = this.personalizedPlanService.getPersonalizedPlan();
      this.getTopics();
    }
    else {
      this.personalizedPlanService.getUserPlanId(this.userId)
        .subscribe(response => {
          if (response != undefined) {
            this.planId = response[1].id;
            this.getTopics();
          }
        });
    }
  }

  //filterSelectedResource(topic): void {
  //  this.getTopics(topic);
  //}

  ngOnInit() {
    this.getPersonalizedPlan();
    //this.getTopics(this.topic);
  }

}
