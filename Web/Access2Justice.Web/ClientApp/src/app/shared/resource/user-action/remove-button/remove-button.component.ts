import { Component, OnInit, Input } from '@angular/core';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { SavedResources, ProfileResources } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { ProfileComponent } from '../../../../profile/profile.component';
import { PersonalizedPlanComponent } from '../../../../guided-assistant/personalized-plan/personalized-plan.component';
import { Router } from '@angular/router';
import { EventUtilityService } from '../../../services/event-utility.service';
import { Global, UserStatus } from '../../../../global';
import { NavigateDataService } from '../../../services/navigate-data.service';

@Component({
  selector: 'app-remove-button',
  templateUrl: './remove-button.component.html',
  styleUrls: ['./remove-button.component.css']
})
export class RemoveButtonComponent implements OnInit {
  @Input() resourceId;
  @Input() resourceType;
  @Input() personalizedResources;
  @Input() selectedPlanDetails;
  removeResource: SavedResources;
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  isRemovedPlan: boolean = false;
  resourceDetails: any;
  topicIndex: number;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private profileComponent: ProfileComponent,
    private personalizedPlanComponent: PersonalizedPlanComponent,
    private router: Router,
    private eventUtilityService: EventUtilityService,
    private global: Global,
    private navigateDataService: NavigateDataService) {
    if (global.role === UserStatus.Shared && location.pathname.indexOf(global.shareRouteUrl) >= 0) {
      global.showRemove = false;
    }
    else {
      global.showRemove = true;
    }
  }

  removeSavedResources() {
    if (this.selectedPlanDetails) {
      this.removeSavedPlan();
    }
    if (this.resourceId) {
      this.removedSavedResource();
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
      if (topic.name === this.selectedPlanDetails.topic) {
        this.selectedPlanDetails.planDetails.topics.splice(this.topicIndex, 1);
      }
      this.topicIndex++;
    });
  }

  removePersonalizedPlan() {
    if (this.router.url.indexOf("/plan") !== -1) {
      this.navigateDataService.setData(this.selectedPlanDetails.planDetails);
      this.personalizedPlanComponent.getTopics();
      this.personalizedPlanService.showSuccess("Removed from Plan successfully");
    } else {
      const params = {
        "personalizedPlan": this.selectedPlanDetails.planDetails,
        "oId": this.global.userId
      }
      this.personalizedPlanService.userPlan(params)
        .subscribe(response => {
          if (response) {
            this.profileComponent.getPersonalizedPlan();
            this.personalizedPlanService.showSuccess('Removed from plan successfully');
          }
        });
    }
  }

  removedSavedResource() {
    this.removeResource = { itemId: '', resourceType: '', resourceDetails: {} };
    this.profileResources.resourceTags = [];
    if (this.personalizedResources.resources) {
      this.removeUserSavedResourceFromProfile(this.personalizedResources.resources, true);
    }
    if (this.personalizedResources.webResources) {
      this.removeUserSavedResourceFromProfile(this.personalizedResources.webResources, false);
    }
    if (this.personalizedResources.topics) {
      this.removeUserSavedResourceFromProfile(this.personalizedResources.topics, false);
    }
    this.saveResourceToProfile(this.profileResources.resourceTags);
  }

  removeUserSavedResourceFromProfile(savedResource, isResource) {
    savedResource.forEach(resource => {
      if (isResource) {
        if (resource.id !== this.resourceId && resource.resourceType !== "Topics" && resource.resourceType !== "WebResources") {
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
    this.removeResource = { itemId: resource.id, resourceType: resource.resourceType, resourceDetails: this.resourceDetails };
    this.profileResources.resourceTags.push(this.removeResource);

  }

  saveResourceToProfile(resourceTags) {
    this.profileResources = { oId: this.global.userId, resourceTags: resourceTags, type: 'resources' };
    this.personalizedPlanService.saveResources(this.profileResources)
      .subscribe(response => {
        if (response != undefined) {
          this.personalizedPlanService.showSuccess('Resource removed from profile');
          this.eventUtilityService.updateSavedResource("RESOURCE REMOVED");
        }
      });
  }

  ngOnInit() {
  }

}
