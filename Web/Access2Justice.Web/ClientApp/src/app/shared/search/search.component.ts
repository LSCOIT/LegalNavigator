import { Component, OnInit, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { SearchService } from './search.service';
import { NavigateDataService } from '../navigate-data.service';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  @Input()
  searchResults: any;
  luisInput: ILuisInput = { Sentence: '', Location: '', TranslateFrom: '', TranslateTo: '' };  
  
  constructor(private searchService: SearchService, private router: Router, private navigateDataService: NavigateDataService) { }

  ngOnInit() {
  }

  onSubmit(searchForm: NgForm): void {
    this.luisInput.Sentence = searchForm.value.inputText;
    this.luisInput.Location = JSON.parse(sessionStorage.getItem("globalMapLocation"));

    this.searchService.search(this.luisInput)
      .subscribe(response => {
        if (!isNullOrUndefined(response)) {
          this.searchResults = response;
          this.navigateDataService.setData(this.searchResults);          
          this.router.navigateByUrl('/searchRefresh', { skipLocationChange: true })
            .then(() =>
              this.router.navigate(['/search'])
            );
        }
      });
  }
}
