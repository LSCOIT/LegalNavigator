import { Location } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { Global } from "../../global";
import { LocationDetails } from "../../common/map/map";
import { MapService } from "../../common/map/map.service";
import { ArrayUtilityService } from "../../common/services/array-utility.service";
import { NavigateDataService } from "../../common/services/navigate-data.service";
import { IntentInput } from "../personalized-plan/personalized-plan";
import { PersonalizedPlanService } from "../personalized-plan/personalized-plan.service";

@Component({
  selector: "app-curated-experience-result",
  templateUrl: "./curated-experience-result.component.html",
  styleUrls: ["./curated-experience-result.component.css"]
})
export class CuratedExperienceResultComponent implements OnInit {
  guidedAssistantResults;
  relevantIntents: Array<string>;
  subscription: any;
  savedTopic = { intent: "", isSelected: false };
  savedTopics: any = [];
  intentInput: IntentInput;
  locationDetails: LocationDetails;
  locationSubscription: any;

  constructor(
    private navigateDataService: NavigateDataService,
    private toastr: ToastrService,
    private router: Router,
    private mapService: MapService,
    private arrayUtilityService: ArrayUtilityService,
    private global: Global,
    private personalizedPlanService: PersonalizedPlanService,
    private location: Location
  ) {}

  saveForLater() {
    if (this.global.userId) {
      this.locationDetails = JSON.parse(
        sessionStorage.getItem("globalMapLocation")
      );
      this.intentInput = {
        location: this.locationDetails.location,
        intents: this.savedTopics
      };
      this.personalizedPlanService.saveTopicsFromGuidedAssistantToProfile(
        this.intentInput,
        true
      );
    } else {
      sessionStorage.setItem(
        this.global.topicsSessionKey,
        JSON.stringify(this.savedTopics)
      );
      this.toastr.success(
        "Topics added to session. You can view them later once you login"
      );
    }
  }

  onSelectTopic(event, intent) {
    this.savedTopic = { intent: intent, isSelected: event.target.checked };
    if (
      this.savedTopic.isSelected &&
      !this.arrayUtilityService.checkObjectExistInArray(
        this.savedTopics,
        this.savedTopic.intent
      )
    ) {
      this.savedTopics.push(this.savedTopic.intent);
    } else {
      this.savedTopics.splice(
        this.savedTopics.indexOf(this.savedTopic.intent),
        1
      );
    }
  }

  filterIntent() {
    if (this.guidedAssistantResults && this.guidedAssistantResults.relevantIntents) {
      this.relevantIntents = this.guidedAssistantResults.relevantIntents.filter(
        resource => resource !== "None"
      );
    }
  }
  
  backToSearch() {
    sessionStorage.removeItem("searchTextResults");
    this.router.navigateByUrl("/guidedassistant");
  }

  ngOnInit() {
    this.locationSubscription = this.location.subscribe(x => {
      if (x.type === "popstate") {
        this.backToSearch();
      }
    });
    this.subscription = this.mapService.notifyLocation.subscribe(value => {
      sessionStorage.removeItem("searchTextResults");
      this.router.navigateByUrl("/guidedassistant");
    });

    this.guidedAssistantResults = this.navigateDataService.getData();
    this.filterIntent();
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    if (this.locationSubscription) {
      this.locationSubscription.unsubscribe();
    }
  }
}
