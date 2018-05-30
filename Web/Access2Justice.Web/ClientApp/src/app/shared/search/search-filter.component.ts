import { Component, OnInit, Input } from '@angular/core';
import { ResourceResult } from '../search/search-result';

@Component({
  selector: 'app-search-filter',
  templateUrl: './search-filter.component.html',
  styleUrls: ['./search-filter.component.css']
})
export class SearchFilterComponent implements OnInit {
  @Input()
  resourceResults: any;

  constructor() { }

  ngOnInit() {
    
  }

}
