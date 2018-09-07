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
    component.perPage = mockPageSize;
    component.page = mockCurrentPage;
    let line = component.getMin();
    expect(line).toBe(mockBeginLine);
  });

  it('should get end line number for a specified page: max', () => {
    component.perPage = mockPageSize;
    component.page = mockCurrentPage;
    let line = component.getMax();
    expect(line).toBe(mockEndLine);
  });

  it('should be on specified page to be defined', () => {
    component.count = mockRecordCount;
    component.perPage = mockPageSize;
    let numberofPages = component.totalPages();
    expect(numberofPages).toBe(11);
  });

  it('should be defined - the page is not last page', () => {
    component.count = mockRecordCount;
    component.page = mockCurrentPage;
    component.perPage = mockPageSize;
    let isLastPage = component.lastPage();
    expect(isLastPage).toBe(false);
  });

  it('should be defined - the page is last page', () => {
    component.count = mockRecordCount;
    component.page = mockEndPage;
    component.perPage = mockPageSize;
    let isLastPage = component.lastPage();
    expect(isLastPage).toBe(true);
  });

  it('should be displayed the four pages along with current page', () => {
    component.count = mockRecordCount;
    component.page = mockCurrentPage;
    component.perPage = mockPageSize;
    component.pagesToShow = 4;
    let returnPages: number[] = [];
    returnPages = component.getPages();
    expect(component.getPages).toHaveBeenCalled;
    expect(expectedPages).toEqual(returnPages);
  });

  it('should be displayed last four pages if the current page is last page', () => {
    component.count = mockRecordCount;
    component.page = mockEndPage
    component.perPage = mockPageSize;
    component.pagesToShow = 4;
    let returnPages: number[] = [];
    returnPages = component.getPages();
    expect(component.getPages).toHaveBeenCalled;
    expect(expectedLastPages).toEqual(returnPages);
  });
});
