import { Component, Input, OnInit } from "@angular/core";
import { Router } from "@angular/router";

import { Global, UserStatus } from "../../../../global";
import { PersonalizedPlanComponent } from "../../../../guided-assistant/personalized-plan/personalized-plan.component";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { ProfileComponent } from "../../../../profile/profile.component";
import { EventUtilityService } from "../../../services/event-utility.service";
import { GuidUtilityService } from "../../../services/guid-utility.service";

@Component({
  selector: "app-unshare-button",
  templateUrl: "./unshare-button.component.html",
  styleUrls: ["./unshare-button.component.css"],
  host: {
    "[class.app-resource-button]": "true",
  },
})
export class UnshareButtonComponent implements OnInit {
  @Input() showIcon = true;
  @Input() type: string;
  @Input() url: string;
  @Input() webResourceUrl: string;
  @Input() resourceId;
  @Input() resourceType;
  @Input() personalizedResources;
  @Input() selectedPlanDetails;
  @Input() sharedResources;
  @Input() sharedToResources;
  @Input() email;

  resourceDetails: any;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private profileComponent: ProfileComponent,
    private personalizedPlanComponent: PersonalizedPlanComponent,
    private router: Router,
    private eventUtilityService: EventUtilityService,
    private global: Global,
    private guidUtilityService: GuidUtilityService
  ) {
    global.showRemove = !(
      global.role === UserStatus.Shared &&
      location.pathname.indexOf(global.shareRouteUrl) >= 0
    );
  }

  unshareSharedResources() {

    if (this.resourceId && this.sharedToResources) {
      this.unshareSharedToResources();
    } else if (this.selectedPlanDetails) {
      this.unawareSharedToPlan();
    }
  }

  unawareSharedToPlan() {
    debugger;
    const resource = this.selectedPlanDetails.planDetails.topics.find((o) => {
      return (
        o.topicId === this.selectedPlanDetails.topic &&
        o.shared.sharedTo &&
        o.shared.sharedTo.length > 0
      );
    });
    const resourceId = resource.shared.itemId;
    // const resourceId = this.guidUtilityService.getGuidFromResourceUrl(
    //   resource.shared.url
    // );
    const unsharedElem = {
      oId: this.global.userId,
      email: this.email,
      itemId: resourceId,
      resourceType: this.resourceType,
      topicId: this.selectedPlanDetails.topic,
      resourceDetails: null,
    };
    this.personalizedPlanService
      .unshareSharedToResources(unsharedElem)
      .subscribe(() => {
        this.personalizedPlanService.showSuccess(
          "Shared resource unshared from profile"
        );
        this.eventUtilityService.updateSharedResource("RESOURCE UNSHARED");
      });

    // const needRemoveAfterUnshared = resource.shared.sharedTo.length === 1;
    // if ( needRemoveAfterUnshared ) {
    //    // Remove resource
    //    const sharedResource = this.selectedPlanDetails.planDetails.topics.find(o => {
    //      return o.topicId === this.selectedPlanDetails.topic;
    //    });
    //
    //    const removedElem = {
    //      Oid: this.global.userId,
    //      itemId: resourceId,
    //      resourceType: "Plan",
    //      type: 'shared-resources'
    //    };
    //    this.personalizedPlanService.removeSharedResources(removedElem).subscribe(() => {
    //      this.personalizedPlanService.showSuccess(
    //        'Shared resource removed from profile'
    //      );
    //      this.eventUtilityService.updateSharedResource('RESOURCE REMOVED');
    //    });
    //  }
  }

  unshareSharedToResources() {
    debugger;
    const resource = this.personalizedResources.find((o) => {
      return o.id === this.resourceId;
    });
    const unsharedElem = {
      oId: this.global.userId,
      email: this.sharedToResources,
      itemId: this.resourceId,
      resourceType: this.resourceType,
      resourceDetails: null,
    };
    this.personalizedPlanService
      .unshareSharedToResources(unsharedElem)
      .subscribe(() => {
        this.personalizedPlanService.showSuccess(
          "Shared resource unshared from profile"
        );
        this.eventUtilityService.updateSharedResource("RESOURCE UNSHARED");
      });
  }

  ngOnInit() {}
}
