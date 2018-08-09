import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { SavedResources, ProfileResources } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { ProfileComponent } from '../../../../profile/profile.component';
import { PersonalizedPlanComponent } from '../../../../guided-assistant/personalized-plan/personalized-plan.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { resource } from 'selenium-webdriver/http';
import { EventUtilityService } from '../../../event-utility.service';

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
  userId: string;
  isRemoved: boolean;
  removeResource: SavedResources;
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  isRemovedPlan: boolean = false;
  modalRef: BsModalRef;
  resourceDetails: any;
  topicIndex: number;

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private profileComponent: ProfileComponent,
    private personalizedPlanComponent: PersonalizedPlanComponent,
    private modalService: BsModalService,
    private router: Router,
    private eventUtilityService: EventUtilityService
  ) {
      let profileData = sessionStorage.getItem("profileData");
      if (profileData != undefined) {
        profileData = JSON.parse(profileData);
        this.userId = profileData["UserId"];
    }
  }

  removeSavedResources(template: TemplateRef<any>) {
    if (this.selectedPlanDetails) {
      this.removeSavedPlan(template);
    }
    if (this.resourceId) {
      this.removedSavedResource(template);
    }
  }

  removeSavedPlan(template) {
    if (this.selectedPlanDetails.planDetails) {
      this.createRemovePlanTag();
      this.removePersonalizedPlan(template);
    }
  }

  createRemovePlanTag() {
    this.topicIndex = 0;
    this.selectedPlanDetails.planDetails.topics.forEach(topic => {
      if (topic.topicId === this.selectedPlanDetails.topicId) {
        this.selectedPlanDetails.planDetails.topics.splice(this.topicIndex, 1);
      }
      this.topicIndex++;
    });
  }

  removePersonalizedPlan(template) {
    const params = {
      "id": this.selectedPlanDetails.planDetails.id,
      "topics": this.selectedPlanDetails.planDetails.topics,
      "isShared": this.selectedPlanDetails.planDetails.isShared
    }
    this.personalizedPlanService.userPlan(params)
      .subscribe(response => {
        if (response) {
          if (this.userId) {
            this.profileComponent.getPersonalizedPlan();
          } else {
            this.personalizedPlanComponent.getTopics();
          }
        }
      });
  }

  removedSavedResource(template) {
    this.removeResource = { itemId: '', resourceType: '', resourceDetails: {} };
    this.profileResources.resourceTags = [];
    if (this.personalizedResources.resources) {
      this.removeUserSavedResourceFromProfile(this.personalizedResources.resources, true, template);
    }
    if (this.personalizedResources.webResources) {
      this.removeUserSavedResourceFromProfile(this.personalizedResources.webResources, false, template);
    }
    if (this.personalizedResources.topics) {
      this.removeUserSavedResourceFromProfile(this.personalizedResources.topics, false, template);
    }
    this.saveResourceToProfile(this.profileResources.resourceTags, template);
  }

  removeUserSavedResourceFromProfile(savedResource, isResource, template) {
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

  saveResourceToProfile(resourceTags, template) {
    this.profileResources = { oId: this.userId, resourceTags: resourceTags, type: 'resources' };
    this.personalizedPlanService.saveResources(this.profileResources)
      .subscribe(response => {
        if (response != undefined) {
          this.isRemovedPlan = true;
          this.eventUtilityService.updateSavedResource("RESOURCE REMOVED");
        }
      });
  }

  close() {
    this.modalRef.hide();
    this.router.navigate(['/profile']);
    this.profileComponent.ngOnInit();
  }

  ngOnInit() {
  }

}
