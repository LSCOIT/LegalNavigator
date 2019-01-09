import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NavigateDataService } from '../../services/navigate-data.service';
import { PaginationService } from '../../pagination/pagination.service';
import { IResourceFilter } from '../../search/search-results/search-results.model';
import { LocationDetails } from '../../map/map';

@Injectable()
export class ShowMoreService {
  searchResults: any;
  resourceFilter: IResourceFilter = {
    ResourceType: '',
    ContinuationToken: '',
    TopicIds: [],
    ResourceIds: [],
    PageNumber: 0,
    Location: {
      "state": "",
      "county": "",
      "city": "",
      "zipCode": ""
    },
    IsResourceCountRequired: true,
    IsOrder: false,
    OrderByField: '',
    OrderBy: ''
  };  
  activeResource: any;
  locationDetails: LocationDetails;

  constructor(
    private http: HttpClient,
    private navigateDataService: NavigateDataService,
    private router: Router,
    private paginationService: PaginationService
  ) { }

  clickSeeMoreOrganizations(resourceType: string, activeId: string, topIntent: string) {
    sessionStorage.removeItem("cacheSearchResults");
    sessionStorage.removeItem("searchedLocationMap");
    this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    this.resourceFilter.ResourceType = resourceType;
    this.resourceFilter.TopicIds = [];
    if (activeId) {
      this.resourceFilter.TopicIds.push(activeId);
    }
    this.resourceFilter.Location = this.locationDetails.location;
    this.resourceFilter.IsResourceCountRequired = true;
    this.resourceFilter.OrderBy = "DESC";
    this.resourceFilter.OrderByField = "date";
    this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
      if (response != undefined) {
        this.searchResults = response;
        this.searchResults.topIntent = topIntent;
        this.searchResults.resourceType = resourceType;
        this.searchResults.isItFromTopicPage = true;
        this.navigateDataService.setData(this.searchResults);
        this.router.navigate(['/search']);
      }
    });
  }
}
