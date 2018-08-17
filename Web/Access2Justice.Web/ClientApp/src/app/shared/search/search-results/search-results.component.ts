import { Component, OnInit, Input, OnDestroy, OnChanges, SimpleChanges, SimpleChange } from '@angular/core';
import { NavigateDataService } from '../../navigate-data.service';
import { ResourceResult } from './search-result';
import { SearchService } from '../search.service';
import { PaginationService } from '../pagination.service';
import { IResourceFilter, ILuisInput } from './search-results.model';
import { MapService } from '../../map/map.service';
import { environment } from '../../../../environments/environment';
import { PersonalizedPlanService } from '../../../guided-assistant/personalized-plan/personalized-plan.service';

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
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: [], ResourceIds: [], PageNumber: 0, Location: {}, IsResourceCountRequired: false };
  luisInput: ILuisInput = { Sentence: '', Location: {}, LuisTopScoringIntent: '', TranslateFrom: '', TranslateTo: '' };
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
  initialResourceLength:number;
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

  constructor(
    private navigateDataService: NavigateDataService,
    private searchService: SearchService,
    private mapService: MapService,
    private paginationService: PaginationService,
    private personalizedPlanService: PersonalizedPlanService) { }

  bindData() {
    this.showDefaultMessage = false;
    this.showNoResultsMessage = false;
    this.searchResults = this.navigateDataService.getData();    
    if (this.searchResults != undefined && this.personalizedResources === undefined) {
      this.cacheSearchResultsData();
      this.isInternalResource = this.searchResults.resources;
      this.isWebResource = this.searchResults.webResources;
      if (this.isWebResource) {
        this.mapWebResources();
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
    // need to revisit this logic..
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
      sessionStorage.setItem("searchedLocationMap", JSON.stringify(this.location));
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
    if (this.isInternalResource && event != undefined && event.filterParam != undefined) {
      this.page = 1;
      this.currentPage = 0;
      this.filterType = event.filterParam;
      this.getInternalResource(event.filterParam, 0);
    }
  }

  notifyLocationChange() {
    if (sessionStorage.getItem("localSearchMapLocation")) {
      this.location = JSON.parse(sessionStorage.getItem("localSearchMapLocation"));
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
      PageNumber: 0, ContinuationToken: '', IsResourceCountRequired: true, ResourceIds: []
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
    this.searchService.search(this.luisInput)
      .subscribe(response => {
        if (response != undefined) {
          this.searchResults = response;
          this.displayFetchedResources();
        }
      });
  }

  displayFetchedResources() {
    this.navigateDataService.setData(this.searchResults);    
    this.cacheSearchResultsData();
    this.isInternalResource = true;
    this.displayNoResultsMessage();
    this.mapInternalResource();
    //TODO: There is reload issue in ngFor and data is not loading.
    // So we are triggering Filter method.
    let event = { filterParam: environment.All };
    this.filterSearchResults(event);
  }

  displayNoResultsMessage() {
    this.displayMessage = false;
    if (this.searchResults != undefined &&
      this.searchResults.resources != undefined &&
      this.searchResults.resources.length === 0) {
      this.displayMessage = true;
      this.isInternalResource = false;
    }
  }

  getInternalResource(filterName, pageNumber): void {
    this.checkResource(filterName, pageNumber);
    if (this.isServiceCall) {
      if (sessionStorage.getItem("localSearchMapLocation")) {
        this.resourceFilter.Location = JSON.parse(sessionStorage.getItem("localSearchMapLocation"));
      }
      this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
        this.searchResults = response;
        if (this.page == 1) {
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
        if (this.page == 1) {
          this.updateCacheStorage(this.filterType);
        }
        this.isServiceCall = false;
        break;
      }
      else if (this.resourceTypeFilter[index].ResourceName === resourceName) {
        this.total = this.resourceTypeFilter[index].ResourceCount;
        this.resourceFilter.ResourceType = this.resourceTypeFilter[index].ResourceName;
        this.resourceFilter.PageNumber = this.currentPage;
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
    this.paginationService.searchByOffset(this.searchText, offset)
      .subscribe(response => {
        if (response != undefined) {
          this.searchResults = response;
        }
      });
  }

  goToPage(n: number): void {
    if (this.page < n) {
      this.currentPage = this.page;
    } else {
      this.currentPage = n;
    }
    this.page = n;
    if (this.isWebResource) {
      this.searchResource(this.calculateOffsetValue(n));
    } else {
      this.getInternalResource(this.filterType, this.currentPage - 1);
    }
  }

  onNext(): void {
    this.currentPage = this.page;
    this.page++;
    if (this.isWebResource) {
      this.searchResource(this.calculateOffsetValue(this.page));
    } else {
      this.getInternalResource(this.filterType, this.currentPage - 1);
    }
  }

  onPrev(): void {
    this.currentPage = this.page;
    this.page--;
    if (this.isWebResource) {
      this.searchResource(this.calculateOffsetValue(this.page));
    } else {
      this.getInternalResource(this.filterType, this.page - 1);
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
    if (sessionStorage.getItem("bookmarkedResource")) {
      this.personalizedPlanService.saveResourcesToUserProfile();
    }
  }

  ngOnDestroy() {
    sessionStorage.removeItem("localSearchMapLocation");
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    const personalizedResources: SimpleChange = changes.personalizedResources;
    this.mapPersonalizedResource(personalizedResources.currentValue);
  }
}
