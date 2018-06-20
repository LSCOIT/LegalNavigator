import { Component, OnInit, Input } from '@angular/core';
import { NavigateDataService } from '../../navigate-data.service';
import { ResourceResult } from './search-result';
import { SearchService } from '../search.service';
import { IResourceFilter } from './search-results.model';

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
  sortType: any;
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
    if (this.searchResults != undefined) {
      this.isInternalResource = this.searchResults.resources;
      this.isWebResource = this.searchResults.webResources;
      if (this.isWebResource) {
        this.total = this.searchResults.webResources.webPages.totalEstimatedMatches;
        this.searchText = this.searchResults.webResources.queryContext.originalQuery;
        this.pagesToShow = 10;
        this.limit = 50;
      } else if (this.isInternalResource) {
        this.resourceTypeFilter = this.searchResults.resourceTypeFilter;
        // need to revisit this logic..
        this.resourceResults = this.searchResults.resourceTypeFilter.reverse();        
        if (this.resourceTypeFilter != undefined) {

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
    this.sortType = event;
    if (this.isInternalResource && event != undefined) {
      this.page = 1;
      this.currentPage = 0;
      this.filterType = event.filterParam;
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
      }
      else if (item.ResourceName === resourceName) {
        this.total = item.ResourceCount;
        this.resourceFilter.ResourceType = item.ResourceName;
        this.resourceFilter.PageNumber = this.currentPage;
        this.resourceFilter.TopicIds = this.topicIds;
        if (item.ResourceList != undefined && item.ResourceList[pageNumber] != undefined ) {
          this.resourceFilter.ContinuationToken = JSON.stringify(item.ResourceList[pageNumber]["continuationToken"]);
        }
        this.isServiceCall = true;
      }

    });
    return this.isServiceCall;
  }

  addResource(filterName) {
    for (var i = 0; i < this.resourceTypeFilter.length; i++) {
      if (this.resourceTypeFilter[i].ResourceName === filterName) {
        if (this.resourceTypeFilter[i]["ResourceList"] == undefined ) {
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

}
