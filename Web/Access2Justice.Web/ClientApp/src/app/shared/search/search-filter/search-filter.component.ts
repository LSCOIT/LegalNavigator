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
  filterParam: string;
  sortParam: string;
  @ViewChildren('filterButtons') filterButtons: QueryList<any>;
  @Input() initialResourceFilter: string;
  buttonToHighlight = [];

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
    this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam });
    this.selectedFilterCriteria = this.filterParam;
  }

  sendSortCriteria(value, resourceType) {
    this.sortParam = resourceType;
    this.notifyFilterCriteria.emit({ filterParam: this.filterParam, sortParam: this.sortParam });
    this.selectedSortCriteria = value;
  }

  ngOnInit() { }

  ngAfterViewInit() {
    this.setInitialHighlightButton();
  }
}
