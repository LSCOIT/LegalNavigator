import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { PersonalizedPlanService } from '../../../guided-assistant/personalized-plan/personalized-plan.service';
import { RemovePlanTag, UserRemovePlanTag, StepTag, UpdatePlan, UserUpdatePlan, PlanTag, SavedResources, ProfileResources } from '../../../guided-assistant/personalized-plan/personalized-plan';
import { ProfileComponent } from '../../../profile/profile.component';
import { PersonalizedPlanComponent } from '../../../guided-assistant/personalized-plan/personalized-plan.component';
import { resource } from 'selenium-webdriver/http';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { Router } from '@angular/router';

@Component({
  selector: 'app-remove-button',
  template: `
  <span (click)="removeSavedResources(template)">Remove</span>
<ng-template #template>
  <div *ngIf="isRemovedPlan">
    <button type="button" class="close" aria-label="Close" (click)="close()">
      <span aria-hidden="true">&times;</span>
    </button>
    <div class="row text-center">
      <h3>Removed Resource from Profile</h3>
    </div>
    <div class="row text-center">
      <button type="button" (click)="close()" class="btn btn-primary">Close</button>
    </div>
  </div>
</ng-template>
 `
})
export class RemoveButtonComponent implements OnInit {
  @Input() resourceId;
  @Input() resourceType;
  @Input() personalizedResources;
  @Input() selectedPlanDetails;
  userId: string;
  isRemoved: boolean;
  removePlanTag: RemovePlanTag = { id: '', oId: '', topicId: '' };
  userRemovePlanTag: UserRemovePlanTag = { id: '', planId: '', oId: '', topicId: '', type: '' };
  updatedStep: StepTag = { id: '', order: '', markCompleted: false };
  updatedSteps: Array<StepTag>;
  planTag: PlanTag = { topicId: '', stepTags: this.updatedSteps };
  planTags: Array<PlanTag>;
  updatePlan: UpdatePlan = { id: '', oId: '', planTags: this.planTags };
  userUpdatePlan: UserUpdatePlan = { id: '', planId: '', oId: '', planTags: this.planTags, type: '' };
  removeResource: SavedResources;
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  isRemovedPlan: boolean = false;
  modalRef: BsModalRef;

  constructor(private personalizedPlanService: PersonalizedPlanService, private profileComponent: ProfileComponent,
    private personalizedPlanComponent: PersonalizedPlanComponent, private modalService: BsModalService,
    private router: Router) {
    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      this.userId = profileData["UserId"];
    }
  }

  removeSavedResources(template: TemplateRef<any>) {
    this.updatedSteps = [];
    this.planTags = [];
    if (this.selectedPlanDetails) {
      this.removeSavedPlan(template);
    }
    if (this.resourceId) {
      this.removedSavedResource(template);
    }
  }

  removeSavedPlan(template) {
    if (this.selectedPlanDetails.planDetails) {
      this.selectedPlanDetails.planDetails.planTags.forEach(plan => {
        if (plan.topicId !== this.selectedPlanDetails.topicId) {
          plan.stepTags.forEach(step => {
            this.updatedStep = { id: step.id.id, order: step.order, markCompleted: step.markCompleted };
            this.updatedSteps.push(this.updatedStep);
          });
          this.planTag = { topicId: plan.topicId, stepTags: this.updatedSteps };
          this.planTags.push(this.planTag);
        }
      });
      if (this.selectedPlanDetails.planDetails.planId) {
        this.userUpdatePlan = {
          id: this.selectedPlanDetails.planDetails.id, planId: this.selectedPlanDetails.planDetails.planId,
          oId: this.userId, planTags: this.planTags, type: this.selectedPlanDetails.planDetails.type
        };
        this.personalizedPlanService.userPlan(this.userUpdatePlan)
          .subscribe(response => {
            if (response != undefined) {
              this.profileComponent.getPersonalizedPlan();
              //this.isRemovedPlan = true;
              //this.modalRef = this.modalService.show(template);
            }
          });
      }
      else {
        this.updatePlan = { id: this.selectedPlanDetails.planDetails.id, oId: this.userId, planTags: this.planTags };
        this.personalizedPlanService.getMarkCompletedUpdatedPlan(this.updatePlan)
          .subscribe(response => {
            if (response != undefined) {
              this.personalizedPlanComponent.getTopics();
              //this.isRemovedPlan = true;
              //this.modalRef = this.modalService.show(template);
            }
          });
      }
    }
  }

  removedSavedResource(template) {
    console.log(this.personalizedResources);
    this.removeResource = { itemId: '', resourceType: '' };
    this.profileResources.resourceTags = [];
    if (this.personalizedResources.resources) {
      this.personalizedResources.resources.forEach(resource => {
        if (resource.id !== this.resourceId) {
          this.removeResource = { itemId: resource.id, resourceType: resource.resourceType };
          this.profileResources.resourceTags.push(this.removeResource);
        }
      });
      //this.saveResourceToProfile(this.profileResources.resourceTags, template);
    }
    if (this.personalizedResources.topics) {
      this.personalizedResources.topics.forEach(topic => {
        if (topic.id !== this.resourceId) {
          this.removeResource = { itemId: topic.id, resourceType: topic.resourceType };
          this.profileResources.resourceTags.push(this.removeResource);
        }
      });
      //this.saveResourceToProfile(this.profileResources.resourceTags, template);
    }
    this.saveResourceToProfile(this.profileResources.resourceTags, template);
  }

  saveResourceToProfile(resourceTags, template) {
    this.profileResources = { oId: this.userId, resourceTags: resourceTags, type: 'resources' };
    this.personalizedPlanService.userPlan(this.profileResources)
      .subscribe(response => {
        if (response != undefined) {
          this.profileComponent.getpersonalizedResources();
          this.isRemovedPlan = true;
          this.modalRef = this.modalService.show(template);
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
