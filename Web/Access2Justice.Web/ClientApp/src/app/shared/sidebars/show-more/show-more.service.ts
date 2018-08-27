import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NavigateDataService } from '../../navigate-data.service';
import { PaginationService } from '../../pagination/pagination.service';
import { IResourceFilter } from '../../search/search-results/search-results.model';

@Injectable()
export class ShowMoreService {
  constructor(
    private http: HttpClient,
    private navigateDataService: NavigateDataService,
    private router: Router,
    private paginationService: PaginationService
  ) { }
  searchResults: any;
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: [], ResourceIds: [], PageNumber: 0, Location: { "state": "", "county": "", "city": "", "zipCode": "" }, IsResourceCountRequired: true };
  topIntent: string;
  activeResource: any;

  clickSeeMoreOrganizations(resourceType: string, activeId: string) {
    sessionStorage.removeItem("cacheSearchResults");
    sessionStorage.removeItem("searchedLocationMap");
    this.resourceFilter.ResourceType = resourceType;
    this.resourceFilter.TopicIds = [];
    if (activeId) {
      this.resourceFilter.TopicIds.push(activeId);
    }
    this.resourceFilter.Location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    this.resourceFilter.IsResourceCountRequired = true;
    this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
      if (response != undefined) {
        this.searchResults = response;
        this.searchResults.topIntent = this.topIntent;
        this.searchResults.resourceType = resourceType;
        this.searchResults.isItFromTopicPage = true;
        this.navigateDataService.setData(this.searchResults);
        this.router.navigate(['/search']);
      }
    });
  }
}
