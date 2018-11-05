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
  planTopics: Array<PlanTopic>;
  personalizedPlan: PersonalizedPlan = { id: '', topics: this.planTopics, isShared: false };
  selectedPlanDetails: any;
  tempFilteredtopicsList: Array<PersonalizedPlanTopic> = [];
  @Output() notifyFilterTopics = new EventEmitter<object>();
  removePlanDetails: any;
  plan: any;
  sortOrder: Array<string> = ["isComplete", "order"];
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
      topic.steps = this.orderBy(topic.steps, this.sortOrder);
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
        topic.steps = this.orderBy(topic.steps, this.sortOrder);
      }
    });
    this.navigateDataService.setData(this.planDetails);
    if (this.isChecked) {
      this.personalizedPlanService.showSuccess("Step Completed");
    } else {
      this.personalizedPlanService.showSuccess("Step Added Back");
    }
  }

  resourceUrl(url) {
    this.url = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    return this.url;
  }

  planTagOptions(topicId) {
    this.getRemovePlanDetails();
    this.planTopics = [];
    if (this.removePlanDetails.length > 0) {
      this.removePlanDetails.forEach(planTopic => {
        this.planTopics.push(planTopic.topic);
      });
    }
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
      if (a[field[0]] < b[field[0]]) {
        return -1;
      } else if (a[field[0]] > b[field[0]]) {
        return 1;
      } else {
        if (a[field[1]] < b[field[1]]) {
          return -1;
        } else if (a[field[1]] > b[field[1]]) {
          return 1;
        }
        return 0;
      }
    });
  }

}
