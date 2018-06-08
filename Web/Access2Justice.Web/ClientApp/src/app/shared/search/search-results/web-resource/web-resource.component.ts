import { Component, OnInit, Input } from '@angular/core';

import { SearchService } from '../../search.service';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-web-resource',
  templateUrl: './web-resource.component.html',
  styleUrls: ['./web-resource.component.css']
})
export class WebResourceComponent implements OnInit {

  @Input()
  searchResults: any;

  searchResponse: any;
  totalEstimatedMatches: any;
  searchText: string;

  loading = false;
  total = 0;
  page = 1;
  limit = 10;
  offset = 0;
  pagesToShow = 10;

  constructor(private searchService: SearchService) {  }

  ngOnInit() {
    if (!isNullOrUndefined(this.searchResults) && !isNullOrUndefined(this.searchResults.webResources)) {
      this.total = this.searchResults.webResources.webPages.totalEstimatedMatches;
      this.searchText = this.searchResults.webResources.queryContext.originalQuery;
      this.searchResults = this.searchResults.webResources.webPages;
      
      
    } 
  }

  searchResource(offset: number): void {
    this.searchService.searchByOffset(this.searchText, offset)
      .subscribe(response => {
        if (!isNullOrUndefined(response) && !isNullOrUndefined(response)) {
          this.searchResponse = response;
          if (!isNullOrUndefined(this.searchResponse) && !isNullOrUndefined(this.searchResponse.webResources)) {
            this.searchResults = this.searchResponse.webResources.webPages;
          }
        }
      });
  }



  goToPage(n: number): void {
    this.page = n;
    this.searchResource(this.calculateOffsetValue(n));
  }

  onNext(): void {
    this.page++;
    this.searchResource(this.calculateOffsetValue(this.page++));
  }

  onPrev(): void {
    this.page--;
    this.searchResource(this.calculateOffsetValue(this.page--));
  }

  calculateOffsetValue(pageNumber: number): number {
    
    this.offset = Math.ceil(pageNumber * 10) + 1;

    return this.offset;
  }


}
