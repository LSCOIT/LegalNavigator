import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList, OnChanges } from '@angular/core';
import { Global } from '../../../global';

@Component({
  selector: 'app-search-filter',
  templateUrl: './search-filter.component.html',
  styleUrls: ['./search-filter.component.css']
})
export class SearchFilterComponent implements OnInit, OnChanges {
  @Input()
  resourceResults: any;
  @Input() searchResults: any;
  @Output() notifyFilterCriteria = new EventEmitter<object>();
  selectedSortCriteria: string = 'A-Z';
  selectedFilterCriteria: string = 'All';
  filterParam: string;
  sortParam: string="name";
  @ViewChildren('filterButtons') filterButtons: QueryList<any>;
  @Input() initialResourceFilter: string;
  buttonToHighlight = [];
  orderBy: string = "ASC";

  constructor(private global: Global) { }

  findButtonWith(initialResourceType) {
    this.buttonToHighlight =
      this.filterButtons
        .filter(filterButton => filterButton.nativeElement.innerText.includes(initialResourceType));
  }

  setInitialHighlightButton() {
    if (this.initialResourceFilter) {
      this.findButtonWith(this.initialResourceFilter);
      this.buttonToHighlight[0].nativeElement.classList.add('button-highlight');
    } else {
      this.findButtonWith('All');
      this.buttonToHighlight[0].nativeElement.classList.add('button-highlight');
    }
  }

  resetButtonColor() {
    this.filterButtons
      .forEach(filterButton => {
        if (filterButton.nativeElement.classList.contains('button-highlight')) {
          filterButton.nativeElement.classList.remove('button-highlight');
        }
      });
  }

  sendFilterCriteria(event, resourceType) {
    this.resetButtonColor();
    event.target["classList"].add('button-highlight');
    this.filterParam = resourceType;
    this.selectedFilterCriteria = this.filterParam;
    this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam, order: this.orderBy });
  }

  sendSortCriteria(value, resourceType) {
    if (this.sortParam === resourceType) {
      if (this.orderBy === 'ASC') {
        this.orderBy = 'DESC';
      } else if(this.orderBy === 'DESC') {
        this.orderBy = 'ASC';
      }
    } else {
      this.orderBy = 'ASC';
    }
    this.sortParam = resourceType;
    this.selectedSortCriteria = this.getOrderByFieldName(value);
    this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam, order: this.orderBy });
  }

  getOrderByFieldName(inputFieldName): string {
    let orderByField = '';
    if (inputFieldName === "name") {
      orderByField = 'A-Z';
    } else if (inputFieldName === "date") {
      orderByField = 'Newest to Oldest'
    }
    return orderByField;
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.setInitialHighlightButton();
  }

  ngOnChanges() {
    if (this.searchResults.searchFilter) {
      this.sortParam = this.searchResults.searchFilter.OrderByField;
      this.selectedSortCriteria = this.getOrderByFieldName(this.searchResults.searchFilter.OrderByField);
      this.orderBy = this.searchResults.searchFilter.OrderBy;
    }
  }
}
