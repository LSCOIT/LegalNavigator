import { Component, OnDestroy, OnInit, Optional } from "@angular/core";
import { TabDirective } from "ngx-bootstrap";

import { PersonalizedPlanService } from "../../guided-assistant/personalized-plan/personalized-plan.service";
import { Subscription } from "rxjs";
import { EventUtilityService } from "../../common/services/event-utility.service";
import { GuidUtilityService } from "../../common/services/guid-utility.service";

@Component({
  selector: "app-shared-resources",
  templateUrl: "./shared-resources.component.html",
  styleUrls: ["./shared-resources.component.scss"]
})
export class SharedResourcesComponent implements OnInit, OnDestroy {
  public sharedResources = [];
  public planDetails = [];
  private sharedResourcesIds = [];
  private eventsSub: Subscription;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private guidUtilityService: GuidUtilityService,
    @Optional() private tab: TabDirective,
    private eventUtilityService: EventUtilityService
  ) {}

  private getSharedResources(): void {
    this.personalizedPlanService
      .getUserSavedResources("shared-resources")
      .subscribe(sharedResources => {
        this.sharedResourcesIds = sharedResources;
      });
    this.personalizedPlanService
      .getPersonalizedResources("shared-resources")
      .subscribe(sharedResources => {
       
        this.sharedResources = [];
        const planDetailTags = {
          topics: []
        };

        for (const key in sharedResources) {
          sharedResources[key].forEach(i => {
            const isSharedPlan = i.topics;
            const resource = isSharedPlan ? i.topics[0] : i;
            resource.shared = this.sharedResourcesIds.find(o => {
              if (o.itemId) {
                return o.itemId === i.id;
              } else if (o.id) {
                return o.id === i.id;
              } else if (o.url) {
                return (
                  this.guidUtilityService.getGuidFromResourceUrl(o.url) === i.id
                );
              }
            });

            if (resource.shared && resource.shared.isShared === true) {
              resource.topicId = this.guidUtilityService.getGuidFromResourceUrl(
                resource.shared.url
              );
            }

            if (isSharedPlan) {
              planDetailTags.topics.push(resource);
            } else {
              this.sharedResources.push(resource);
            }
          });
        }

        // this.planDetails = this.personalizedPlanService.getPlanDetails(
        //   planDetailTags.topics,
        //   planDetailTags
        // );
      });
  }

  ngOnInit() {
    debugger;
    if (this.tab) {
      this.tab.select.subscribe(() => this.getSharedResources());
    } else {
      this.getSharedResources();
    }

    //  this.eventsSub = this.eventUtilityService.sharedResourceUpdated$.subscribe(() => this.getSharedResources());
  }

  ngOnDestroy() {
    if (this.eventsSub) {
      this.eventsSub.unsubscribe();
    }
  }
}
