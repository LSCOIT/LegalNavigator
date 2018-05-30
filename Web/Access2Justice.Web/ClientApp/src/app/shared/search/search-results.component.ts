import { Component, OnInit, Input } from '@angular/core';
import { NavigateDataService } from '../navigate-data.service';
import { isNullOrUndefined } from 'util';

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
  constructor(private navigateDataService: NavigateDataService) { }

  ngOnInit() {
    this.searchResults = this.navigateDataService.getResourceData();
    this.searchText = this.navigateDataService.getsearchText();

    if (!isNullOrUndefined(this.searchResults)) {
      this.isInternalResource = this.searchResults.resources;
      this.isWebResource = this.searchResults.webResources;

      if (!this.isWebResource && !this.isInternalResource) {
        this.isLuisResponse = true;
      }

    }
  }

}
