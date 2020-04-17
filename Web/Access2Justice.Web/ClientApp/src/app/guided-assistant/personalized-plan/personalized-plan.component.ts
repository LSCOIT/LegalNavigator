import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";

// import {ENV} from 'environment';
import { Global } from "../../global";
import { NavigateDataService } from "../../common/services/navigate-data.service";
import { StaticResourceService } from "../../common/services/static-resource.service";
import {
  PersonalizedPlan,
  PersonalizedPlanDescription,
  PersonalizedPlanTopic,
} from "../personalized-plan/personalized-plan";
import { PersonalizedPlanService } from "../personalized-plan/personalized-plan.service";

@Component({
  selector: "app-personalized-plan",
  templateUrl: "./personalized-plan.component.html",
  styleUrls: ["./personalized-plan.component.css"],
})
export class PersonalizedPlanComponent implements OnInit {
  activeActionPlan = this.activeRoute.snapshot.params["id"];
  personalizedPlan: PersonalizedPlan;
  topics: Array<any> = [];
  planTopic: PersonalizedPlanTopic;
  topicsList: Array<PersonalizedPlanTopic> = [];
  tempTopicsList: Array<PersonalizedPlanTopic> = [];
  planDetails: any = [];
  planDetailTags: any;
  type = "Plan";
  name = "PersonalizedActionPlanPage";
  personalizedPlanContent: PersonalizedPlanDescription;
  staticContent: any;
  staticContentSubcription: any;
  // blobUrl: string = ENV.blobUrl;
  description = "";

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    private activeRoute: ActivatedRoute,
    private navigateDataService: NavigateDataService,
    private staticResourceService: StaticResourceService,
    private global: Global
  ) {}

  getTopics(): void {
    this.personalizedPlan = this.navigateDataService.getData();
    if (this.personalizedPlan) {
      this.topics = this.personalizedPlan.topics;
      this.planDetailTags = this.personalizedPlan;
      this.setPlan();
    } else {
      this.personalizedPlanService
        .getActionPlanConditions(this.activeActionPlan)
        .subscribe(plan => {
          if (plan) {
            this.topics = plan.topics;
            this.planDetailTags = plan;
          }
          this.setPlan();
        });
    }
  }

  setPlan() {
    this.topicsList = this.personalizedPlanService.createTopicsList(
      this.topics
    );
    this.planDetails = this.personalizedPlanService.getPlanDetails(
      this.topics,
      this.planDetailTags
    );
  }

  filterPlan(topic) {
    this.tempTopicsList = this.topicsList;
    this.filterTopicsList(topic);
    this.planDetails = this.personalizedPlanService.displayPlanDetails(
      this.planDetailTags,
      this.topicsList
    );
  }

  filterTopicsList(topic) {
    this.topicsList = [];
    this.tempTopicsList.forEach((topicDetail) => {
      if (topicDetail.topic.name === topic) {
        this.planTopic = {
          topic: topicDetail.topic,
          isSelected: !topicDetail.isSelected,
        };
      } else {
        this.planTopic = {
          topic: topicDetail.topic,
          isSelected: topicDetail.isSelected,
        };
      }
      this.topicsList.push(this.planTopic);
    });
  }

  filterTopics(event) {
    this.topics = event.plan.topics;
    this.planDetailTags = event.plan;
    this.topicsList = event.topicsList;
    this.filterPlan("");
  }

  getPersonalizedPlanHeading(): void {
    if (
      this.staticResourceService.PersonalizedPlanDescription &&
      this.staticResourceService.PersonalizedPlanDescription.location[0]
        .state == this.staticResourceService.getLocation()
    ) {
      this.personalizedPlanContent = this.staticResourceService.PersonalizedPlanDescription;
      this.description = this.personalizedPlanContent.description;
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.personalizedPlanContent = this.staticContent.find(
          (x) => x.name === this.name
        );
        this.staticResourceService.PersonalizedPlanDescription = this.personalizedPlanContent;
        this.description = this.personalizedPlanContent.description;
      }
    }
  }

  ngOnInit() {
    this.getTopics();
    this.getPersonalizedPlanHeading();
    this.staticContentSubcription = this.global.notifyStaticData.subscribe(
      () => {
        this.getPersonalizedPlanHeading();
      }
    );
  }

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
