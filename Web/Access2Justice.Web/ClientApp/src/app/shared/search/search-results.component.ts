import { Component, OnInit, Input } from '@angular/core';
import { NavigateDataService } from '../navigate-data.service';
import { isNullOrUndefined } from 'util';
import { ResourceResult } from '../search/search-result';

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
  resourceResult: ResourceResult = {};
  resourceResults: ResourceResult[] = [];
  constructor(private navigateDataService: NavigateDataService) { }

  ngOnInit() {
    this.searchResults = this.navigateDataService.getResourceData();
    this.searchText = this.navigateDataService.getsearchText();

    if (!isNullOrUndefined(this.searchResults)) {
      this.isInternalResource = this.searchResults.resources;
      this.isWebResource = this.searchResults.webResources;

      if (!this.isWebResource && !this.isInternalResource) {
        this.isLuisResponse = true;
        console.log(this.searchResults.luisResponse);
      }

    }

  //To get the unique filter results
    if (!isNullOrUndefined(this.searchResults) && !isNullOrUndefined(this.searchResults.resources)) {
      this.resourceResults.push({ 'resourceName': 'All', 'resourceCount': this.searchResults.resources.length });
      this.uniqueResources = new Set(this.searchResults.resources.map(item => item.resourceType));
      this.uniqueResources.forEach(item => {
        console.log(item, this.searchResults.resources.filter(x => x.resourceType == item).length);
        if (!isNullOrUndefined(item)) {
          this.resourceResults.push({ 'resourceName': item, 'resourceCount': this.searchResults.resources.filter(x => x.resourceType == item).length });
        }
      });
    }
  }



}


