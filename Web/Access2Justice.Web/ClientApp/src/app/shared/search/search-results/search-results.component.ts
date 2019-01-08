import { Component, OnInit, Input, OnChanges, SimpleChanges, SimpleChange } from '@angular/core';
import { NavigateDataService } from '../../navigate-data.service';
import { ResourceResult } from './search-result';
import { SearchService } from '../search.service';
import { PaginationService } from '../../pagination/pagination.service';
import { IResourceFilter, ILuisInput } from './search-results.model';
import { MapService } from '../../map/map.service';
import { environment } from '../../../../environments/environment';
import { PersonalizedPlanService } from '../../../guided-assistant/personalized-plan/personalized-plan.service';
import { ActivatedRoute } from '@angular/router';
import { Global } from '../../../global';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { LocationDetails } from '../../map/map';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit, OnChanges {
  @Input() fullPage = false;
  isInternalResource: boolean;
  isWebResource: boolean;
  isLuisResponse: boolean;
  searchText: string;
  @Input() searchResults: any;
  uniqueResources: any;
  sortType: any;
  resourceResults: ResourceResult[] = [];
  filterType: string = environment.All;
  resourceTypeFilter: any[];
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: [], ResourceIds: [], PageNumber: 0, Location: {}, IsResourceCountRequired: false, IsOrder: true, OrderByField: 'date', OrderBy: 'DESC' };
  luisInput: ILuisInput = { Sentence: '', Location: {}, LuisTopScoringIntent: '', TranslateFrom: '', TranslateTo: '', OrderByField: 'date', OrderBy: 'DESC' };
  location: any;
  topicIds: any[];
  isServiceCall: boolean;
  currentPage: number = 0;
  @Input()
  personalizedResources: any;
  isPersonalizedresource: boolean;
  subscription: any;
  showRemoveOption: boolean;
  @Input() showRemove: boolean;
  initialResourceLength: number;
  displayMessage: boolean = false;
  loading = false;
  total = 0;
  page = 1;
  limit = 0;
  offset = 0;
  pagesToShow = 0;
  topIntent: string;
  initialResourceFilter: string;
  showDefaultMessage: boolean = false;
  showNoResultsMessage: boolean = false;
  guidedAssistantId: string;
  locationDetails: LocationDetails;
  orderBy: string;
  searchResultDetails: any = { filterParam: 'All', sortParam: 'date', order: 'DESC', topIntent: '' };
  isBindData: boolean;

  constructor(
    private navigateDataService: NavigateDataService,
    private searchService: SearchService,
    private mapService: MapService,
    private paginationService: PaginationService,
    private personalizedPlanService: PersonalizedPlanService,
    private global: Global,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private router: Router) {

    if (sessionStorage.getItem("bookmarkedResource")) {
      this.route.data.map(data => data.cres)
        .subscribe(response => {
          if (response) {
            this.global.setProfileData(response.oId, response.name, response.eMail, response.roleInformation);
            this.personalizedPlanService.saveResourcesToUserProfile();
          }
        });
    }
  }

  bindData() {
    this.showDefaultMessage = false;
    this.showNoResultsMessage = false;
    this.searchResults = this.navigateDataService.getData();
    if (this.searchResults != undefined && this.personalizedResources === undefined) {
      this.guidedAssistantId = this.searchResults.guidedAssistantId;
      this.cacheSearchResultsData();
      this.isInternalResource = this.searchResults.resources;
      this.isWebResource = this.searchResults.webResources;
      if (this.isWebResource) {
        if (this.searchResults.webResources.webPages === undefined) {
          this.showNoResultsMessage = true;
          this.isWebResource = false;
        }
        else {
          this.mapWebResources();
        }
      }
      else {
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
    if (this.personalizedResources != undefined) {
      this.showDefaultMessage = false;
      this.mapPersonalizedResource(this.personalizedResources);
    }
  }

  mapPersonalizedResource(personalizedResources) {
    this.searchResults = { resources: [] }
    this.searchResults.resources = personalizedResources.resources
      .concat(
        personalizedResources.topics,
        personalizedResources.webResources);
    this.isPersonalizedresource = this.searchResults;
    this.applyFilter();
  }

  mapInternalResource() {
    this.resourceTypeFilter = this.searchResults.resourceTypeFilter;
    if (this.searchResults.resourceType) {
      this.initialResourceFilter = this.searchResults.resourceType;
      this.filterType = this.searchResults.resourceType;
    } else {
      this.filterType = environment.All;
    }
    this.resourceResults = this.searchResults.resourceTypeFilter;
    this.navigateDataService.setData(undefined);
    if (this.resourceTypeFilter != undefined) {
      for (let index = 0; index < this.resourceTypeFilter.length; index++) {
        if ((this.searchResults.isItFromTopicPage &&
          this.resourceTypeFilter[index].ResourceName === this.searchResults.resourceType)
          || (this.resourceTypeFilter[index].ResourceName === this.filterType
            && !this.searchResults.isItFromTopicPage)) {
          this.resourceTypeFilter[index]["ResourceList"] = [{
            'resources': this.searchResults.resources,
            'continuationToken': this.searchResults.continuationToken
          }];
          this.topicIds = this.searchResults.topicIds;
          this.total = this.resourceTypeFilter[index].ResourceCount;
          this.pagesToShow = environment.internalResourcePagesToShow;
          this.limit = environment.internalResourceRecordsToDisplay;
          break;
        }
      }
    }
  }

  mapWebResources() {
    this.total = this.searchResults.webResources.webPages.totalEstimatedMatches;
    this.searchText = this.searchResults.webResources.queryContext.originalQuery;
    this.pagesToShow = environment.webResourcePagesToShow;
    this.limit = environment.webResourceRecordsToDisplay;
  }

  cacheSearchResultsData() {
    sessionStorage.removeItem("cacheSearchResults");
    sessionStorage.setItem("cacheSearchResults", JSON.stringify(this.searchResults));
    if (this.location) {
      sessionStorage.setItem("searchedLocationMap", JSON.stringify(this.locationDetails));
    }
  }

  updateCacheStorage(filterName: string) {
    if (sessionStorage.getItem("cacheSearchResults")) {
      let sessionData = JSON.parse(sessionStorage.getItem("cacheSearchResults"));
      sessionData.resources = this.searchResults.resources;
      sessionData.resourceType = filterName;
      sessionStorage.setItem("cacheSearchResults", JSON.stringify(sessionData));
    }
  }

  filterSearchResults(event) {
    this.sortType = event;
    if (event.filterParam == undefined) {
      this.sortType.filterParam = "All";
    }
    if (this.isInternalResource && event != undefined) {
      this.page = 1;
      this.currentPage = 0;
    }
    console.log("this.sortType.filterParam", this.sortType.filterParam);
    console.log("this.global.searchResultDetails.filterParam", this.global.searchResultDetails.filterParam);
    if (!this.isPersonalizedresource) {
      if (this.sortType.sortParam !== this.global.searchResultDetails.sortParam ||
        this.sortType.order !== this.global.searchResultDetails.order) {
        if (this.sortType.filterParam === this.global.searchResultDetails.filterParam) {
          this.searchResultDetails = event;
          this.global.searchResultDetails.sortParam = this.searchResultDetails.sortParam;
          this.global.searchResultDetails.order = this.searchResultDetails.order;
          this.global.searchResultDetails.filterParam = "All";
          this.isBindData = true;
          if (sessionStorage.getItem("searchedLocationMap")) {
            this.locationDetails = JSON.parse(sessionStorage.getItem("searchedLocationMap"));
          } else {
            this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
          }
          this.luisInput.Location = this.locationDetails.location;
          this.searchResults.topIntent = this.topIntent;
          this.luisInput.LuisTopScoringIntent = this.searchResults.topIntent;
          this.luisInput.Sentence = this.searchResults.topIntent
            ? this.searchResults.topIntent
            : this.global.searchResultDetails.topIntent;
          this.luisInput.OrderByField = this.global.searchResultDetails.sortParam;
          this.luisInput.OrderBy = this.global.searchResultDetails.order;
          this.searchService.search(this.luisInput)
            .subscribe(response => {
              this.spinner.hide();
              if (response != undefined) {
                this.searchResults = response;
                this.navigateDataService.setData(this.searchResults);
                this.router.navigateByUrl('/searchRefresh', { skipLocationChange: true })
                  .then(() =>
                    this.router.navigate(['/search'])
                  );
              }
            });
        } else {
          this.global.searchResultDetails.filterParam = event.filterParam;
          this.getInternalResource(event.filterParam, 0);
        }
      } else {
        this.global.searchResultDetails.filterParam = event.filterParam;
        this.getInternalResource(event.filterParam, this.currentPage);
      }
    }
  }

  notifyLocationChange() {
    if (sessionStorage.getItem("searchedLocationMap")) {
      this.locationDetails = JSON.parse(sessionStorage.getItem("searchedLocationMap"));;
      this.location = this.locationDetails.location;
    }
    this.subscription = this.mapService.notifyLocalLocation.subscribe((value) => {
      this.location = value;
      if (this.searchResults.isItFromTopicPage) {
        this.filterResourceByTopicAndLocation();
      } else {
        this.filterResourceByKeywordAndLocation();
      }
    });
  }

  filterResourceByTopicAndLocation() {
    this.resourceFilter = {
      ResourceType: environment.All, TopicIds: this.searchResults.topicIds, Location: this.location,
      PageNumber: 0, ContinuationToken: '', IsResourceCountRequired: true, ResourceIds: [], IsOrder: true, OrderByField: this.sortType.sortParam, OrderBy: this.orderBy
    }
    this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
      if (response != undefined) {
        this.searchResults = response;
        this.searchResults.topIntent = this.topIntent;
        this.searchResults.isItFromTopicPage = true;
        this.searchResults.resourceType = environment.All;
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
    this.searchService.search(this.luisInput)
      .subscribe(response => {
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
    let event = { filterParam: environment.All };
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
        this.locationDetails = JSON.parse(sessionStorage.getItem("searchedLocationMap"));
      } else {
        this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      }
      this.resourceFilter.Location = this.locationDetails.location;
      this.resourceFilter.OrderBy = this.sortType.order;
      this.resourceFilter.OrderByField = this.sortType.sortParam;
      this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
        this.searchResults = response;
        if (this.page === 1) {
          this.updateCacheStorage(filterName);
        }
        this.addResource(filterName);
      });
    }
  }

  checkResource(resourceName, pageNumber): void {
    for (let index = 0; index < this.resourceTypeFilter.length; index++) {
      if (this.resourceTypeFilter[index].ResourceName === resourceName
        && this.resourceTypeFilter[index].ResourceList != undefined
        && this.resourceTypeFilter[index].ResourceList[this.page - 1] != undefined) {
        this.total = this.resourceTypeFilter[index].ResourceCount;
        this.searchResults = this.resourceTypeFilter[index].ResourceList[this.page - 1];
        if (this.page === 1) {
          this.updateCacheStorage(resourceName);
        }
        this.isServiceCall = false;
        break;
      }
      else if (this.resourceTypeFilter[index].ResourceName === resourceName) {
        this.total = this.resourceTypeFilter[index].ResourceCount;
        this.resourceFilter.ResourceType = this.resourceTypeFilter[index].ResourceName;
        this.resourceFilter.PageNumber = this.page - 1;
        this.resourceFilter.TopicIds = this.topicIds;
        if (this.resourceTypeFilter[index].ResourceList != undefined
          && this.resourceTypeFilter[index].ResourceList[pageNumber] != undefined) {
          this.resourceFilter.ContinuationToken = JSON.stringify(this.resourceTypeFilter[index].ResourceList[pageNumber]["continuationToken"]);
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
          this.resourceTypeFilter[index]["ResourceList"] = [{
            'resources': this.searchResults.resources,
            'continuationToken': this.searchResults.continuationToken
          }];
        } else {
          this.resourceTypeFilter[index]["ResourceList"].push({
            'resources': this.searchResults.resources,
            'continuationToken': this.searchResults.continuationToken
          });
        }
      }
    }
  }

  searchResource(offset: number): void {
    this.spinner.show();
    this.paginationService.searchByOffset(this.searchText, offset)
      .subscribe(response => {
        this.spinner.hide();
        if (response != undefined) {
          this.searchResults = response;
        }
        window.scrollTo(0, 0);
      }, error => {
        this.spinner.hide();
        this.router.navigate(['/error']);
      });
  }

  goToPage(n: number): void {
    debugger;
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
          order: this.global.searchResultDetails.order
        };
      }
      this.getInternalResource(this.global.searchResultDetails.filterParam, this.currentPage - 1);
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
          order: this.global.searchResultDetails.order
        };
      }
      this.getInternalResource(this.global.searchResultDetails.filterParam, this.currentPage - 1);
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
          order: this.global.searchResultDetails.order
        };
      }
      this.getInternalResource(this.global.searchResultDetails.filterParam, this.currentPage - 1);
    }
  }

  calculateOffsetValue(pageNumber: number): number {
    return Math.ceil(pageNumber * 10) + 1;
  }

  applyFilter() {
    if (this.searchResults != undefined && this.searchResults.resources != undefined) {
      let allFilter = [{
        'ResourceName': 'All',
        'ResourceCount': this.searchResults.resources.length
      }];
      this.uniqueResources = new Set(this.searchResults.resources
        .map(item => item.resourceType));
      let resourceFilters = [];
      resourceFilters = Array.from(this.uniqueResources).map(resource => {
        return (
          {
            'ResourceName': resource,
            'ResourceCount': this.searchResults.resources
              .filter(x => x.resourceType === resource).length
          }
        );
      });
      this.resourceResults = [...allFilter, ...resourceFilters];
    }
  }

  ngOnInit() {
    if (!this.navigateDataService.getData()
      && sessionStorage.getItem("cacheSearchResults")
      && !this.personalizedResources) {
      this.navigateDataService.setData(JSON.parse(sessionStorage.getItem("cacheSearchResults")));
    }
    this.bindData();
    this.notifyLocationChange();
    this.showRemoveOption = this.showRemove;
    if (this.searchResults.searchFilter) {
      if (this.searchResults.searchFilter.OrderByField === 'modifiedTimeStamp') {
        this.searchResults.searchFilter.OrderByField = 'date';
      }
      this.global.searchResultDetails.sortParam = this.searchResults.searchFilter.OrderByField;
      this.global.searchResultDetails.order = this.searchResults.searchFilter.OrderBy;
      this.global.searchResultDetails.filterParam = this.searchResults.resourceType;
    } else {
      this.sortType = { filterParam: undefined, sortParam: "date", order: "DESC" };
    }
  }

  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    const personalizedResources: SimpleChange = changes.personalizedResources;
    this.mapPersonalizedResource(personalizedResources.currentValue);
  }
}
