import { Component, OnDestroy, OnInit, Optional } from "@angular/core";
import { TabDirective } from "ngx-bootstrap";

import { PersonalizedPlanService } from "../../guided-assistant/personalized-plan/personalized-plan.service";
import { Subscription, Observable, forkJoin } from "rxjs";
import { EventUtilityService } from "../../common/services/event-utility.service";
import { GuidUtilityService } from "../../common/services/guid-utility.service";

@Component({
  selector: "app-shared-resources",
  templateUrl: "./shared-resources.component.html",
  styleUrls: ["./shared-resources.component.scss"],
})
export class SharedResourcesComponent implements OnInit, OnDestroy {
  public sharedResources = [];
  public planDetails = [];
  public planIds = [];
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
      .subscribe((sharedResources) => {
        this.sharedResourcesIds = sharedResources;
      });
    this.personalizedPlanService
      .getPersonalizedResources("shared-resources")
      .subscribe((sharedResources) => {
        this.sharedResources = [];
        const planDetailTags = {
          topics: [],
          id: "",
        };
        for (const key in sharedResources) {
          sharedResources[key].forEach((i) => {
            const resource = i;
            resource.shared = this.sharedResourcesIds.find((o) => {
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
            if (resource.topicIds) {
              this.planIds.push(resource);
            } else {
              this.sharedResources.push(resource);
            }
          });
        }

        if (sharedResources.plans.length > 0) {
          let observables: Observable<any>[] = [];
          for (let h = 0; h < sharedResources.plans.length; h++) {
            observables.push(
              this.personalizedPlanService.getActionPlanConditions(
                sharedResources.plans[h].id
              )
            );
          }

          forkJoin(observables).subscribe((dataArray) => {
            for (let g = 0; g < dataArray.length; g++) {
              var test = this.sharedResourcesIds.find(
                (x) => x.plan && x.plan.id === dataArray[g].id
              );
              for (let f = 0; f < dataArray[g].topics.length; f++) {
                dataArray[g].topics[f].shared = {
                  sharedTo: test.sharedTo,
                  itemId: test.itemId,
                };
              }
              var nonfilteredTopics = dataArray[g].topics;
              var resTopics = nonfilteredTopics.filter((x) =>
                sharedResources.plans[g].topicIds.includes(x.topicId)
              );
              planDetailTags.topics = planDetailTags.topics.concat(resTopics);
            }
            planDetailTags.id = this.personalizedPlanService.planDetails.id;

            this.planDetails = this.personalizedPlanService.getPlanDetails(
              planDetailTags.topics,
              planDetailTags
            );
          });
        } else {
          this.planDetails = [];
        }
      });
  }

  ngOnInit() {
    if (this.tab) {
      this.tab.select.subscribe(() => this.getSharedResources());
    } else {
      this.getSharedResources();
    }

    this.eventsSub = this.eventUtilityService.sharedResourceUpdated$.subscribe(
      () => this.getSharedResources()
    );
  }

  ngOnDestroy() {
    if (this.eventsSub) {
      this.eventsSub.unsubscribe();
    }
  }
}
