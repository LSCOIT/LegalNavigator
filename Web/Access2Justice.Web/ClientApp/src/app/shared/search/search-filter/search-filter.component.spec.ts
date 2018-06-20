import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchFilterComponent } from './search-filter.component';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';

describe('SearchFilterComponent', () => {
  let component: SearchFilterComponent;
  let fixture: ComponentFixture<SearchFilterComponent>;
  let buttonElement: DebugElement;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SearchFilterComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchFilterComponent);
    component = fixture.componentInstance;
    buttonElement = fixture.debugElement.query(By.css(".filter-type button"));
    fixture.detectChanges();
  });

  it('should create', () => {
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

