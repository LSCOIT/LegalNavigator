import { Component, Input, TemplateRef } from '@angular/core';
import { PlanTopic, PersonalizedPlan, PlanStep } from '../../guided-assistant/personalized-plan/personalized-plan';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { DomSanitizer } from '@angular/platform-browser';
import { Title } from '@angular/platform-browser/src/browser/title';

@Component({
  selector: 'app-action-plans',
  templateUrl: './action-plans.component.html',
  styleUrls: ['./action-plans.component.css']
})
export class ActionPlansComponent implements OnChanges {
  @Input() planDetails;
  displaySteps: boolean = false;
  updatedPlan: any;
  modalRef: BsModalRef;
  url: any;
  userId: string;
  isCompleted: boolean = false;
  isUser: boolean = false;
  isChecked: boolean = false;
  personalizedPlanStep: PlanStep = { stepId: '', title: '', description: '', order: 1, isComplete: false, resources: [], topicIds: [] };
  personalizedPlanSteps: Array<PlanStep>;
  planTopic: PlanTopic = { topicId: '', steps: this.personalizedPlanSteps };
  planTopics: Array<PlanTopic>;
  resourceIds: Array<string>;
  personalizedPlan: PersonalizedPlan = { id: '', topics: this.planTopics, isShared:false };
  selectedPlanDetails: any = { planDetails: [], topicId: '' };

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
    this.planDetails = planDetails;
    if (planDetails.length === 0) {
      this.displaySteps = false;
    } else if (this.planDetails.topics){
        this.sortStepsByOrder(planDetails);
        this.displaySteps = true;
    }
  }

  sortStepsByOrder(planDetails) {
    planDetails.topics.forEach(topic => {
      topic.steps = this.orderBy(topic.steps, "isComplete");
    });
  }

  checkCompleted(event, topicId, stepId, template: TemplateRef<any>) {
      this.isChecked = event.target.checked;
      this.updateMarkCompleted(topicId, stepId, this.isChecked);
      this.updateProfilePlan(this.isChecked, template);
  }

  updateMarkCompleted(topicId, stepId, isChecked) {
    this.planTopics = [];
    this.personalizedPlanSteps = [];
    this.planTopic = { topicId: '', steps: this.personalizedPlanSteps };
    this.planDetails.topics.forEach(item => {
      if (item.topicId === topicId) {
        this.personalizedPlanSteps = this.updateStepTagForMatchingTopicId(item, stepId, isChecked);
      } else {
        this.personalizedPlanSteps = this.stepTagForNonMatchingTopicId(item);
      }
      this.planTopic = { topicId: item.topicId, steps: this.personalizedPlanSteps };
      this.planTopics.push(this.planTopic);
    });
    this.personalizedPlan = { id: this.planDetails.id, topics: this.planTopics, isShared: this.planDetails.isShared };
  }

  getResourceIds(resources): Array<string> {
    this.resourceIds = [];
    resources.forEach(resource => {
      this.resourceIds.push(resource.id);
    });
    return this.resourceIds;
  }

  updateStepTagForMatchingTopicId(item, stepId, isChecked): Array<PlanStep> {
    this.personalizedPlanSteps = [];
    item.steps.forEach(step => {
      this.personalizedPlanStep = {
        stepId: step.stepId, title: step.title, description: step.description,
        order: step.order, isComplete: step.isComplete, resources: this.getResourceIds(step.resources), topicIds: []
      };
      if (step.stepId === stepId) {
        this.personalizedPlanStep.isComplete = isChecked;
      }
      this.personalizedPlanSteps.push(this.personalizedPlanStep);
    });
    return this.personalizedPlanSteps;
  }

  stepTagForNonMatchingTopicId(item): Array<PlanStep> {
    this.personalizedPlanSteps = [];
    item.steps.forEach(step => {
      this.personalizedPlanStep = {
        stepId: step.stepId, title: step.title, description: step.description,
        order: step.order, isComplete: step.isComplete, resources: this.getResourceIds(step.resources), topicIds: []
      };
      this.personalizedPlanSteps.push(this.personalizedPlanStep);
    });
    return this.personalizedPlanSteps;
  }

  updateProfilePlan(isChecked, template) {
    const params = {
      "id": this.personalizedPlan.id,
      "topics": this.personalizedPlan.topics,
      "isShared": this.personalizedPlan.isShared
    }
    this.personalizedPlanService.userPlan(params)
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
    });
  }

  resourceUrl(url) {
    this.url = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    return this.url;
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  planTagOptions(topicId) {
    this.planTopics = [];
    this.planTopic = { topicId: '', steps: this.personalizedPlanSteps };
    this.planDetails.topics.forEach(item => {
      this.personalizedPlanSteps = [];
      item.steps.forEach(step => {
        this.personalizedPlanStep = {
          stepId: step.stepId, title: step.title, description: step.description,
          order: step.order, isComplete: step.isComplete, resources: this.getResourceIds(step.resources), topicIds: []
        };
        this.personalizedPlanSteps.push(this.personalizedPlanStep);
      });
      this.planTopic = { topicId: item.topicId, steps: this.personalizedPlanSteps };
      this.planTopics.push(this.planTopic);
    });
    this.personalizedPlan = { id: this.planDetails.id, topics: this.planTopics, isShared: this.planDetails.isShared };
    this.selectedPlanDetails = { planDetails: this.personalizedPlan, topicId: topicId };
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

