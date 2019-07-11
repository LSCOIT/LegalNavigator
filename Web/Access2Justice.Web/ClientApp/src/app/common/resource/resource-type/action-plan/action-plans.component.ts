import { Component, EventEmitter, Input, Output, TemplateRef } from "@angular/core";
import { OnChanges } from "@angular/core/src/metadata/lifecycle_hooks";
import { DomSanitizer } from "@angular/platform-browser";
import { BsModalService } from "ngx-bootstrap/modal";
import { BsModalRef } from "ngx-bootstrap/modal/bs-modal-ref.service";
import { ToastrService } from "ngx-toastr";
import { Global, UserStatus } from "../../../../global";
import { PersonalizedPlan, PersonalizedPlanTopic, PlanTopic } from "../../../../guided-assistant/personalized-plan/personalized-plan";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { NavigateDataService } from "../../../services/navigate-data.service";

@Component({
  selector: "app-action-plans",
  templateUrl: "./action-plans.component.html",
  styleUrls: ["./action-plans.component.css"]
})
export class ActionPlansComponent implements OnChanges {
  @Input() planDetails;
  @Input() topicsList;
  displaySteps: boolean = false;
  updatedPlan: any;
  modalRef: BsModalRef;
  url: any;
  safeUrl: any;
  isCompleted: boolean = false;
  isUser: boolean = false;
  isChecked: boolean = false;
  planTopics: Array<PlanTopic>;
  personalizedPlan: PersonalizedPlan = {
    id: "",
    topics: this.planTopics,
    answersDocId: "",
    curatedExperienceId: "",
    isShared: false
  };
  selectedPlanDetails: any;
  tempFilteredtopicsList: Array<PersonalizedPlanTopic> = [];
  @Output() notifyFilterTopics = new EventEmitter<object>();
  removePlanDetails: any;
  plan: any;
  sortOrder: Array<string> = ["isComplete", "order"];
  resourceTypeList = [
    "Forms",
    "Guided Assistant",
    "Organizations",
    "Videos",
    "Articles"
  ];

  constructor(
    private modalService: BsModalService,
    private personalizedPlanService: PersonalizedPlanService,
    public sanitizer: DomSanitizer,
    private toastr: ToastrService,
    private global: Global,
    private navigateDataService: NavigateDataService
  ) {
    this.sanitizer = sanitizer;
    if (
      global.role === UserStatus.Shared &&
      location.pathname.indexOf(global.shareRouteUrl) >= 0
    ) {
      global.showMarkComplete = false;
      global.showDropDown = false;
    } else {
      global.showMarkComplete = true;
      global.showDropDown = true;
    }
  }

  getPersonalizedPlan(planDetails): void {
    this.planDetails = planDetails;
    if (!planDetails || planDetails.length === 0) {
      this.displaySteps = false;
    } else if (this.planDetails.topics) {
      this.sortStepsByOrder(planDetails);
      this.displaySteps = true;
    }
  }

  sortStepsByOrder(planDetails) {
    planDetails.topics.forEach(topic => {
      topic.steps.forEach(step => {
        // Match all URLs outside <a> tags. Next these urls will be wrapped with <a> tag.
        step.description = step.description.replace(/(([--:\w?@%&+~#=]*\.[a-z]{2,4}\/{0,2})((?:[?&](?:\w+)=(?:\w+))+|[\w?@%&+~#=;\/\-]+)?)(?![^<>]*>|[^"]*?<\/a)/g, '<a href=\"$1\">$1</a>');
      });
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
    if (this.global.userId) {
      this.updatePlanToProfile();
    } else {
      this.showStepCompleteStatus();
    }
  }

  updatePlanToProfile() {
    const params = {
      personalizedPlan: this.navigateDataService.getData()
        ? this.navigateDataService.getData()
        : this.personalizedPlanService.planDetails,
      oId: this.global.userId,
      saveActionPlan: true
    };
    this.personalizedPlanService.userPlan(params).subscribe(response => {
      if (response) {
        this.showStepCompleteStatus();
      }
    });
  }

  showStepCompleteStatus() {
    if (this.isChecked) {
      this.personalizedPlanService.showSuccess("Step Completed");
    } else {
      this.personalizedPlanService.showSuccess("Step Added Back");
    }
  }

  resourceUrl(url) {
    this.url = url.replace("watch?v=", "embed/");
    this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
    return this.safeUrl;
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  planTagOptions(topicId) {
    this.getRemovePlanDetails();
    this.planTopics = [];
    if (this.removePlanDetails.length > 0) {
      this.removePlanDetails.forEach(planTopic => {
        this.planTopics.push(planTopic.topic);
      });
    }
    this.personalizedPlan = {
      id: this.planDetails.id,
      topics: this.planTopics,
      answersDocId: this.planDetails.answersDocId,
      curatedExperienceId: this.planDetails.curatedExperienceId,
      isShared: this.planDetails.isShared
    };
    this.selectedPlanDetails = {
      planDetails: this.personalizedPlan,
      topic: topicId
    };
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
