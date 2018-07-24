import { Component, Input, TemplateRef } from '@angular/core';
import { PlanSteps, UpdatePlan, Steps, PlanTag, StepTag, UserUpdatePlan } from '../../guided-assistant/personalized-plan/personalized-plan';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-action-plan-card',
  templateUrl: './action-plan-card.component.html',
  styleUrls: ['./action-plan-card.component.css']
})
export class ActionPlanCardComponent implements OnChanges {
  @Input() planDetails;
  topics: any = [];
  planSteps: Array<PlanSteps>;
  steps: Array<Steps>;
  updatedSteps: Array<StepTag>;
  planTags: Array<PlanTag>;
  planStep: PlanSteps = { topicId: '', topicName: '', stepTags: [] };
  step: Steps = { id: '', title: '', type: '', description: '', order: '', markCompleted: false };
  updatedStep: StepTag = { id: '', order: '', markCompleted: false };
  displaySteps: boolean = false;
  updatePlan: UpdatePlan = { id: '', oId: '', planTags: this.planTags };
  userUpdatePlan: UserUpdatePlan = { id: '', planId: '', oId: '', planTags: this.planTags, type: '' };
  planTag: PlanTag = { topicId: '', stepTags: this.steps };
  updatedPlan: any;
  modalRef: BsModalRef;
  url: any;
  userId: string;
  isCompleted: boolean = false;
  selectedPlanDetails: any = { planDetails: [], topicId: '' };
  isUser: boolean = false;
  isChecked: boolean = false;

  constructor(private personalizedPlanService: PersonalizedPlanService,
    private modalService: BsModalService,
    public sanitizer: DomSanitizer) {
    this.sanitizer = sanitizer;
    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      this.userId = profileData["UserId"];
    }
  }

  getPersonalizedPlan(planDetails): void {
    if (planDetails.length === 0) {
      this.planSteps = null;
      this.displaySteps = false;
    } else {
      this.planSteps = [];
      if (planDetails[0]) {
        planDetails = planDetails[0];
      }
      this.buildPersonalizedPlan(planDetails);
    }
  }

  buildPersonalizedPlan(planDetails) {
    if (planDetails.planTags) {
      this.buildPersonalizedPlanWithPlanTags(planDetails);
    } else {
      this.buildPersonalizedPlanWithPlanSteps(planDetails);
    }
  }

  buildPersonalizedPlanWithPlanTags(planDetails) {
    planDetails.planTags.forEach(item => {
      this.planStep = { topicId: '', topicName: '', stepTags: [] };
      if (item.id[0]) {
        this.planStep.topicName = item.id[0].name;
        this.planStep.topicId = item.id[0].id;
      }
      this.planStep.stepTags = this.orderBy(item.stepTags, "markCompleted");
      this.planSteps.push(this.planStep);
      this.displaySteps = true;
    });
  }

  buildPersonalizedPlanWithPlanSteps(planDetails) {
    this.planStep = { topicId: '', topicName: '', stepTags: [] };
    this.planStep.topicName = planDetails.id[0].name;
    this.planStep.topicId = planDetails.id[0].id;
    this.planStep.stepTags = this.orderBy(planDetails.stepTags, "markCompleted");
    this.planSteps.push(this.planStep);
    this.displaySteps = true;
  }

  checkCompleted(event, topicId, stepId, template: TemplateRef<any>) {
    if (this.userId) {
      this.planSteps = [];
      this.planTags = [];
      this.steps = [];
      this.isUser = false;
      this.isChecked = event.target.checked;
      this.updateMarkCompleted(topicId, stepId, this.isChecked);
      this.updatedUserPlan(this.isChecked, this.planTags, template);
    } else {
      this.isUser = true;
      this.modalRef = this.modalService.show(template);
    }
  }

  updateMarkCompleted(topicId, stepId, isChecked) {
    this.planDetails.planTags.forEach(item => {
      this.planStep = { topicId: '', topicName: '', stepTags: [] };
      this.planStep.topicName = item.id[0].name;
      this.planStep.topicId = item.id[0].id;
      this.updatedSteps = [];
      if (this.planStep.topicId === topicId) {
        this.updatedSteps = this.updateStepTagForMatchingTopicId(item, stepId, isChecked);
      } else {
        this.updatedSteps = this.stepTagForNonMatchingTopicId(item);
      }
      this.updatePlanSteps(item);
    });
  }

  updateStepTagForMatchingTopicId(item, stepId, isChecked): Array<StepTag> {
    item.stepTags.forEach(step => {
      this.step = { id: '', title: '', type: '', description: '', order: '', markCompleted: false };
      this.step = step;
      this.updatedStep = { id: step.id.id, order: step.order, markCompleted: step.markCompleted }
      if (step.id.id === stepId) {
        this.step.markCompleted = isChecked;
        this.updatedStep.markCompleted = isChecked;
      }
      this.steps.push(this.step);
      this.updatedSteps.push(this.updatedStep);
    });
    item.stepTags = this.steps;
    return this.updatedSteps;
  }

  stepTagForNonMatchingTopicId(item): Array<StepTag> {
    item.stepTags.forEach(step => {
      this.updatedStep = { id: step.id.id, order: step.order, markCompleted: step.markCompleted }
      this.updatedSteps.push(this.updatedStep);
    });
    return this.updatedSteps;
  }

  updatePlanSteps(item) {
    this.planStep.stepTags = item.stepTags;
    this.planSteps.push(this.planStep);
    this.planTag = { topicId: item.id[0].id, stepTags: this.updatedSteps };
    this.planTags.push(this.planTag);
    this.displaySteps = true;
  }

  updatedUserPlan(isChecked, planTags, template) {
    this.planTags = planTags;
    this.updatePlan = { id: '', oId: '', planTags: this.planTags };
    if (this.planDetails.planId) {
      this.updateProfilePlan(isChecked, template);
    } else {
      this.updatePersonalizedPlan(isChecked, template);
    }
  }

  updateProfilePlan(isChecked, template) {
    this.userUpdatePlan = { id: this.planDetails.id, planId: this.planDetails.planId, oId: this.userId, planTags: this.planTags, type: this.planDetails.type };
    this.personalizedPlanService.userPlan(this.userUpdatePlan)
      .subscribe(response => {
        if (response) {
          if (isChecked) {
            this.isCompleted = true;
          }
          else {
            this.isCompleted = false;
          }
          this.modalRef = this.modalService.show(template);
        }
      });
  }

  updatePersonalizedPlan(isChecked, template) {
    this.updatePlan = { id: this.planDetails.id, oId: this.userId, planTags: this.planTags };
    this.personalizedPlanService.getMarkCompletedUpdatedPlan(this.updatePlan)
      .subscribe(response => {
        if (response) {
          if (isChecked) {
            this.isCompleted = true;
          } else {
            this.isCompleted = false;
          }
          this.modalRef = this.modalService.show(template);
        }
      });
  }

  close() {
    this.modalRef.hide();
    this.personalizedPlanService.getActionPlanConditions(this.planDetails.id).subscribe(items => {
      this.updatedPlan = items;
      this.getPersonalizedPlan(this.updatedPlan);
    })
  }

  resourceUrl(url) {
    this.url = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    return this.url;
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  planTagOptions(topicId) {
    this.selectedPlanDetails = { planDetails: this.planDetails, topicId: topicId };
  }

  ngOnChanges() {
    this.getPersonalizedPlan(this.planDetails);
  }

  orderBy(items, field) {
    return items.sort((a, b) => {
      if (a[field] < b[field]) {
        return -1;
      } else if (a[field] > b[field]) {
        return 1;
      } else {
        return 0;
      }
    });
  }
}

