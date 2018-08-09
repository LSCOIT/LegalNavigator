import { Component, Input, TemplateRef, EventEmitter, Output } from '@angular/core';
import { PlanTopic, PersonalizedPlan, PlanStep, PersonalizedPlanTopic } from '../../../../guided-assistant/personalized-plan/personalized-plan';
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
  @Input() topicsList
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
  personalizedPlan: PersonalizedPlan = { id: '', topics: this.planTopics, isShared: false };
  selectedPlanDetails: any = { planDetails: [], topicId: '' };
  topics: Array<any> = [];
  filteredtopicsList: Array<PersonalizedPlanTopic> = [];
  tempFilteredtopicsList: Array<PersonalizedPlanTopic> = [];
  personalizedPlanTopic: PersonalizedPlanTopic = { topic: {}, isSelected: false };
  @Output() notifyFilterTopics = new EventEmitter<object>();
  removePlanDetails: any;
  plan: any;

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
    } else if (this.planDetails.topics) {
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
    this.getPlanDetails(topicId, stepId, this.isChecked, template);
  }

  getPlanDetails(topicId, stepId, isChecked, template) {
    this.personalizedPlanService.getActionPlanConditions(this.planDetails.id)
      .subscribe(plan => {
        if (plan) {
          this.topics = plan.topics;
        }
        this.plan = this.personalizedPlanService.getPlanDetails(this.topics, plan);
        this.updateMarkCompleted(topicId, stepId, isChecked);
        this.updateProfilePlan(this.isChecked, template);
      });
  }

  updateMarkCompleted(topicId, stepId, isChecked) {
    this.planTopics = [];
    this.personalizedPlanSteps = [];
    this.planTopic = { topicId: '', steps: this.personalizedPlanSteps };
    this.plan.topics.forEach(item => {
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
          this.filteredtopicsList = [];
          response.topics.forEach(topic => {
            for (let i = 0; i < this.tempFilteredtopicsList.length; i++) {
              if (topic.topicId === this.tempFilteredtopicsList[i].topic.topicId) {
                this.personalizedPlanTopic = { topic: topic, isSelected: this.tempFilteredtopicsList[i].isSelected };
                this.filteredtopicsList.push(this.personalizedPlanTopic);
              }
            }
          });
          this.getUpdatedPersonalizedPlan(response, isChecked, template);
        }
      });
  }

  getUpdatedPersonalizedPlan(response, isChecked, template) {
    this.notifyFilterTopics.emit({ plan: response, topicsList: this.filteredtopicsList });
    this.loadPersonalizedPlan();
    if (isChecked) {
      this.isCompleted = true;
    } else {
      this.isCompleted = false;
    }
    this.modalRef = this.modalService.show(template);
  }

  loadPersonalizedPlan() {
    this.planDetails.topics = [];
    this.filteredtopicsList.forEach(topic => {
      if (topic.isSelected) {
        this.planDetails.topics.push(topic);
      }
    });
  }

  close() {
    this.modalRef.hide();
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
    this.removePlanDetails = [];
    this.getRemovePlanDetails();
    this.removePlanDetails.forEach(item => {
      this.personalizedPlanSteps = [];
      item.topic.steps.forEach(step => {
        this.personalizedPlanStep = {
          stepId: step.stepId, title: step.title, description: step.description,
          order: step.order, isComplete: step.isComplete, resources: this.getResourceIds(step.resources), topicIds: []
        };
        this.personalizedPlanSteps.push(this.personalizedPlanStep);
      });
      this.planTopic = { topicId: item.topic.topicId, steps: this.personalizedPlanSteps };
      this.planTopics.push(this.planTopic);
    });
    this.personalizedPlan = { id: this.planDetails.id, topics: this.planTopics, isShared: this.planDetails.isShared };
    this.selectedPlanDetails = { planDetails: this.personalizedPlan, topicId: topicId };
  }

  getRemovePlanDetails() {
    this.removePlanDetails = [];
    this.tempFilteredtopicsList.forEach(topic => {
      this.removePlanDetails.push(topic);
    });
  }

  ngOnChanges() {
    this.getPersonalizedPlan(this.planDetails);
    this.tempFilteredtopicsList = this.topicsList;
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

