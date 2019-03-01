import { AfterViewInit, Component, EventEmitter, Input, OnChanges, OnInit, Output, QueryList, ViewChildren } from '@angular/core';

export interface ParamsChange {
  filterParam?: string;
  sortParam?: string;
  order?: 'ASC' | 'DESC';
}

@Component({
  selector: 'app-search-filter',
  templateUrl: './search-filter.component.html',
  styleUrls: ['./search-filter.component.css']
})
export class SearchFilterComponent implements OnInit, OnChanges, AfterViewInit {
  @Input()
  resourceResults: any;
  @Input() searchResults: any;
  @Output() notifyFilterCriteria = new EventEmitter<ParamsChange>();
  selectedSortCriteria = 'Newest to Oldest';
  selectedFilterCriteria = 'All';
  filterParam = 'All';
  sortParam = 'date';
  @ViewChildren('filterButtons') filterButtons: QueryList<any>;
  @Input() initialResourceFilter: string;
  buttonToHighlight = [];
  order: 'ASC' | 'DESC' = 'ASC';

  constructor() {
  }

  findButtonWith(initialResourceType) {
    this.buttonToHighlight = this.filterButtons.filter(filterButton =>
      filterButton.nativeElement.innerText.includes(initialResourceType)
    );
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
    this.filterButtons.forEach(filterButton => {
      if (filterButton.nativeElement.classList.contains('button-highlight')) {
        filterButton.nativeElement.classList.remove('button-highlight');
      }
    });
  }

  sendFilterCriteria(event, resourceType) {
    this.resetButtonColor();
    event.target['classList'].add('button-highlight');
    this.filterParam = resourceType;
    this.selectedFilterCriteria = this.filterParam;
    this.notifyFilterCriteria.emit({
      filterParam: this.filterParam,
      sortParam: this.sortParam,
      order: this.order
    });
  }

  sendSortCriteria(field: string, order: 'ASC' | 'DESC') {
    this.sortParam = field;
    this.order = order;
    this.selectedSortCriteria = this.getOrderByFieldName(
      this.sortParam,
      this.order
    );
    this.notifyFilterCriteria.emit({
      filterParam: this.filterParam,
      sortParam: this.sortParam,
      order: this.order
    });
  }

  getOrderByFieldName(inputFieldName, orderBy): string {
    let orderByField = '';
    if (inputFieldName === 'name') {
      if (orderBy === 'ASC') {
        orderByField = 'A-Z';
      } else {
        orderByField = 'Z-A';
      }
    } else if (inputFieldName === 'date') {
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
      this.order = this.searchResults.searchFilter.OrderBy;
      this.selectedSortCriteria = this.getOrderByFieldName(
        this.sortParam,
        this.order
      );
      if (!this.selectedSortCriteria) {
        this.selectedSortCriteria = 'Newest to Oldest';
      }
    }
  }
}
