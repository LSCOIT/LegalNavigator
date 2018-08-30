import { Component, OnInit, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { SearchService } from './search.service';
import { NavigateDataService } from '../navigate-data.service';
import { ILuisInput } from './search-results/search-results.model';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  inputText: string; 
  @Input()
  searchResults: any;
  luisInput: ILuisInput = { Sentence: '', Location: '', TranslateFrom: '', TranslateTo: '', LuisTopScoringIntent: '' };
  
  constructor(private searchService: SearchService, private router: Router, private navigateDataService: NavigateDataService, private spinner: NgxSpinnerService) { }

  onSubmit(searchForm: NgForm): void {
    this.luisInput.Sentence = searchForm.value.inputText;
    this.luisInput.Location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    sessionStorage.removeItem("cacheSearchResults"); 
    sessionStorage.removeItem("searchedLocationMap");
    this.spinner.show();
    this.searchService.search(this.luisInput)
      .subscribe(response => {
        if (response != undefined) {
          this.searchResults = response;
          this.navigateDataService.setData(this.searchResults);
          this.spinner.hide();
          this.router.navigateByUrl('/searchRefresh', { skipLocationChange: true })
            .then(() =>
              this.router.navigate(['/search'])
            );
        }
      });
  }

  ngOnInit() {
  }

}
