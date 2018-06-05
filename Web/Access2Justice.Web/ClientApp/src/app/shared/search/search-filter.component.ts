import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ResourceResult } from '../search/search-result';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'app-search-filter',
  templateUrl: './search-filter.component.html',
  styleUrls: ['./search-filter.component.css']
})
export class SearchFilterComponent implements OnInit {
  @Input()
  resourceResults: any;
  @Output() notifyFilterCriteria = new EventEmitter();
  
  constructor() { }

  sendfilterCriteria(resourceType) {
    this.notifyFilterCriteria.emit(resourceType);
  }

  ngOnInit() {
      
  }

}
