import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchFilterComponent } from './search-filter.component';

describe('SearchFilterComponent', () => {
  let component: SearchFilterComponent;
  let fixture: ComponentFixture<SearchFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SearchFilterComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create after selecting Filter Criteria', () => {
    component.filterParam = "Best Match"
    component.notifyFilterCriteria.emit({ filterParam: component.filterParam, sortParam: component.sortParam });
    expect(component).toBeTruthy();
  });

  it('should create after sort', () => {
    let value = "Best Match";
    component.sortParam = "Best Match";
    component.notifyFilterCriteria.emit({ filterParam: component.filterParam, sortParam: component.sortParam });
    component.selectedSortCriteria = value;
    expect(component.selectedSortCriteria).toBe(value);
  });

  it('should select corrent Sort Criteria', () => {
    let value = "Best Match";
    component.sortParam = "Best Match";
    component.notifyFilterCriteria.emit({ filterParam: component.filterParam, sortParam: component.sortParam });
    component.selectedSortCriteria = value;
    expect(component.selectedSortCriteria).toEqual(value);
  });
    
});

