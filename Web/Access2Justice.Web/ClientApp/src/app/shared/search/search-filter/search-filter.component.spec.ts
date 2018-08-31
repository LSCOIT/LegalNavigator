import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement, NO_ERRORS_SCHEMA } from '@angular/core';
import { SearchFilterComponent } from './search-filter.component';

describe('SearchFilterComponent', () => {
  let component: SearchFilterComponent;
  let fixture: ComponentFixture<SearchFilterComponent>;
  let buttonElement: DebugElement;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SearchFilterComponent],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchFilterComponent);
    component = fixture.componentInstance;
    buttonElement = fixture.debugElement.query(By.css(".filter-type button"));
    fixture.detectChanges();
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should create current filter criteria', () => {
    let resourceType = "All";
    component.sendFilterCriteria(resourceType);
    expect(component.filterParam).toEqual(resourceType);
  });

  it('should create current sort criteria', () => {
    let value = "Best Match";
    let resourceType = "bestMatch";
    component.sendSortCriteria(value, resourceType);
    expect(component.selectedSortCriteria).toEqual(value);
  });
    
});

