import { Component, OnInit, Input } from '@angular/core';
import { NavigateDataService } from '../../navigate-data.service';
import { isNullOrUndefined } from 'util';
import { ResourceResult } from './search-result';
import { SearchService } from '../search.service';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {

  isInternalResource: boolean;
  isWebResource: boolean;
  isLuisResponse: boolean;
  searchText: any;
  @Input()
  searchResults: any;
  uniqueResources: any;
  resourceResults: ResourceResult[] = [];
  filterType: string;
  sortType: any;
  resourceTypeFilter: any[];
  resourceFilter: IResourceFilter = { ResourceType: '',ContinuationToken: '',TopicIds: '',PageNumber: '',Location: ''};
  topicIds: any[];
  isServiceCall: boolean;

  loading = false;
  total = 0;
  page = 1;
  limit = 10;
  offset = 0;
  pagesToShow = 2;


  constructor(private navigateDataService: NavigateDataService, private searchService: SearchService) { }

  bindData() {
    this.searchResults = this.navigateDataService.getData();
    if (!isNullOrUndefined(this.searchResults)) {
      this.isInternalResource = this.searchResults.resources;
      this.isWebResource = this.searchResults.webResources;
      if (this.isWebResource) {
        this.total = this.searchResults.webResources.webPages.totalEstimatedMatches;
        this.searchText = this.searchResults.webResources.queryContext.originalQuery;
        this.pagesToShow = 10;
      } else if (this.isInternalResource) {
        this.total = 20;
        this.resourceTypeFilter = this.searchResults.resourceTypeFilter;
        this.resourceResults = this.searchResults.resourceTypeFilter.reverse();
        //need to write logic here..
        if (!isNullOrUndefined(this.resourceTypeFilter)) {

          for (var i = 0; i < this.resourceTypeFilter.length; i++) {
            if (this.resourceTypeFilter[i].ResourceName === "All") {
              this.resourceTypeFilter[i]["ResourceList"] = [{
                'resources': this.searchResults.resources,
                'continutationToken': this.searchResults.continuationToken
              }];
              this.topicIds = this.searchResults.topicIds;
            }
          }
          //this.searchResults.resourceTypeFilter.forEach(item => {
          //  if (item === "All") {
          //    this.searchResults.resourceTypeFilter[item]["ResourceList"] = [{
          //      'resources': this.searchResults.resources,
          //      'continutationToken': this.searchResults.continuationToken,
          //      'topicIds': this.searchResults.topicIds
          //    }];
          //  }
          //});

        }

      } else {
        this.isLuisResponse = true;
        console.log(this.searchResults.luisResponse);
      }

    }
  }

  applyFilter() {
    if (!isNullOrUndefined(this.searchResults) && !isNullOrUndefined(this.searchResults.resources)) {
      this.resourceResults.push({
        'resourceName': 'All',
        'resourceCount': this.searchResults.resources.length
      });
      this.uniqueResources = new Set(this.searchResults.resources
        .map(item => item.resourceType));
      this.uniqueResources.forEach(item => {
        if (!isNullOrUndefined(item)) {
          this.resourceResults.push({
            'resourceName': item,
            'resourceCount': this.searchResults.resources
              .filter(x => x.resourceType === item).length
          });
        }
      });
    }
  }

  filterSearchResults(event) {
    if (!isNullOrUndefined(event)) {
      this.sortType = event;
      this.getInternalResource(event.filterParam, 0);
    }
  }

  ngOnInit() {
    this.bindData();
   // this.applyFilter();
  }

  getInternalResource(filterName, pageNumber): void {
    this.isServiceCall = this.checkResource(filterName, pageNumber);
    if (this.isServiceCall) {
      this.searchService.getPagedResources(this.resourceFilter).subscribe(response => {
        this.searchResults = response;
        this.addResource(filterName, response);
      });
    }
  }

  checkResource(resourceName, pageNumber): boolean {

    this.resourceTypeFilter.forEach(item => {
      if (item.ResourceName === resourceName && !isNullOrUndefined(item.ResourceList) && !isNullOrUndefined(item.ResourceList[pageNumber])) {
        this.searchResults = item.ResourceList[pageNumber];
        this.isServiceCall = false;
      }
      else if (item.ResourceName === resourceName && isNullOrUndefined(item.ResourceList)) {
        this.resourceFilter.ResourceType = item.ResourceName;
        this.resourceFilter.PageNumber = pageNumber;
        this.resourceFilter.TopicIds = this.topicIds;
        if (!isNullOrUndefined(item.ResourceList) && !isNullOrUndefined(item.ResourceList[pageNumber - 1])) {
          this.resourceFilter.ContinuationToken = item.ResourceList[pageNumber - 1]["continuationToken"];
        }
        this.isServiceCall = true;
      }

    });
    return this.isServiceCall;
  }

  addResource(filterName, resource) {
    for (var i = 0; i < this.resourceTypeFilter.length; i++) {
      if (this.resourceTypeFilter[i].ResourceName === filterName) {
        if (isNullOrUndefined(this.resourceTypeFilter[i]["ResourceList"])) {
          this.resourceTypeFilter[i]["ResourceList"] = [{
            'resources': this.searchResults.resources,
            'continutationToken': this.searchResults.continuationToken
          }];
        } else {
          this.resourceTypeFilter[i]["ResourceList"].push({
            'resources': this.searchResults.resources,
            'continutationToken': this.searchResults.continuationToken
          });

        }

      }
    }
  }

  searchResource(offset: number): void {
    this.searchService.searchByOffset(this.searchText, offset)
      .subscribe(response => {
        if (!isNullOrUndefined(response) && !isNullOrUndefined(response)) {
          this.searchResults = response;
          //if (!isNullOrUndefined(this.searchResults) && !isNullOrUndefined(this.searchResults.webResources)) {
          //  this.searchResults = this.searchResults.webResources.webPages;
          //}
        }
      });
  }



  goToPage(n: number): void {
    this.page = n;
    if (this.isWebResource) {
      this.searchResource(this.calculateOffsetValue(n));
    } else {

    }
  }

  onNext(): void {
    this.page++;
    if (this.isWebResource) {
      this.searchResource(this.calculateOffsetValue(this.page++));
    } else {

    }
  }

  onPrev(): void {
    this.page--;
    if (this.isWebResource) {
      this.searchResource(this.calculateOffsetValue(this.page--));
    } else {

    }
  }

  calculateOffsetValue(pageNumber: number): number {

    this.offset = Math.ceil(pageNumber * 10) + 1;

    return this.offset;
  }

}


