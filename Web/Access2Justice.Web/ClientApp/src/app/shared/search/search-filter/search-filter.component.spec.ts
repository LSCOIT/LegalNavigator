import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement, NO_ERRORS_SCHEMA, EventEmitter, ElementRef } from '@angular/core';
import { SearchFilterComponent } from './search-filter.component';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';
import { QueryList } from '@angular/core/src/render3';
import { Query } from '@angular/core/src/metadata/di';
import { Component } from '@angular/core/src/metadata/directives';

describe('SearchFilterComponent', () => {
  let component: SearchFilterComponent;
  let fixture: ComponentFixture<SearchFilterComponent>;
  let buttonElement: DebugElement;
  let mockFilterButtonsExists:
    [{ "dirty": "false" },
      {
      "first":
      {
        "nativeElement": {
          "classList": ["btn",
            "btn-selected", "button-highlight"]
        }
      }
    }, {
        "last":
        {
          "nativeElement": {
            "classList": ["btn",
              "btn-selected"]
          }
        }
      }];
  let mockFilterButtonsNotExists = [{
    "first":
      {
        "nativeElement": {
          "classList": ["btn",
            "btn-selected"]
        }
      }
  }];

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SearchFilterComponent],
      providers: [
        { provide: ActivatedRoute, useValue: { snapshot: { params: { id: 1 } }, paramMap: Observable.of({ get: () => 1 }) } }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    })
      .compileComponents();
    TestBed.compileComponents();
    fixture = TestBed.createComponent(SearchFilterComponent);
    component = fixture.componentInstance;
  }));

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should create current filter criteria', () => {
    let resourceType = ["All"];
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

