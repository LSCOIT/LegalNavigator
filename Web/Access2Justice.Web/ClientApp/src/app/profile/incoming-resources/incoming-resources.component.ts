import {Component, OnDestroy, OnInit, Optional} from '@angular/core';
import { TabDirective } from "ngx-bootstrap";

import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { Subscription } from "rxjs";
import { EventUtilityService } from "../../common/services/event-utility.service";

@Component({
  selector: 'app-incoming-resources',
  templateUrl: './incoming-resources.component.html',
  styleUrls: ['./incoming-resources.component.scss']
})
export class IncomingResourcesComponent implements OnInit, OnDestroy {

  public incomingResources = [];
  public planDetails = [];
  private incomingResourcesIds = [];
  private eventsSub: Subscription;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    @Optional() private tab: TabDirective,
    private eventUtilityService: EventUtilityService
  ) { }

  private getIncomingResources(): void {
    this.personalizedPlanService.getUserSavedResources('incoming-resources').
                        subscribe(incResIds => { this.incomingResourcesIds = incResIds});
    this.personalizedPlanService.getPersonalizedResources('incoming-resources').subscribe(incomingResources => {

      this.incomingResources = [];
      const planDetailTags = {
        topics: []
      };

      for (const key in incomingResources) {
        incomingResources[key].forEach(i => {
          const isSharedPlan = i.topics;
          const resource = isSharedPlan ? i.topics[0] : i;
          resource.shared = this.incomingResourcesIds.find(o => o.itemId === i.id);

          if (isSharedPlan) {
            planDetailTags.topics.push(resource);
          } else {
            this.incomingResources.push(resource);
          }
        });
      }

      this.planDetails = this.personalizedPlanService.getPlanDetails(
        planDetailTags.topics, planDetailTags
      );
    });
  }

  ngOnInit() {
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
