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
  searchText: string;
  @Input()
  searchResults: any;
  resourceResults: ResourceResult[] = [];
  filterType: string = 'All';  
  resourceTypeFilter: any[];
  resourceFilter: IResourceFilter = { ResourceType: '',ContinuationToken: '',TopicIds: '',PageNumber: '',Location: ''};
  topicIds: any[];
  isServiceCall: boolean;
  currentPage: number = 0;  

  loading = false;
  total = 0;
  page = 1;
  limit = 0;
  offset = 0;
  pagesToShow = 0;


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
        this.limit = 10;
      } else if (this.isInternalResource) {
        this.resourceTypeFilter = this.searchResults.resourceTypeFilter;
        this.resourceResults = this.searchResults.resourceTypeFilter.reverse();
        //need to write logic here..
        if (!isNullOrUndefined(this.resourceTypeFilter)) {

          for (var i = 0; i < this.resourceTypeFilter.length; i++) {
            if (this.resourceTypeFilter[i].ResourceName === "All") {
              this.resourceTypeFilter[i]["ResourceList"] = [{
                'resources': this.searchResults.resources,
                'continuationToken': this.searchResults.continuationToken
              }];
              this.topicIds = this.searchResults.topicIds;
              this.total = this.resourceTypeFilter[i].ResourceCount;
              this.pagesToShow = 2;
              this.limit = 1;
              break;
            }
          }
        }

      } else {
        this.isLuisResponse = true;
        console.log(this.searchResults.luisResponse);
      }

    }
  }
  
  filterSearchResults(event) {
    if (!isNullOrUndefined(event)) {      
      this.filterType = event.filterParam;
      this.page = 1;
      this.currentPage = 0;
      this.getInternalResource(event.filterParam, 0);
    }
  }

  ngOnInit() {
    this.bindData();
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
      if (item.ResourceName === resourceName && !isNullOrUndefined(item.ResourceList)
        && item.ResourceList[this.page - 1] !== undefined) {
        this.searchResults = item.ResourceList[this.page - 1];
        this.isServiceCall = false;
      }
      else if (item.ResourceName === resourceName) {
        this.total = item.ResourceCount;
        this.resourceFilter.ResourceType = item.ResourceName;
        this.resourceFilter.PageNumber = this.currentPage;
        this.resourceFilter.TopicIds = this.topicIds;
        if (!isNullOrUndefined(item.ResourceList) && !isNullOrUndefined(item.ResourceList[pageNumber])) {
          this.resourceFilter.ContinuationToken = JSON.stringify(item.ResourceList[pageNumber]["continuationToken"]);
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
            'continuationToken': this.searchResults.continuationToken
          }];
        } else {
          this.resourceTypeFilter[i]["ResourceList"].push({
            'resources': this.searchResults.resources,
            'continuationToken': this.searchResults.continuationToken
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

}
