import {Component, OnDestroy, OnInit, Optional} from '@angular/core';
import { TabDirective } from "ngx-bootstrap";

import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { Subscription, Observable, forkJoin } from "rxjs";
import { EventUtilityService } from "../../common/services/event-utility.service";

@Component({
  selector: 'app-incoming-resources',
  templateUrl: './incoming-resources.component.html',
  styleUrls: ['./incoming-resources.component.scss']
})
export class IncomingResourcesComponent implements OnInit, OnDestroy {

  public incomingResources = [];
  public planDetails = [];
  public planIds = [];
  private incomingResourcesIds = [];
  private eventsSub: Subscription;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    @Optional() private tab: TabDirective,
    private eventUtilityService: EventUtilityService
  ) { }

  private getIncomingResources(): void {
    this.personalizedPlanService.getUserSavedResources('incoming-resources').
                        subscribe(incResIds => { this.incomingResourcesIds = incResIds;});

    this.personalizedPlanService.getPersonalizedResources('incoming-resources').subscribe(incomingResources => {
      this.incomingResources = [];
      const planDetailTags = {
        topics: [],
        id: ''
      };

      for (const key in incomingResources) {
        incomingResources[key].forEach(i => {
         
          const resource = i;
          if(resource.topicIds){
            this.planIds.push(resource);
          } 
          else
          {
            resource.shared = this.incomingResourcesIds.find(o => o.itemId === i.id);
            this.incomingResources.push(resource);
          }
        });
      }

      //get plan Details
      if(incomingResources.plans){
        
        let observables: Observable<any>[] = [];
        for (let h = 0; h < incomingResources.plans.length; h++) {
          observables.push(this.personalizedPlanService.getActionPlanConditions(incomingResources.plans[h].id))
        }
        
        forkJoin(observables)
       .subscribe(dataArray => {
          
          for(let g =0;g<dataArray.length;g++){
            debugger;
            var test = this.incomingResourcesIds.find(x=>x.plan && x.plan.id === dataArray[g].id);
            for(let f = 0; f < dataArray[g].topics.length;f++){
              dataArray[g].topics[f].shared = {sharedBy: test.sharedBy, itemId: test.itemId};
            }
            var fffftopic = dataArray[g].topics;
            var fff = fffftopic.filter(x=>incomingResources.plans[g].topicIds.includes(x.topicId));
            planDetailTags.topics = planDetailTags.topics.concat(fff);
          }
          planDetailTags.id = this.personalizedPlanService.planDetails.id;

          this.planDetails = this.personalizedPlanService.getPlanDetails(
              planDetailTags.topics, planDetailTags
            );
        }); 
      }
    });
  }

  ngOnInit() {
    debugger;
    if (this.tab) {
      this.tab.select.subscribe(() => this.getIncomingResources());
    } else {
      this.getIncomingResources();
    }

    this.eventsSub = this.eventUtilityService.sharedResourceUpdated$.subscribe(() => this.getIncomingResources());
  }

  ngOnDestroy() {
    if (this.eventsSub) {
      this.eventsSub.unsubscribe();
    }
  }

}
