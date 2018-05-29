import { Component, OnInit, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { SearchService } from './search.service';
import { NavigateDataService } from '../navigate-data.service';


@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  @Input()
  searchResults: any;
  
  constructor(private searchService: SearchService, private router: Router, private navigateDataService: NavigateDataService) { }

  ngOnInit() {
  }

  onSubmit(searchForm: NgForm): void {
    console.log(searchForm.value.inputText);
    this.searchService.search(searchForm.value.inputText)
      .subscribe(response => {
        if (response != null || response != undefined) {
          this.searchResults = response;
          this.navigateDataService.setResourceData(this.searchResults);
          this.navigateDataService.setsearchText(searchForm.value.inputText);
          this.router.navigate(['/search']);
          
        }
      });
  }

  searchTextChange(searchText: string): void {
    console.log(searchText);
  }

}
