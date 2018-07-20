import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginationComponent } from './pagination.component';

describe('PaginationComponent', () => {
  let component: PaginationComponent;
  let fixture: ComponentFixture<PaginationComponent>;
  let mockRecordCount = 105;
  let mockPageSize = 10;
  let mockCurrentPage = 2;
  let mockEndPage = 11;
  let mockBeginLine = 11;
  let mockEndLine = 20;
  let beTrue = false;
  let expectedPages: number[] = [];
  expectedPages[0] = 1;
  expectedPages[1] = 2;
  expectedPages[2] = 3;
  expectedPages[3] = 4;

  let expectedLastPages: number[] = [];
  expectedLastPages[0] = 8;
  expectedLastPages[1] = 9;
  expectedLastPages[2] = 10;
  expectedLastPages[3] = 11;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PaginationComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaginationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get start line number for a specified page: min', () => {
    component.perPage = mockPageSize; //10
    component.page = mockCurrentPage; //2
    let line = component.getMin()    //start line number in 2nd page i.e. 11
    expect(line).toBe(mockBeginLine);
  });

  it('should get end line number for a specified page: max', () => {
    component.perPage = mockPageSize; //10
    component.page = mockCurrentPage; //2
    let line = component.getMax();    //start line number in 2nd page i.e. 20
    expect(line).toBe(mockEndLine);
  });

  it('should be on specified page to be defined', () => {
    component.count = mockRecordCount; // total record count
    component.perPage = mockPageSize // 10
    let numberofPages = component.totalPages();;
    expect(numberofPages).toBe(11); // number of pages to be 11
  });

  it('should be defined - the page is not last page', () => {
    component.count = mockRecordCount; // total record count
    component.page = mockCurrentPage // 2
    component.perPage = mockPageSize; // 10
    let isLastPage = component.lastPage(); //is this last page?
    expect(isLastPage).toBe(false) // false
  });

  it('should be defined - the page is last page', () => {
    component.count = mockRecordCount; // total record count
    component.page = mockEndPage // 11
    component.perPage = mockPageSize; // 10
    let isLastPage = component.lastPage(); //is this last page?
    expect(isLastPage).toBe(true) // true
  });

  it('should be displayed the four pages along with current page', () => {
    component.count = mockRecordCount; // total record count
    component.page = mockCurrentPage // 2
    component.perPage = mockPageSize; // 10
    component.pagesToShow = 4;
    let returnPages: number[] = [];
    returnPages = component.getPages(); // will get 1,2,3,4 i.e. first found pages
    expect(component.getPages).toHaveBeenCalled;
    expect(expectedPages).toEqual(returnPages);
  });

  it('should be displayed last four pages if the current page is last page', () => {
    component.count = mockRecordCount; // total record count
    component.page = mockEndPage // 2
    component.perPage = mockPageSize; // 10
    component.pagesToShow = 4;
    let returnPages: number[] = [];
    returnPages = component.getPages(); // will get 8,9,10,11 i.e. first found pages
    expect(component.getPages).toHaveBeenCalled;
    expect(expectedLastPages).toEqual(returnPages);
  });
});
