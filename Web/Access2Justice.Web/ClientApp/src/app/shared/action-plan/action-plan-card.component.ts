import { Component, Input, TemplateRef } from '@angular/core';
import { PlanSteps, UpdatePlan, Steps, PlanTag, StepTag, UserUpdatePlan, PlanTopic, PersonalizedPlan, PlanStep, QuickLink } from '../../guided-assistant/personalized-plan/personalized-plan';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { DomSanitizer } from '@angular/platform-browser';
import { Title } from '@angular/platform-browser/src/browser/title';

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

  personalizedPlanStep: PlanStep = { stepId: '', title: '', description: '', order: 1, isComplete: false, resources: []};
  personalizedPlanSteps: Array<PlanStep>;
  quickLink: QuickLink = { title: '', url: '' };
  quickLinks: Array<QuickLink>;
  planTopic: PlanTopic = { name: '', icon: '', quickLinks: [], topicId: '', planStep: this.personalizedPlanSteps };
  planTopics: Array<PlanTopic>;
  personalizedPlan: PersonalizedPlan = { id: '', topics: this.planTopics };

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
      if(planDetails.topics) {
        this.buildPersonalizedPlan(planDetails);
      }
    }
  }

  buildPersonalizedPlan(planDetails) {
    planDetails.topics.forEach(item => {
      this.planStep = { topicId: '', topicName: '', stepTags: [] };
      if (item) {
        this.planStep.topicName = item.name;
        this.planStep.topicId = item.topicId;
      }
      this.planStep.stepTags = this.orderBy(item.steps, "isComplete");
      this.planSteps.push(this.planStep);
      this.displaySteps = true;
    });
  }



  checkCompleted(event, topicId, stepId, template: TemplateRef<any>) {
    if (this.userId) {
      this.planSteps = [];
      this.planTags = [];
      this.steps = [];
      this.isUser = false;
      this.isChecked = event.target.checked;
      this.updateMarkCompleted(topicId, stepId, this.isChecked);
      this.updateProfilePlan(this.isChecked, template);
      //this.updatedUserPlan(this.isChecked, this.planTags, template);
    } else {
      this.isUser = true;
      this.modalRef = this.modalService.show(template);
    }
  }

  updateMarkCompleted(topicId, stepId, isChecked) {
    this.planTopics = [];
    this.personalizedPlanSteps = [];
    this.planTopic = { name: '', icon: '', quickLinks: [], topicId: '', planStep: this.personalizedPlanSteps };
    this.planDetails.topics.forEach(item => {
      if (item.topicId === topicId) {
        this.personalizedPlanSteps = this.updateStepTagForMatchingTopicId(item, stepId, isChecked);
      } else {
        this.personalizedPlanSteps = this.stepTagForNonMatchingTopicId(item);
      }
      this.quickLinks= this.getQuickLinks(item);
      this.planTopic = { name: item.name, icon: item.icon, quickLinks: this.quickLinks, topicId: item.topicId, planStep: this.personalizedPlanSteps };
      this.planTopics.push(this.planTopic);
    });
    this.personalizedPlan = { id: this.planDetails.id, topics: this.planTopics };
    console.log(this.planDetails);
  }

  getQuickLinks(topic): Array<QuickLink>{
    this.quickLinks = [];
    topic.quickLinks.forEach(quickLink => {
      this.quickLink = { url: quickLink.url, title: quickLink.title };
      this.quickLinks.push(this.quickLink);
    });
    return this.quickLinks;
  }

  updateStepTagForMatchingTopicId(item, stepId, isChecked): Array<PlanStep> {
    this.personalizedPlanSteps = [];
    item.steps.forEach(step => {
      this.personalizedPlanStep = {
        stepId: step.stepId, title: step.title, description: step.description,
        order: step.order, isComplete: step.isComplete, resources: step.resources
      };
      if (step.stepId === stepId) {
        this.personalizedPlanStep.isComplete = isChecked;
      }
      this.personalizedPlanSteps.push(this.personalizedPlanStep);
    });
    item.steps = this.personalizedPlanSteps;
    return this.personalizedPlanSteps;
  }

  stepTagForNonMatchingTopicId(item): Array<PlanStep> {
    this.personalizedPlanSteps = [];
    item.steps.forEach(step => {
      this.personalizedPlanStep = {
        stepId: step.stepId, title: step.title, description: step.description,
        order: step.order, isComplete: step.isComplete, resources: step.resources
      };
      this.personalizedPlanSteps.push(this.personalizedPlanStep);
    });
    return this.personalizedPlanSteps;
  }

  updatePlanSteps(item) {
    this.planStep.stepTags = item.steps;
    this.planSteps.push(this.planStep);
    this.planTag = { topicId: item.topicId, stepTags: this.updatedSteps };
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

    this.personalizedPlanService.userPlan(this.personalizedPlan)//this.userUpdatePlan)
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

