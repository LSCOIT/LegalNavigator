import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { isNullOrUndefined } from 'util';

@Component({
    selector: 'app-search-filter',
    templateUrl: './search-filter.component.html',
    styleUrls: ['./search-filter.component.css']
})
export class SearchFilterComponent implements OnInit {
    @Input()
    resourceResults: any;
    @Output() notifyFilterCriteria = new EventEmitter<object>();
    selectedSortCriteria: string = 'Best Match';
    filterParam: any;
    sortParam: any;

    constructor() { }

    sendFilterCriteria(resourceType) {
        this.filterParam = resourceType;
        this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam });
    }

    sendSortCriteria(value, resourceType) {
        this.sortParam = resourceType;
        this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam });
        this.selectedSortCriteria = value;
    }

    ngOnInit() { }

}
