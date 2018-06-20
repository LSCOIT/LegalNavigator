import { Component, OnInit, Input } from '@angular/core';
import { NavigateDataService } from '../../navigate-data.service';
import { isNullOrUndefined } from 'util';
import { ResourceResult } from './search-result';
import { DisplayMapLocation } from '../../location/location';

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
  filterType: string = 'All';
  location: DisplayMapLocation;

  constructor(private navigateDataService: NavigateDataService) { }

  bindData() {
    this.searchResults = this.navigateDataService.getData();
    if (!isNullOrUndefined(this.searchResults)) {
      this.isInternalResource = this.searchResults.resources;
      this.isWebResource = this.searchResults.webResources;
      if (!this.isWebResource && !this.isInternalResource) {
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

  filterSearchResults(resourceType) {
    this.filterType = resourceType;
  }

  displayOrganizations(location) {
    console.log("Test" + location);
    this.location = location;
  }
  ngOnInit() {
    this.bindData();
    this.applyFilter();
  }
}


