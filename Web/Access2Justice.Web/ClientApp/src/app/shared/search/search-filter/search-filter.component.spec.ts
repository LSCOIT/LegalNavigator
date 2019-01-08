import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA, QueryList } from '@angular/core';
import { SearchFilterComponent } from './search-filter.component';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';
import { Global } from '../../../global';

describe('SearchFilterComponent', () => {
  let component: SearchFilterComponent;
  let fixture: ComponentFixture<SearchFilterComponent>;
  let global;
  
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SearchFilterComponent],
      providers: [
        { provide: ActivatedRoute, 
          useValue: 
          { 
            snapshot: { params: { id: 1 } }, 
            paramMap: Observable.of({ get: () => 1 }) 
          } 
        },
        { provide: Global, useValue: global }
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
    component.filterButtons = new QueryList();
    let mockEvent = {
      target: {
        classList: { add: () => {} }
      }
    };
    spyOn(component, 'resetButtonColor');
    component.sendFilterCriteria(mockEvent, resourceType);
    expect(component.resetButtonColor).toHaveBeenCalled();
    expect(component.filterParam).toEqual(resourceType);
  });

  it('should set order by and sort criteria', () => {
    spyOn(component, 'getOrderByFieldName');
    component.sendSortCriteria("name", "DESC");
    expect(component.sortParam).toBe("name");
    expect(component.orderBy).toBe("DESC");
    expect(component.getOrderByFieldName).toHaveBeenCalledWith("name", "DESC");
  });

  it('should set order by and sort criteria', () => {
    component.sendSortCriteria("date", "ASC");
    expect(component.sortParam).toBe("date");
    expect(component.orderBy).toBe("ASC");
    component.getOrderByFieldName("date", "ASC");
    expect(component.selectedSortCriteria).toEqual("Oldest to Newest");
  });
});

