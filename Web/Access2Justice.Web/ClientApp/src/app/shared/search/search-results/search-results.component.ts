import { Component, OnInit, Input, OnDestroy, OnChanges, SimpleChanges, SimpleChange } from '@angular/core';
import { NavigateDataService } from '../../navigate-data.service';
import { ResourceResult } from './search-result';
import { SearchService } from '../search.service';
import { PaginationService } from '../pagination.service';
import { IResourceFilter, ILuisInput } from './search-results.model';
import { LocationService } from '../../location/location.service';
import { environment } from '../../../../environments/environment';

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
  filterType: string = 'All';
  resourceTypeFilter: any[];
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', ResourceIds: '', PageNumber: 0, Location: {} };
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

  constructor(
    private navigateDataService: NavigateDataService,
    private searchService: SearchService,
    private locationService: LocationService,
    private paginationService: PaginationService) { }

  bindData() {
    this.searchResults = this.navigateDataService.getData();
    if (this.searchResults != undefined && this.personalizedResources === undefined) {
      this.isInternalResource = this.searchResults.resources;
      this.isWebResource = this.searchResults.webResources;
      if (this.isWebResource) {
        this.total = this.searchResults.webResources.webPages.totalEstimatedMatches;
        this.searchText = this.searchResults.webResources.queryContext.originalQuery;
        this.pagesToShow = environment.webResourcePagesToShow;
        this.limit = environment.webResourceRecordsToDisplay;
      } else if (this.isInternalResource) {
        this.mapInternalResource();
      } else {
        this.isLuisResponse = true;
        console.log(this.searchResults.luisResponse);
      }
    }
    if (this.personalizedResources != undefined) {      
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
    // need to revisit this logic..
    this.resourceResults = this.searchResults.resourceTypeFilter.reverse();
    if (this.resourceTypeFilter != undefined) {

      for (let index = 0; index < this.resourceTypeFilter.length; index++) {
        if (this.resourceTypeFilter[index].ResourceName === "All") {
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

  filterSearchResults(event) {
    this.sortType = event;
    if (this.isInternalResource && event != undefined) {
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
    this.subscription = this.locationService.notifyLocalLocation.subscribe((value) => {
      this.searchResults = this.navigateDataService.getData();
      this.location = value;
      this.luisInput.Location = value;
      this.luisInput.LuisTopScoringIntent = this.searchResults.topIntent;
      this.luisInput.Sentence = this.searchResults.topIntent;
      this.searchService.search(this.luisInput)
        .subscribe(response => {
          if (response != undefined) {
            this.searchResults = response;
            this.navigateDataService.setData(this.searchResults);
            this.total = 0;
            this.displayMessage = false;
            this.mapInternalResource();
            if (this.searchResults != undefined &&
              this.searchResults.resources != undefined &&
              this.searchResults.resources.length === 0) {
              this.displayMessage = true;
            }            
          }
        });
    });
  }

  getInternalResource(filterName, pageNumber): void {
    this.isServiceCall = this.checkResource(filterName, pageNumber);
    if (this.isServiceCall) {
      if (sessionStorage.getItem("localSearchMapLocation")) {
        this.resourceFilter.Location = JSON.parse(sessionStorage.getItem("localSearchMapLocation"));
      }
      this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
        this.searchResults = response;
        this.addResource(filterName);
      });
    }
  }

  checkResource(resourceName, pageNumber): boolean {
    this.resourceTypeFilter.forEach(item => {
      if (item.ResourceName === resourceName && item.ResourceList != undefined
        && item.ResourceList[this.page - 1] != undefined) {
        this.total = item.ResourceCount;
        this.searchResults = item.ResourceList[this.page - 1];
        this.isServiceCall = false;
      } else if (item.ResourceName === resourceName) {
        this.total = item.ResourceCount;
        this.resourceFilter.ResourceType = item.ResourceName;
        this.resourceFilter.PageNumber = this.currentPage;
        this.resourceFilter.TopicIds = this.topicIds;
        if (item.ResourceList != undefined && item.ResourceList[pageNumber] != undefined) {
          this.resourceFilter.ContinuationToken = JSON.stringify(item.ResourceList[pageNumber]["continuationToken"]);
        }
        this.isServiceCall = true;
      }
    });
    return this.isServiceCall;
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
    this.bindData();
    this.notifyLocationChange();
    if (this.showRemove) {
      this.showRemoveOption = this.showRemove;
    }
    else {
      this.showRemoveOption = false;
    }
  }

  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    const personalizedResources: SimpleChange = changes.personalizedResources;
    console.log(personalizedResources);
    this.mapPersonalizedResource(personalizedResources.currentValue);
  }
}
