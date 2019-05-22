import {Component, OnDestroy, OnInit, Optional} from '@angular/core';
import { TabDirective } from "ngx-bootstrap";

import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { Subscription } from "rxjs";
import { EventUtilityService } from "../../common/services/event-utility.service";

@Component({
  selector: 'app-shared-resources',
  templateUrl: './shared-resources.component.html',
  styleUrls: ['./shared-resources.component.scss']
})
export class SharedResourcesComponent implements OnInit, OnDestroy {
  public sharedResources = [];
  public planDetails = [];
  private sharedResourcesIds = [];
  private eventsSub: Subscription;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    @Optional() private tab: TabDirective,
    private eventUtilityService: EventUtilityService
  ) { }

  private getSharedResources(): void {
    this.personalizedPlanService.getUserSavedResources('shared-resources').subscribe(incResIds => { this.sharedResourcesIds = incResIds });
    this.personalizedPlanService.getPersonalizedResources('shared-resources').subscribe(sharedResources => {
      this.sharedResources = [];
      var planDetailTags = {
        topics: []
      };
      for (let key in sharedResources) {
        sharedResources[key].forEach(i => {
          var isSharedPlan = i.topics;
          var resource = isSharedPlan ? i.topics[0] : i;
          resource.shared = this.sharedResourcesIds.find(o => {
            if(o.itemId) {
              return o.itemId === i.id;
            } else if(o.id) {
              return o.id === i.id;
            } else if (o.url) {
              return o.url.substring(o.url.length - 36) === i.id;
            }
          });

          if (isSharedPlan) {
            planDetailTags.topics.push(resource);
          } else {
            this.sharedResources.push(resource);
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
      this.tab.select.subscribe(() => this.getSharedResources());
    } else {
      this.getSharedResources();
    }

    this.eventsSub = this.eventUtilityService.sharedResourceUpdated$.subscribe(() => this.getSharedResources());
  }

  ngOnDestroy() {
    if (this.eventsSub) {
      this.eventsSub.unsubscribe();
    }
  }

}
