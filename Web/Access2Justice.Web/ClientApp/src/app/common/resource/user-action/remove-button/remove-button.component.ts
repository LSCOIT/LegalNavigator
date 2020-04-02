import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';

import { Global, UserStatus } from '../../../../global';
import { ProfileResources, SavedResource } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { PersonalizedPlanComponent } from '../../../../guided-assistant/personalized-plan/personalized-plan.component';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { ProfileComponent } from '../../../../profile/profile.component';
import { EventUtilityService } from '../../../services/event-utility.service';
import { NavigateDataService } from '../../../services/navigate-data.service';
import {GuidUtilityService} from "../../../services/guid-utility.service";

@Component({
  selector: 'app-remove-button',
  templateUrl: './remove-button.component.html',
  styleUrls: ['./remove-button.component.css'],
  host: {
    '[class.app-resource-button]': 'true',
  }
})
export class RemoveButtonComponent implements AfterViewInit {
  @Input() resourceId;
  @Input() resourceType;
  @Input() personalizedResources;
  @Input() selectedPlanDetails;
  @Input() sharedResources;
  @Input() sharedToResources;
  @Input() isNeedToRemoveSharedPlan;
  @Input() isNeedRemoveIncomingPlan;
  @Input() isNeedRemoveIncomingResource;

  removeResource: SavedResource;
  profileResources: ProfileResources = {oId: '', resourceTags: [], type: ''};
  resourceDetails: any;
  topicIndex: number;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private profileComponent: ProfileComponent,
    private personalizedPlanComponent: PersonalizedPlanComponent,
    private router: Router,
    private eventUtilityService: EventUtilityService,
    private global: Global,
    private navigateDataService: NavigateDataService,
    private guidUtilityService: GuidUtilityService,
  ) {
    global.showRemove = !(global.role === UserStatus.Shared && location.pathname.indexOf(global.shareRouteUrl) >= 0);
  }

  removeSavedResources() {
    if (this.selectedPlanDetails && !this.isNeedToRemoveSharedPlan && !this.isNeedRemoveIncomingPlan) {
      this.removeSavedPlan();
    } else if (this.selectedPlanDetails && this.isNeedToRemoveSharedPlan && !this.isNeedRemoveIncomingPlan) {
      this.removeSharedPlan();
    } else if (this.selectedPlanDetails && !this.isNeedToRemoveSharedPlan && this.isNeedRemoveIncomingPlan) {
      this.removeIncomingPlan();
    }

    if (this.resourceId && this.sharedResources) {
      this.removeSharedResources();
    } else if (this.resourceId && this.sharedToResources) {
      this.removeSharedToResources();
    } else if (this.resourceId && !this.isNeedRemoveIncomingResource) {
      this.removeSavedResource();
    } else if (this.resourceId && this.isNeedRemoveIncomingResource) {
      this.removeIncomingResource();
    }
  }

  removeSavedPlan() {
    if (this.selectedPlanDetails.planDetails) {
      this.createRemovePlanTag();
      this.removePersonalizedPlan();
    }
  }

  createRemovePlanTag() {
    this.topicIndex = 0;
    this.selectedPlanDetails.planDetails.topics.forEach(topic => {
      if (topic.topicId === this.selectedPlanDetails.topic) {
        this.selectedPlanDetails.planDetails.topics.splice(this.topicIndex, 1);
      }
      this.topicIndex++;
    });
  }

  removePersonalizedPlan() {
    if (this.router.url.indexOf('/plan') !== -1) {
      this.navigateDataService.setData(this.selectedPlanDetails.planDetails);
      this.personalizedPlanComponent.getTopics();
      this.personalizedPlanService.showSuccess(
        'Removed from Plan successfully'
      );
    } else {
      const params = {
        personalizedPlan: this.selectedPlanDetails.planDetails,
        oId: this.global.userId
      };
      this.personalizedPlanService.userPlan(params).subscribe(response => {
        if (response) {
          this.profileComponent.getPersonalizedPlan();
          this.personalizedPlanService.showSuccess(
            'Removed from plan successfully'
          );
        }
      });
    }
  }

  removeSavedResource() {
    this.removeResource = {itemId: '', resourceType: '', resourceDetails: {}, url: ''};
    this.profileResources.resourceTags = [];
    if (this.personalizedResources.resources) {
      this.removeUserSavedResourceFromProfile(
        this.personalizedResources.resources,
        true
      );
    }
    if (this.personalizedResources.webResources) {
      this.removeUserSavedResourceFromProfile(
        this.personalizedResources.webResources,
        false
      );
    }
    if (this.personalizedResources.topics) {
      this.removeUserSavedResourceFromProfile(
        this.personalizedResources.topics,
        false
      );
    }
    this.saveResourceToProfile(this.profileResources.resourceTags);
  }

  removeUserSavedResourceFromProfile(savedResource, isResource) {
    savedResource.forEach(resource => {
      if (isResource) {
        if (
          resource.id !== this.resourceId &&
          resource.resourceType !== 'Topics' &&
          resource.resourceType !== 'WebResources'
        ) {
          this.removeUserSavedResource(resource);
        }
      } else {
        if (resource.id !== this.resourceId) {
          this.removeUserSavedResource(resource);
        }
      }
    });
  }

  removeUserSavedResource(resource) {
    if (resource.resourceDetails) {
      this.resourceDetails = resource.resourceDetails;
    } else {
      this.resourceDetails = {};
    }
    this.removeResource = {
      itemId: resource.id,
      resourceType: resource.resourceType,
      url: resource.url,
      resourceDetails: this.resourceDetails
    };
    this.profileResources.resourceTags.push(this.removeResource);
  }

  saveResourceToProfile(resourceTags) {
    this.profileResources = {
      oId: this.global.userId,
      resourceTags: resourceTags,
      type: 'resources'
    };
    this.personalizedPlanService
      .saveResources(this.profileResources)
      .subscribe(response => {
        if (response !== undefined) {
          this.personalizedPlanService.showSuccess(
            'Resource removed from profile'
          );
          this.eventUtilityService.updateSavedResource('RESOURCE REMOVED');
        }
      });
  }

  removeSharedResources() {
    const resource = this.personalizedResources.find(o => {
      return o.id === this.resourceId;
    });
    const removedElem = {
      Oid: this.global.userId,
      itemId: this.resourceId,
      resourceType: this.resourceType,
      sharedBy:
        (resource.shared && resource.shared.sharedBy) || "katana_2w@ukr.net",
      type: "shared-resources"
    };
    this.personalizedPlanService
      .removeSharedResources(removedElem)
      .subscribe(() => {
        this.personalizedPlanService.showSuccess(
          "Shared resource removed from profile"
        );
        this.eventUtilityService.updateSharedResource("RESOURCE REMOVED");
      });
  }

  removeSharedToResources() {
    const resource = this.personalizedResources.find(o => {
      return o.id === this.resourceId;
    });
    const removedElem = {
      Oid: this.global.userId,
      itemId: this.resourceId,
      resourceType: this.resourceType,
      sharedBy: (resource.shared && resource.shared.sharedBy) || 'katana_2w@ukr.net',
      type: 'shared-resources'
    };
    this.personalizedPlanService.removeSharedToResources(removedElem).subscribe(() => {
      this.personalizedPlanService.showSuccess(
        'Shared resource removed from profile'
      );
      this.eventUtilityService.updateSharedResource('RESOURCE REMOVED');
    });
  }

   ngAfterViewInit() {
    const element = document.getElementById("removeButtonForSharedToTab");
    if (element) {
      element.textContent = "Remove block";
    }
  }

  private removeSharedPlan() {
    const safe = this;
    const unsharePromise = new Promise(function(resolve, reject) {
      // Remove resource
      const sharedResource = safe.selectedPlanDetails.planDetails.topics.find(o => {
        return o.topicId === safe.selectedPlanDetails.topic;
      });
      const resourceId = safe.guidUtilityService.getGuidFromResourceUrl(sharedResource.shared.url);
      const removedElem = {
        Oid: safe.global.userId,
        itemId: resourceId,
        resourceType: "Plan",
        type: 'shared-resources'
      };
      safe.personalizedPlanService.removeSharedResources(removedElem).subscribe(() => {
        safe.personalizedPlanService.showSuccess(
          'Shared resource removed from profile'
        );
        safe.eventUtilityService.updateSharedResource('RESOURCE REMOVED');
      });

      resolve();
    });

    unsharePromise.then(() => {
      safe.selectedPlanDetails.planDetails.topics.forEach(topic => {
        if (topic.shared.sharedTo && topic.shared.sharedTo.length > 0) {
          topic.shared.sharedTo.forEach(email => {
            const id = safe.guidUtilityService.getGuidFromResourceUrl(topic.shared.url);
            const unsharedElem = {
              oId: safe.global.userId,
              email: email,
              itemId: id,
              resourceType: "Plan",
              resourceDetails: null
            };
            safe.personalizedPlanService.unshareSharedToResources(unsharedElem).subscribe(() => {
            });
          });
        }
      });
    });
  }

  removeIncomingPlan = () => {

    var item = this.selectedPlanDetails.planDetails.topics.filter(x=> x.topicId == this.selectedPlanDetails.topic);
    const resourceId =  item[0].shared.itemId;
    const sharedBy =  item[0].shared.sharedBy;
    const removedElem = {
      Oid: this.global.userId,
      itemId: resourceId,
      resourceType: "Plan",
      sharedBy: sharedBy || 'katana_2w@ukr.net',
      type: 'incoming-resources',
      topicId: this.selectedPlanDetails.topic
    };
    this.personalizedPlanService.removeSharedResources(removedElem).subscribe(() => {
      this.personalizedPlanService.showSuccess(
        'Incoming resource removed from profile'
      );
      this.eventUtilityService.updateSharedResource('RESOURCE REMOVED');
    });
  }

  private removeIncomingResource() {

   const resource = this.personalizedResources.find(o => {
      return o.id === this.resourceId;
    });
    const removedElem = {
      Oid: this.global.userId,
      itemId: this.resourceId,
      resourceType: this.resourceType,
      sharedBy: resource.shared.sharedBy || 'katana_2w@ukr.net',
      type: 'incoming-resources'
    };
    this.personalizedPlanService.removeSharedResources(removedElem).subscribe(() => {
      this.personalizedPlanService.showSuccess(
        'Incoming resource removed from profile'
      );
      this.eventUtilityService.updateSharedResource('RESOURCE REMOVED');
    });
  }
}
