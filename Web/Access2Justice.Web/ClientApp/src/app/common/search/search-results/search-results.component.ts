import { Component, Input, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Subscription } from "rxjs";
import { map } from "rxjs/operators";
import { NgxSpinnerService } from "ngx-spinner";

import { ENV } from "environment";
import { Global } from "../../../global";
import { PersonalizedPlanService } from "../../../guided-assistant/personalized-plan/personalized-plan.service";
import { LocationDetails } from "../../map/map";
import { MapService } from "../../map/map.service";
import { PaginationService } from "../../pagination/pagination.service";
import { NavigateDataService } from "../../services/navigate-data.service";
import { SearchService } from "../search.service";
import { ResourceResult } from "./search-result";
import { ILuisInput, IResourceFilter } from "./search-results.model";
import { ParamsChange } from "../search-filter/search-filter.component";

@Component({
  selector: "app-search-results",
  templateUrl: "./search-results.component.html",
  styleUrls: ["./search-results.component.css"],
})
export class SearchResultsComponent implements OnInit, OnDestroy {
  @Input() fullPage = false;
  @Input() searchResults: any;
  @Input() showRemove: boolean;
  isInternalResource: boolean;
  isWebResource: boolean;
  searchText: string;
  sortType: any;
  resourceResults: ResourceResult[] = [];
  filterType: string = ENV.All;
  resourceTypeFilter: any[];
  resourceFilter: IResourceFilter = {
    ResourceType: "",
    ContinuationToken: "",
    TopicIds: [],
    ResourceIds: [],
    PageNumber: 0,
    Location: {},
    IsResourceCountRequired: false,
    IsOrder: true,
    OrderByField: "name",
    OrderBy: "ASC",
    url: "",
    sharedTo: [],
  };
  luisInput: ILuisInput = {
    Sentence: "",
    Location: {},
    LuisTopScoringIntent: "",
    TranslateFrom: "",
    TranslateTo: "",
    OrderByField: "name",
    OrderBy: "ASC",
  };
  location: any;
  topicIds: any[];
  isServiceCall: boolean;
  currentPage = 0;
  showRemoveOption: boolean;
  displayMessage = false;
  total = 0;
  page = 1;
  limit = 0;
  offset = 0;
  pagesToShow = 0;
  topIntent: string;
  initialResourceFilter: string;
  showDefaultMessage = false;
  showNoResultsMessage = false;
  guidedAssistantId: string;
  locationDetails: LocationDetails;
  orderBy: string;
  showMap: boolean;

  private locationSub: Subscription;

  constructor(
    private navigateDataService: NavigateDataService,
    private searchService: SearchService,
    private mapService: MapService,
    private paginationService: PaginationService,
    private personalizedPlanService: PersonalizedPlanService,
    private global: Global,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  ngOnInit() {
    if (sessionStorage.getItem("bookmarkedResource")) {
      this.route.data.pipe(map((data) => data.cres)).subscribe((response) => {
        if (response) {
          this.global.setProfileData(
            response.oId,
            response.name,
            response.eMail,
            response.roleInformation
          );
          this.personalizedPlanService.saveResourcesToUserProfile();
        }
      });
    }

    if (
      !this.navigateDataService.getData() &&
      sessionStorage.getItem("cacheSearchResults")
    ) {
      this.navigateDataService.setData(
        JSON.parse(sessionStorage.getItem("cacheSearchResults"))
      );
    }
    this.bindData();
    this.notifyLocationChange();
    this.showRemoveOption = this.showRemove;
    if (this.searchResults && this.searchResults.searchFilter) {
      if (
        this.searchResults.searchFilter.OrderByField === "modifiedTimeStamp"
      ) {
        this.searchResults.searchFilter.OrderByField = "date";
      }
      this.global.searchResultDetails.sortParam = this.searchResults.searchFilter.OrderByField;
      this.global.searchResultDetails.order = this.searchResults.searchFilter.OrderBy;
      this.global.searchResultDetails.filterParam = !this.searchResults
        .resourceType
        ? ENV.All
        : this.searchResults.resourceType;
    } else {
      this.sortType = {
        filterParam: undefined,
        sortParam: "date",
        order: "DESC",
      };
    }
  }

  bindData() {
    this.showDefaultMessage = false;
    this.showNoResultsMessage = false;
    this.searchResults = this.navigateDataService.getData();

    if (this.searchResults) {
      this.searchResults.resources = this.searchResults.resources;
      this.guidedAssistantId = this.searchResults.guidedAssistantId;
      this.cacheSearchResultsData();
      this.isInternalResource = this.searchResults.resources;
      this.isWebResource = this.searchResults.webResources;
      if (this.isWebResource) {
        if (this.searchResults.webResources.webPages === undefined) {
          this.showNoResultsMessage = true;
          this.isWebResource = false;
        } else {
          this.mapWebResources();
        }
      } else {
        this.topIntent = this.searchResults.topIntent;
        if (this.searchResults.resources.length > 0) {
          this.mapInternalResource();
        } else {
          this.isInternalResource = false;
          this.showNoResultsMessage = true;
        }
      }
    } else {
      this.showDefaultMessage = true;
    }
  }

  mapInternalResource() {
    this.resourceTypeFilter = this.searchResults.resourceTypeFilter;
    if (this.searchResults.resourceType) {
      this.initialResourceFilter = this.searchResults.resourceType;
      this.filterType = this.searchResults.resourceType;
    } else {
      this.filterType = ENV.All;
    }
    this.resourceResults = this.searchResults.resourceTypeFilter;
    this.navigateDataService.setData(undefined);
    if (this.resourceTypeFilter != undefined) {
      for (let index = 0; index < this.resourceTypeFilter.length; index++) {
        if (
          (this.searchResults.isItFromTopicPage &&
            this.resourceTypeFilter[index].ResourceName ===
              this.searchResults.resourceType) ||
          (this.resourceTypeFilter[index].ResourceName === this.filterType &&
            !this.searchResults.isItFromTopicPage)
        ) {
          this.resourceTypeFilter[index]["ResourceList"] = [
            {
              resources: this.searchResults.resources,
              continuationToken: this.searchResults.continuationToken,
            },
          ];
          this.topicIds = this.searchResults.topicIds;
          this.total = this.resourceTypeFilter[index].ResourceCount;
          this.pagesToShow = ENV.internalResourcePagesToShow;
          this.limit = ENV.internalResourceRecordsToDisplay;
          break;
        }
      }
    }
  }

  mapWebResources() {
    this.total = this.searchResults.webResources.webPages.totalEstimatedMatches;
    this.searchText = this.searchResults.webResources.queryContext.originalQuery;
    this.pagesToShow = ENV.webResourcePagesToShow;
    this.limit = ENV.webResourceRecordsToDisplay;
  }

  cacheSearchResultsData() {
    sessionStorage.removeItem("cacheSearchResults");
    sessionStorage.setItem(
      "cacheSearchResults",
      JSON.stringify(this.searchResults)
    );
    if (this.location) {
      sessionStorage.setItem(
        "searchedLocationMap",
        JSON.stringify(this.locationDetails)
      );
    }
  }

  updateCacheStorage(filterName: string) {
    if (sessionStorage.getItem("cacheSearchResults")) {
      const sessionData = JSON.parse(
        sessionStorage.getItem("cacheSearchResults")
      );
      sessionData.resources = this.searchResults.resources;
      sessionData.resourceType = filterName;
      sessionStorage.setItem("cacheSearchResults", JSON.stringify(sessionData));
    }
  }

  filterSearchResults(event: ParamsChange) {
    this.sortType = event;
    if (!event.filterParam) {
      this.sortType.filterParam = "All";
    }
    if (this.isInternalResource && event) {
      this.page = 1;
      this.currentPage = 0;
    }

    const pageNumber =
      event.filterParam === this.sortType.filterParam ? this.currentPage : 0;

    this.global.searchResultDetails.filterParam = this.sortType.filterParam;
    this.global.searchResultDetails.sortParam = this.sortType.sortParam;
    this.global.searchResultDetails.order = this.sortType.order;

    this.getInternalResource(event.filterParam, pageNumber);
  }

  notifyLocationChange() {
    if (sessionStorage.getItem("searchedLocationMap")) {
      this.locationDetails = JSON.parse(
        sessionStorage.getItem("searchedLocationMap")
      );
      this.location = this.locationDetails.location;
    }
    this.locationSub = this.mapService.notifyLocalLocation.subscribe(
      (value) => {
        this.location = value;
        if (this.searchResults.isItFromTopicPage) {
          this.filterResourceByTopicAndLocation();
        } else {
          this.filterResourceByKeywordAndLocation();
        }
      }
    );
  }

  filterResourceByTopicAndLocation() {
    this.resourceFilter = {
      ResourceType: ENV.All,
      TopicIds: this.searchResults.topicIds,
      Location: this.location,
      PageNumber: 0,
      ContinuationToken: "",
      IsResourceCountRequired: true,
      ResourceIds: [],
      IsOrder: true,
      OrderByField: this.sortType.sortParam,
      OrderBy: this.orderBy,
      url: "",
      sharedTo: [],
    };
    this.paginationService
      .getPagedResources(this.resourceFilter)
      .subscribe((response) => {
        if (response != undefined) {
          this.searchResults = response;
          this.searchResults.topIntent = this.topIntent;
          this.searchResults.isItFromTopicPage = true;
          this.searchResults.resourceType = ENV.All;
          this.displayFetchedResources();
        }
      });
  }

  filterResourceByKeywordAndLocation() {
    this.luisInput.Location = this.location;
    this.luisInput.LuisTopScoringIntent = this.topIntent;
    this.luisInput.Sentence = this.topIntent;
    this.luisInput.OrderByField = this.global.searchResultDetails.sortParam;
    this.luisInput.OrderBy = this.global.searchResultDetails.order;
    this.searchService.search(this.luisInput).subscribe((response) => {
      if (response["resources"].length) {
        this.searchResults = response;
        this.displayFetchedResources();
      } else {
        this.displayNoResultsMessage();
      }
    });
  }

  displayFetchedResources() {
    this.navigateDataService.setData(this.searchResults);
    this.cacheSearchResultsData();
    this.isInternalResource = true;
    this.mapInternalResource();
    const event = { filterParam: ENV.All };
    this.filterSearchResults(event);
  }

  displayNoResultsMessage() {
    this.displayMessage = true;
    this.isInternalResource = false;
  }

  getInternalResource(filterName, pageNumber): void {
    this.checkResource(filterName, pageNumber);
    if (this.isServiceCall) {
      if (sessionStorage.getItem("searchedLocationMap")) {
        this.locationDetails = JSON.parse(
          sessionStorage.getItem("searchedLocationMap")
        );
      } else {
        this.locationDetails = JSON.parse(
          sessionStorage.getItem("globalMapLocation")
        );
      }
      this.resourceFilter.Location = this.locationDetails.location;
      this.resourceFilter.OrderBy = this.sortType.order;
      this.resourceFilter.OrderByField = this.sortType.sortParam;
      this.paginationService
        .getPagedResources(this.resourceFilter)
        .subscribe((response) => {
          this.searchResults = response;
          if (this.searchResults.resources) {
            this.searchResults.resources = this.searchResults.resources.sort(
              (n1, n2) => {
                if (n1.ranking > n2.ranking) {
                  return 1;
                }

                if (n1.ranking < n2.ranking) {
                  return -1;
                }

                return n1.name > n2.name ? 1 : -1;
              }
            );
          }
          if (this.page === 1) {
            this.updateCacheStorage(filterName);
          }
          this.addResource(filterName);
        });
    }
  }

  checkResource(resourceName, pageNumber): void {
    for (let index = 0; index < this.resourceTypeFilter.length; index++) {
      if (
        this.resourceTypeFilter[index].ResourceName === resourceName &&
        this.resourceTypeFilter[index].ResourceList != undefined &&
        this.resourceTypeFilter[index].ResourceList[this.page - 1] != undefined
      ) {
        this.total = this.resourceTypeFilter[index].ResourceCount;
        this.searchResults = this.resourceTypeFilter[index].ResourceList[
          this.page - 1
        ];
        this.searchResults.topicIds = this.topicIds;
        if (this.page === 1) {
          this.updateCacheStorage(resourceName);
        }
        this.isServiceCall = false;
        break;
      } else if (this.resourceTypeFilter[index].ResourceName === resourceName) {
        this.total = this.resourceTypeFilter[index].ResourceCount;
        this.resourceFilter.ResourceType = this.resourceTypeFilter[
          index
        ].ResourceName;
        this.resourceFilter.PageNumber = this.page - 1;
        this.resourceFilter.TopicIds = this.topicIds;
        if (
          this.resourceTypeFilter[index].ResourceList != undefined &&
          this.resourceTypeFilter[index].ResourceList[pageNumber] != undefined
        ) {
          this.resourceFilter.ContinuationToken = JSON.stringify(
            this.resourceTypeFilter[index].ResourceList[pageNumber][
              "continuationToken"
            ]
          );
        }
        this.isServiceCall = true;
        break;
      }
    }
  }

  addResource(filterName) {
    for (let index = 0; index < this.resourceTypeFilter.length; index++) {
      if (this.resourceTypeFilter[index].ResourceName === filterName) {
        if (this.resourceTypeFilter[index]["ResourceList"] == undefined) {
          this.resourceTypeFilter[index]["ResourceList"] = [
            {
              resources: this.searchResults.resources,
              continuationToken: this.searchResults.continuationToken,
            },
          ];
        } else {
          this.resourceTypeFilter[index]["ResourceList"].push({
            resources: this.searchResults.resources,
            continuationToken: this.searchResults.continuationToken,
          });
        }
      }
    }
  }

  searchResource(offset: number): void {
    this.spinner.show();
    this.paginationService.searchByOffset(this.searchText, offset).subscribe(
      (response) => {
        if (response != undefined) {
          this.searchResults = response;
        }
        window.scrollTo(0, 0);
        this.spinner.hide();
      },
      (error) => {
        this.spinner.hide();
        this.router.navigate(["/error"]);
      }
    );
  }

  goToPage(n: number): void {
    if (this.page < n) {
      this.currentPage = this.page;
    } else {
      this.currentPage = n;
    }
    this.global.searchResultDetails.topIntent = this.searchResults.topIntent;
    this.page = n;
    if (this.isWebResource) {
      this.searchResource(this.calculateOffsetValue(n));
    } else {
      if (!this.sortType) {
        this.sortType = {
          filterParam: this.global.searchResultDetails.filterParam,
          sortParam: this.global.searchResultDetails.sortParam,
          order: this.global.searchResultDetails.order,
        };
      }
      this.getInternalResource(
        this.global.searchResultDetails.filterParam,
        this.currentPage - 1
      );
    }
  }

  onNext(): void {
    this.currentPage = this.page;
    this.page++;
    if (this.isWebResource) {
      this.searchResource(this.calculateOffsetValue(this.page));
    } else {
      if (!this.sortType) {
        this.sortType = {
          filterParam: this.global.searchResultDetails.filterParam,
          sortParam: this.global.searchResultDetails.sortParam,
          order: this.global.searchResultDetails.order,
        };
      }
      this.getInternalResource(
        this.global.searchResultDetails.filterParam,
        this.currentPage - 1
      );
    }
  }

  onPrev(): void {
    this.currentPage = this.page;
    this.page--;
    if (this.isWebResource) {
      this.searchResource(this.calculateOffsetValue(this.page));
    } else {
      if (!this.sortType) {
        this.sortType = {
          filterParam: this.global.searchResultDetails.filterParam,
          sortParam: this.global.searchResultDetails.sortParam,
          order: this.global.searchResultDetails.order,
        };
      }
      this.getInternalResource(
        this.global.searchResultDetails.filterParam,
        this.currentPage - 1
      );
    }
  }

  calculateOffsetValue(pageNumber: number): number {
    return Math.ceil(pageNumber * 10) + 1;
  }

  receiveMapEvent(event) {
    this.showMap = event;
  }

  ngOnDestroy() {
    if (this.locationSub) {
      this.locationSub.unsubscribe();
    }
  }
}
