import { Component, Input, EventEmitter, Output, TemplateRef } from '@angular/core';
import { PlanTopic, PersonalizedPlan, PlanStep, PersonalizedPlanTopic } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { DomSanitizer } from '@angular/platform-browser';
import { Title } from '@angular/platform-browser/src/browser/title';
import { ToastrService } from 'ngx-toastr';
import { Global, UserStatus } from '../../../../global';
import { HttpParams } from '@angular/common/http';
import { NavigateDataService } from '../../../navigate-data.service';

@Component({
  selector: 'app-action-plans',
  templateUrl: './action-plans.component.html',
  styleUrls: ['./action-plans.component.css']
})
export class ActionPlansComponent implements OnChanges {
  @Input() planDetails;
  @Input() topicsList;
  displaySteps: boolean = false;
  updatedPlan: any;
  modalRef: BsModalRef;
  url: any;
  isCompleted: boolean = false;
  isUser: boolean = false;
  isChecked: boolean = false;
  //personalizedPlanStep: PlanStep = { stepId: '', title: '', description: '', order: 1, isComplete: false, resources: [], topicIds: [] };
  //personalizedPlanSteps: Array<PlanStep>;
  ////planTopic: PlanTopic = { topicId: '', steps: this.personalizedPlanSteps };
  planTopics: Array<PlanTopic>;
  //resourceIds: Array<string>;
  personalizedPlan: PersonalizedPlan = { id: '', topics: this.planTopics, isShared: false };
  selectedPlanDetails: any;
  //topics: Array<any> = [];
  //filteredtopicsList: Array<PersonalizedPlanTopic> = [];
  tempFilteredtopicsList: Array<PersonalizedPlanTopic> = [];
  //personalizedPlanTopic: PersonalizedPlanTopic = { topic: {}, isSelected: false };
  @Output() notifyFilterTopics = new EventEmitter<object>();
  removePlanDetails: any;
  plan: any;
  resourceTypeList = [
    'Forms',
    'Guided Assistant',
    'Organizations',
    'Videos',
    'Articles'
  ];

  constructor(
    private modalService: BsModalService,
    private personalizedPlanService: PersonalizedPlanService,
    public sanitizer: DomSanitizer,
    private toastr: ToastrService,
    private global: Global,
    private navigateDataService: NavigateDataService) {
    this.sanitizer = sanitizer;
    if (global.role === UserStatus.Shared && location.pathname.indexOf(global.shareRouteUrl) >= 0) {
      global.showMarkComplete = false;
      global.showDropDown = false;
    }
    else {
      global.showMarkComplete = true;
      global.showDropDown = true;
    }
  }

  getPersonalizedPlan(planDetails): void {
    this.planDetails = planDetails;
    if (!(planDetails) || planDetails.length === 0) {
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

  checkCompleted(event, topicId, stepId) {
    this.isChecked = event.target.checked;
    this.planDetails.topics.forEach(topic => {
      if (topic.topicId === topicId) {
        topic.steps.forEach(step => {
          if (step.stepId === stepId) {
            step.isComplete = this.isChecked;
          }
        });
        topic.steps = this.orderBy(topic.steps, "isComplete");
      }
    });
    this.navigateDataService.setData(this.planDetails);
    if (this.isChecked) {
      this.personalizedPlanService.showSuccess("Step Completed");
    } else {
      this.personalizedPlanService.showSuccess("Step Added Back");
    }
    //this.getPlanDetails(topicId, stepId, this.isChecked);
  }

  //getPlanDetails(topicId, stepId, isChecked) {
  //  let params = new HttpParams()
  //    .set("personalizedPlanId", this.planDetails.id);
  //  this.personalizedPlanService.getActionPlanConditions(params)
  //    .subscribe(plan => {
  //      if (plan) {
  //        this.topics = plan.topics;
  //      }
  //      this.plan = this.personalizedPlanService.getPlanDetails(this.topics, plan);
  //      this.updateMarkCompleted(topicId, stepId, isChecked);
  //      this.updateProfilePlan(this.isChecked);
  //    });
  //}

  //updateMarkCompleted(topicId, stepId, isChecked) {
  //  this.planTopics = [];
  //  this.personalizedPlanSteps = [];
  //  //this.planTopic = { topicId: '', steps: this.personalizedPlanSteps };
  //  this.plan.topics.forEach(item => {
  //    if (item.topicId === topicId) {
  //      this.personalizedPlanSteps = this.updateStepTagForMatchingTopicId(item, stepId, isChecked);
  //    } else {
  //      this.personalizedPlanSteps = this.stepTagForNonMatchingTopicId(item);
  //    }
  //    //this.planTopic = { topicId: item.topicId, steps: this.personalizedPlanSteps };
  //    //this.planTopics.push(this.planTopic);
  //  });
  //  this.personalizedPlan = { id: this.planDetails.id, topics: this.planTopics, isShared: this.planDetails.isShared };
  //}

  //updateStepTagForMatchingTopicId(item, stepId, isChecked): Array<PlanStep> {
  //  this.personalizedPlanSteps = [];
  //  item.steps.forEach(step => {
  //    this.personalizedPlanStep = {
  //      stepId: step.stepId, title: step.title, description: step.description,
  //      order: step.order, isComplete: step.isComplete, resources: this.personalizedPlanService.getResourceIds(step.resources), topicIds: []
  //    };
  //    if (step.stepId === stepId) {
  //      this.personalizedPlanStep.isComplete = isChecked;
  //    }
  //    this.personalizedPlanSteps.push(this.personalizedPlanStep);
  //  });
  //  return this.personalizedPlanSteps;
  //}

  //stepTagForNonMatchingTopicId(item): Array<PlanStep> {
  //  this.personalizedPlanSteps = [];
  //  item.steps.forEach(step => {
  //    this.personalizedPlanStep = {
  //      stepId: step.stepId, title: step.title, description: step.description,
  //      order: step.order, isComplete: step.isComplete, resources: this.personalizedPlanService.getResourceIds(step.resources), topicIds: []
  //    };
  //    this.personalizedPlanSteps.push(this.personalizedPlanStep);
  //  });
  //  return this.personalizedPlanSteps;
  //}

  //updateProfilePlan(isChecked) {
  //  const params = {
  //    "id": this.personalizedPlan.id,
  //    "topics": this.personalizedPlan.topics,
  //    "isShared": this.personalizedPlan.isShared
  //  }
  //  this.personalizedPlanService.userPlan(params)
  //    .subscribe(response => {
  //      if (response) {
  //        this.filteredtopicsList = [];
  //        response.topics.forEach(topic => {
  //          for (let i = 0; i < this.tempFilteredtopicsList.length; i++) {
  //            if (topic.topicId === this.tempFilteredtopicsList[i].topic.topicId) {
  //              this.personalizedPlanTopic = { topic: topic, isSelected: this.tempFilteredtopicsList[i].isSelected };
  //              this.filteredtopicsList.push(this.personalizedPlanTopic);
  //            }
  //          }
  //        });
  //        this.getUpdatedPersonalizedPlan(response, isChecked);
  //      }
  //    });
  //}

  //getUpdatedPersonalizedPlan(response, isChecked) {
  //  this.notifyFilterTopics.emit({ plan: response, topicsList: this.filteredtopicsList });
  //  this.loadPersonalizedPlan();
  //  if (isChecked) {
  //    this.isCompleted = true;
  //    this.toastr.success("Step Completed.");
  //  } else {
  //    this.isCompleted = false;
  //    this.toastr.success("Step Added Back");
  //  }
  //}

  //loadPersonalizedPlan() {
  //  this.planDetails.topics = [];
  //  this.filteredtopicsList.forEach(topic => {
  //    if (topic.isSelected) {
  //      this.planDetails.topics.push(topic);
  //    }
  //  });
  //}

  resourceUrl(url) {
    this.url = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    return this.url;
  }

  planTagOptions(topicId) {
    this.getRemovePlanDetails();
    this.planTopics = [];
    if (this.removePlanDetails.length > 0) {
      this.removePlanDetails.forEach(planTopic => {
        this.planTopics.push(planTopic);
      });
    }
    this.personalizedPlan = { id: this.planDetails.id, topics: this.planTopics, isShared: this.planDetails.isShared };
    this.selectedPlanDetails = { planDetails: this.personalizedPlan, topicId: topicId };
    //  this.planTopics = [];
    //  //this.planTopic = { topicId: '', steps: this.personalizedPlanSteps };
    //  this.removePlanDetails = [];
    //  this.getRemovePlanDetails();
    //  if (this.removePlanDetails.length > 0) {
    //    this.removePlanDetails.forEach(item => {
    //      this.personalizedPlanSteps = [];
    //      item.topic.steps.forEach(step => {
    //        this.personalizedPlanStep = {
    //          stepId: step.stepId, title: step.title, description: step.description,
    //          order: step.order, isComplete: step.isComplete, resources: this.personalizedPlanService.getResourceIds(step.resources), topicIds: []
    //        };
    //        this.personalizedPlanSteps.push(this.personalizedPlanStep);
    //      });
    //      //this.planTopic = { topicId: item.topic.topicId, steps: this.personalizedPlanSteps };
    //      //this.planTopics.push(this.planTopic);
    //    });
    //  }
    //  this.personalizedPlan = { id: this.planDetails.id, topics: this.planTopics, isShared: this.planDetails.isShared };
    //  this.selectedPlanDetails = { planDetails: this.personalizedPlan, topicId: topicId };
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
