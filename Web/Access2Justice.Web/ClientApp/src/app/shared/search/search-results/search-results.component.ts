import { Component, OnInit, Input } from '@angular/core';
import { NavigateDataService } from '../../navigate-data.service';
import { isNullOrUndefined } from 'util';
import { ResourceResult } from './search-result';

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

  filterSearchResults(resourceType) {
    this.filterType = resourceType;
  }
  
  ngOnInit() {
    this.bindData();
  }
}


