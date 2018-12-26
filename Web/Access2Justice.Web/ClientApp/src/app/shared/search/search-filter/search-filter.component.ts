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
  isSort: boolean = true;

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
    //if (this.sortParam === resourceType) {
    //  //this.isSort = !(this.isSort);
    //  this.orderBy = this.isSort ? 'ASC' : 'DESC';
    //} else {
    //  this.isSort = true;
    //  this.orderBy = 'ASC';
    //}
    this.selectedFilterCriteria = this.filterParam;
    this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam, order: this.orderBy });
  }

  sendSortCriteria(value, resourceType) {
    if (this.sortParam === resourceType) {
      this.isSort = !(this.isSort);
      this.orderBy = this.isSort ? 'ASC' : 'DESC';
    } else {
      this.isSort = true;
      this.orderBy = 'ASC';
    }
    this.sortParam = resourceType;
    if (value === "name") {
      value = 'A-Z';
    } else if(value === "date") {
      value = 'Newest to Oldest'
    }
    this.selectedSortCriteria = value;
    this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam, order: this.orderBy });
  }

  ngOnInit() {
    //this.selectedSortCriteria = this.searchResultDetails.sortParam;
    //this.orderBy = this.searchResultDetails.order;
  }

  ngAfterViewInit() {
    this.setInitialHighlightButton();
  }

  ngOnChanges() {
    if (this.searchResults.searchFilter) {
      this.sortParam = this.searchResults.searchFilter.OrderByField;
      if (this.searchResults.searchFilter.OrderByField === "name") {
        this.searchResults.searchFilter.OrderByField = 'A-Z';
      } else if (this.searchResults.searchFilter.OrderByField === "date") {
        this.searchResults.searchFilter.OrderByField = 'Newest to Oldest'
      }
      this.selectedSortCriteria = this.searchResults.searchFilter.OrderByField;
      this.orderBy = this.searchResults.searchFilter.OrderBy;
    }
  }
}
