import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  Output,
  TemplateRef,
  ViewChild,
} from "@angular/core";
import { OnChanges } from "@angular/core/src/metadata/lifecycle_hooks";
import { DomSanitizer } from "@angular/platform-browser";
import { BsModalService } from "ngx-bootstrap/modal";
import { BsModalRef } from "ngx-bootstrap/modal/bs-modal-ref.service";
import { ToastrService } from "ngx-toastr";
import { Global, UserStatus } from "../../../../global";
import {
  PersonalizedPlan,
  PersonalizedPlanTopic,
  PlanTopic,
} from "../../../../guided-assistant/personalized-plan/personalized-plan";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { NavigateDataService } from "../../../services/navigate-data.service";
import { Router } from "@angular/router";
import { template } from "@angular/core/src/render3";

@Component({
  selector: "app-action-plans",
  templateUrl: "./action-plans.component.html",
  styleUrls: ["./action-plans.component.css"],
})
export class ActionPlansComponent implements OnChanges {
  @Input() planDetails;
  @Input() topicsList;
  @Input() showUnshareOption: boolean;
  @Input() showRemoveOption: boolean;
  @Input() showRemoveResourceOption: boolean;
  @Input() isSharedActionPlan: boolean;
  @Input() showDropDown: boolean;
  @Input() isIncomingResource: boolean;

  displaySteps = false;
  updatedPlan: any;
  modalRef: BsModalRef;
  url: any;
  safeUrl: any;
  isCompleted = false;
  isUser = false;
  isChecked = false;
  planTopics: Array<PlanTopic>;
  personalizedPlan: PersonalizedPlan = {
    id: "",
    topics: this.planTopics,
    answersDocId: "",
    curatedExperienceId: "",
    isShared: false,
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
    "Articles",
  ];
  videoTemplateContext: any;
  @ViewChild("videoTemplateForInnerHtml") videoTemplateForInnerHtml: ElementRef;
  @HostListener("window:resource", ["$event"])
  onCallResource(param) {
    if (param.detail.type === "Videos") {
      this.videoTemplateContext = {
        resource: { name: param.detail.name, url: param.detail.url },
      };
      this.modalRef = this.modalService.show(this.videoTemplateForInnerHtml);
    } else {
      this.router.navigate(["/resource", param.detail.id]);
    }
  }

  constructor(
    private modalService: BsModalService,
    private personalizedPlanService: PersonalizedPlanService,
    public sanitizer: DomSanitizer,
    private toastr: ToastrService,
    private global: Global,
    private navigateDataService: NavigateDataService,
    private router: Router
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

  public isNeedToShowShareTo = (topic) => {
    return (
      this.isSharedActionPlan &&
      this.showUnshareOption &&
      topic.shared &&
      topic.shared.sharedTo
    );
  };

  getPersonalizedPlan(planDetails): void {
    debugger;
    this.planDetails = planDetails;
    if (!planDetails || planDetails.length === 0) {
      this.displaySteps = false;
    } else if (this.planDetails.topics) {
      this.sortStepsByOrder(planDetails);
      this.displaySteps = true;
    }
  }

  sortStepsByOrder(planDetails) {
    planDetails.topics.forEach((topic) => {
      topic.steps.forEach((step) => {
        debugger;
        // Match all URLs outside <a> tags. Next these urls will be wrapped with <a> tag.
        step.description = step.description.replace(
          /(([--:\w?@%&+~#=]*\.[a-z]{2,4}\/{0,2})((?:[?&](?:\w+)=(?:\w+))+|[\w?@%&+~#=;\/\-]+)?)(?![^<>]*>|[^"]*?<\/a)/g,
          '<a href="$1">$1</a>'
        );
        step.resources.forEach((resource) => {
          // If resource GUID present in description, need replace guid with <a> tag.
          step.description = step.description.replace(
            resource.id,
            this.generateLink(resource)
          );
        });
      });
      topic.steps = this.orderBy(topic.steps, this.sortOrder);
    });
  }
  generateLink(resource) {
    let tag = '<a class="link" ';

    if (
      resource.resourceType === "Forms" ||
      resource.resourceType === "Related Readings" ||
      resource.resourceType === "Related Links"
    ) {
      tag += `href="${resource.url}" target="_blank">${resource.name}</a>`;
    } else {
      var route = `'${window.location.protocol}//${window.location.host}/resource/${resource.id}'`;
      tag += `href="javascript:void(0)" onclick="window.open(${route},'_blank');return false;">${resource.name}</a>`;
    }

    return tag;
  }

  checkCompleted(event, topicId, stepId) {
    this.isChecked = event.target.checked;
    this.planDetails.topics.forEach((topic) => {
      if (topic.topicId === topicId) {
        topic.steps.forEach((step) => {
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
      saveActionPlan: true,
    };
    this.personalizedPlanService.userPlan(params).subscribe((response) => {
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

  openModal(modalTemplate: TemplateRef<any>) {
    this.modalRef = this.modalService.show(modalTemplate);
  }

  planTagOptions(topic) {
    this.fillTopics();
    this.getRemovePlanDetails();
    this.planTopics = [];
    if (this.removePlanDetails.length > 0) {
      this.removePlanDetails.forEach((planTopic) => {
        this.planTopics.push(!planTopic.topic ? planTopic : planTopic.topic);
      });
    }
    this.personalizedPlan = {
      id: this.planDetails.id,
      topics: this.planTopics,
      answersDocId: this.planDetails.answersDocId,
      curatedExperienceId: this.planDetails.curatedExperienceId,
      isShared: this.planDetails.isShared,
    };
    this.selectedPlanDetails = {
      planDetails: this.personalizedPlan,
      topic: topic.topicId,
    };
  }

  getRemovePlanDetails() {
    this.removePlanDetails = [];
    this.tempFilteredtopicsList.forEach((topic) => {
      this.removePlanDetails.push(topic);
    });
  }

  ngOnChanges() {
    this.fillTopics();
  }

  fillTopics() {
    this.getPersonalizedPlan(this.planDetails);
    this.tempFilteredtopicsList = !this.topicsList
      ? this.planDetails.topics
      : this.topicsList;
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
