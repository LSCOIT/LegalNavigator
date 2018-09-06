import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA, QueryList } from '@angular/core';
import { SearchFilterComponent } from './search-filter.component';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';

describe('SearchFilterComponent', () => {
  let component: SearchFilterComponent;
  let fixture: ComponentFixture<SearchFilterComponent>;
  
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
        }
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
    let event = {
      target: {
        classList: {add: () => {}}
      }
    }
    spyOn(component, 'resetButtonColor');
    component.sendFilterCriteria(resourceType);
    expect(component.resetButtonColor).toHaveBeenCalled();
    expect(component.filterParam).toEqual(resourceType);
  });

  it('should create current sort criteria', () => {
    let value = "Best Match";
    let resourceType = "bestMatch";
    component.sendSortCriteria(value, resourceType);
    expect(component.selectedSortCriteria).toEqual(value);
  });

});

