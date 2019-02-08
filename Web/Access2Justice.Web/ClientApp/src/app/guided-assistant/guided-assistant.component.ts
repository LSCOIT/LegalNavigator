import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from "@angular/router";

import {ENV} from 'environment';
import { Global } from "../global";
import { LocationDetails } from "../shared/map/map";
import { ILuisInput } from "../shared/search/search-results/search-results.model";
import { SearchService } from "../shared/search/search.service";
import { NavigateDataService } from "../shared/services/navigate-data.service";
import { StaticResourceService } from "../shared/services/static-resource.service";
import { GuidedAssistant } from "./guided-assistant";

@Component({
  selector: "app-guided-assistant",
  templateUrl: "./guided-assistant.component.html",
  styleUrls: ["./guided-assistant.component.css"]
})
export class GuidedAssistantComponent implements OnInit, OnDestroy {
  searchText: string;
  topicLength = 12;
  luisInput: ILuisInput = {
    Sentence: "",
    Location: "",
    TranslateFrom: "",
    TranslateTo: "",
    LuisTopScoringIntent: ""
  };
  guidedAssistantResults: any;
  locationDetails: LocationDetails;
  previousSearchText: string;
  name = "GuidedAssistantPrivacyPage";
  guidedAssistantPageContent: GuidedAssistant;
  staticContent: any;
  staticContentSubcription: any;
  blobUrl: string = ENV.blobUrl;
  description = "";

  constructor(
    private searchService: SearchService,
    private router: Router,
    private navigateDataService: NavigateDataService,
    private staticResourceService: StaticResourceService,
    private global: Global
  ) {}

  onSubmit(guidedAssistantForm) {
    this.locationDetails = JSON.parse(
      sessionStorage.getItem("globalMapLocation")
    );
    if (guidedAssistantForm.value.searchText) {
      this.luisInput["Sentence"] = guidedAssistantForm.value.searchText;
      this.luisInput["Location"] = this.locationDetails.location;
      this.searchService.search(this.luisInput).subscribe(response => {
        this.guidedAssistantResults = response;
        this.navigateDataService.setData(this.guidedAssistantResults);
        if (
          this.guidedAssistantResults.guidedAssistantId &&
          this.guidedAssistantResults.topicIds.length > 0
        ) {
          sessionStorage.setItem("searchTextResults", JSON.stringify(response));
        }
        sessionStorage.setItem(
          "searchText",
          guidedAssistantForm.value.searchText
        );
        this.router.navigateByUrl("/guidedassistant/search");
      });
    }
  }

  getGuidedAssistantContent(): void {
    if (
      this.staticResourceService.GuidedAssistantPageContent &&
      this.staticResourceService.GuidedAssistantPageContent.location[0].state ==
        this.staticResourceService.getLocation()
    ) {
      this.guidedAssistantPageContent = this.staticResourceService.GuidedAssistantPageContent;
      this.description = this.guidedAssistantPageContent.description;
    } else {
      if (this.global.getData()) {
        this.staticContent = this.global.getData();
        this.guidedAssistantPageContent = this.staticContent.find(
          x => x.name === this.name
        );
        this.staticResourceService.GuidedAssistantPageContent = this.guidedAssistantPageContent;
        this.description = this.guidedAssistantPageContent.description;
      }
    }
  }

  ngOnInit() {
    if (JSON.parse(sessionStorage.getItem("searchTextResults")) != null) {
      this.navigateDataService.setData(
        JSON.parse(sessionStorage.getItem("searchTextResults"))
      );
      this.router.navigateByUrl("/guidedassistant/search");
    } else {
      if (sessionStorage.getItem("searchText")) {
        this.previousSearchText = sessionStorage.getItem("searchText");
      }
    }
    this.getGuidedAssistantContent();
    this.staticContentSubcription = this.global.notifyStaticData.subscribe(
      () => {
        this.getGuidedAssistantContent();
      }
    );
  }

  ngOnDestroy() {
    if (this.staticContentSubcription) {
      this.staticContentSubcription.unsubscribe();
    }
  }
}
