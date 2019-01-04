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
  selectedSortCriteria: string = 'Newest to Oldest';
  selectedFilterCriteria: string = 'All';
  filterParam: string;
  sortParam: string="date";
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

  sendSortCriteria(resourceType, orderBy) {
    this.sortParam = resourceType;
    this.orderBy = orderBy;
    this.selectedSortCriteria = this.getOrderByFieldName(this.sortParam, this.orderBy);
    this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam, order: this.orderBy });
  }

  getOrderByFieldName(inputFieldName, orderBy): string {
    let orderByField = '';
    if (inputFieldName === "name") {
      if (orderBy === 'ASC') {
        orderByField = 'A-Z';
      } else {
        orderByField = 'Z-A';
      }
    } else if (inputFieldName === "date") {
      if (orderBy === 'DESC') {
        orderByField = 'Newest to Oldest';
      } else {
        orderByField = 'Oldest to Newest';
      }
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
      this.orderBy = this.searchResults.searchFilter.OrderBy;
      this.selectedSortCriteria = this.getOrderByFieldName(this.sortParam, this.orderBy);
    }
  }
}
