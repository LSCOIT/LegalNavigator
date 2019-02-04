import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";

import ENV from 'env';
import { LocationDetails, MapLocation } from "../../map/map";
import { MapService } from "../../map/map.service";
import { PaginationService } from "../../pagination/pagination.service";
import { ILuisInput, IResourceFilter } from "../../search/search-results/search-results.model";
import { NavigateDataService } from "../../services/navigate-data.service";

@Component({
  selector: "app-guided-assistant-sidebar",
  templateUrl: "./guided-assistant-sidebar.component.html",
  styleUrls: ["./guided-assistant-sidebar.component.css"]
})
export class GuidedAssistantSidebarComponent implements OnInit, OnDestroy {
  location: MapLocation;
  locationDetails: LocationDetails;
  activeTopic: any;
  @Input() activeSubTopic: any;
  @Input() guidedAssistantId: string;
  @Input() showSidebar: boolean;
  @Input() searchResultsData: any;
  resourceFilter: IResourceFilter = {
    ResourceType: "",
    ContinuationToken: "",
    TopicIds: [],
    ResourceIds: [],
    PageNumber: 0,
    Location: {},
    IsResourceCountRequired: false,
    IsOrder: false,
    OrderByField: "",
    OrderBy: ""
  };
  luisInput: ILuisInput = {
    Sentence: "",
    Location: "",
    TranslateFrom: "",
    TranslateTo: "",
    LuisTopScoringIntent: ""
  };
  topicIds: string[] = [];
  resources: any;
  subscription: any;
  emptyResult = "";
  guidedAssistantResults: any;
  topIntent: string;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private mapService: MapService,
    private navigateDataService: NavigateDataService,
    private paginationService: PaginationService
  ) {}

  getGuidedAssistantResults() {
    this.topicIds = [];
    if (this.activeTopic) {
      this.topicIds.push(this.activeTopic);
      this.resourceFilter = {
        ResourceType: "Guided Assistant",
        ContinuationToken: "",
        TopicIds: this.topicIds,
        ResourceIds: [],
        PageNumber: 0,
        Location: this.location,
        IsResourceCountRequired: false,
        IsOrder: false,
        OrderByField: "",
        OrderBy: ""
      };
      this.paginationService
        .getPagedResources(this.resourceFilter)
        .subscribe(response => {
          if (response != undefined) {
            this.resources = response["resources"];
            if (this.resources.length > 0) {
              this.guidedAssistantId = this.resources[0].curatedExperienceId;
            }
          } else {
            this.guidedAssistantId = this.emptyResult;
          }
        });
    }
    this.guidedAssistantId = this.emptyResult;
  }

  getGuidedAssistantLinkResult() {
    if (this.router.url.startsWith("/search")) {
      this.navigateDataService.setData(this.searchResultsData);
      this.router.navigateByUrl("/guidedassistant/" + this.guidedAssistantId);
    }
    this.resourceFilter = {
      ResourceType: ENV.All,
      TopicIds: this.topicIds,
      Location: this.location,
      PageNumber: 0,
      ContinuationToken: "",
      IsResourceCountRequired: true,
      ResourceIds: [],
      IsOrder: false,
      OrderByField: "",
      OrderBy: ""
    };
    this.paginationService
      .getPagedResources(this.resourceFilter)
      .subscribe(response => {
        if (response != undefined) {
          this.guidedAssistantResults = response;
          this.guidedAssistantResults.topIntent = this.activeSubTopic.name;
          this.navigateDataService.setData(this.guidedAssistantResults);
          this.router.navigateByUrl(
            "/guidedassistant/" + this.guidedAssistantId,
            { skipLocationChange: true }
          );
        }
      });
  }

  ngOnInit() {
    if (!this.guidedAssistantId) {
      if (sessionStorage.getItem("globalMapLocation")) {
        this.locationDetails = JSON.parse(
          sessionStorage.getItem("globalMapLocation")
        );
        this.location = this.locationDetails.location;
        this.activeTopic = this.activeRoute.snapshot.params["topic"];
        this.getGuidedAssistantResults();
      }
      this.subscription = this.mapService.notifyLocation.subscribe(value => {
        this.locationDetails = value;
        this.location = this.locationDetails.location;
        this.activeTopic = this.activeRoute.snapshot.params["topic"];
        this.getGuidedAssistantResults();
      });
    }
  }
  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }
}
