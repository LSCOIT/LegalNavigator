import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList } from '@angular/core';

@Component({
  selector: 'app-search-filter',
  templateUrl: './search-filter.component.html',
  styleUrls: ['./search-filter.component.css']
})
export class SearchFilterComponent implements OnInit {
  @Input()
  resourceResults: any;
  @Output() notifyFilterCriteria = new EventEmitter<object>();
  selectedSortCriteria: string = 'A-Z';
  selectedFilterCriteria: string = 'All';
  filterParam: string = 'All';
  sortParam: string="name";
  @ViewChildren('filterButtons') filterButtons: QueryList<any>;
  @Input() initialResourceFilter: string;
  buttonToHighlight = [];
  orderBy: string;
  isSort: boolean = true;

  constructor() { }

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
    this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam, order: this.orderBy });
    this.selectedFilterCriteria = this.filterParam;
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
    this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam, order: this.orderBy });
    this.selectedSortCriteria = value;
  }

  ngOnInit() { }

  ngAfterViewInit() {
    this.setInitialHighlightButton();
  }
}
