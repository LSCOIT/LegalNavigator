import { TestBed, inject } from '@angular/core/testing';
import { PaginationService } from './pagination.service';
import { Observable } from 'rxjs/Rx';
import { api } from '../../../api/api';
import { HttpHeaders } from '@angular/common/http';
import { IResourceFilter } from '../search/search-results/search-results.model';

//describe('PaginationService', () => {
//  beforeEach(() => {
//    TestBed.configureTestingModule({
//      providers: [PaginationService]
//    });
//  });

//  it('should be created', inject([PaginationService], (service: PaginationService) => {
//    expect(service).toBeTruthy();
//  }));
//});

describe('PaginationService', () => {
  let service: PaginationService;
  const mockHttpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  let mockSearchText = "Lorem ipsum solor sit amet bibodem consecuter orem ipsum"; // solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter."
  let mockResponse = Observable.of(mockSearchText);
  let mockOffset = 5;
  let resourceInput: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', ResourceIds: '', PageNumber: 1, Location: '' };
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  let mockResoureFilter = 'test';
  beforeEach(() => {
    service = new PaginationService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('should create pagination service component', () => {
    expect(service).toBeTruthy();
  });

  it('should define pagination service component', () => {
    expect(service).toBeDefined();
  });

  it('should retrieve text from the API-getPagedResources when the search text is entered', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });

  it('should search text from the API when we provide search text ', (done) => {
    httpSpy.get.and.returnValue(mockResponse);
    service.searchByOffset(mockSearchText, mockOffset).subscribe(searchOffSet => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(httpSpy.get).toHaveBeenCalled();
      expect(searchOffSet).toEqual(mockSearchText);
      done();
    });
  });

  it('should not fail PagedResources when the resource type is null in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    let resourceInput: IResourceFilter = {
      ResourceType: '',
      ContinuationToken: mockResoureFilter,
      TopicIds: mockResoureFilter,
      ResourceIds: mockResoureFilter,
      PageNumber: 1,
      Location: mockResoureFilter
    };
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });

  it('should not fail PagedResources when the ContinuationToken is null in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    let resourceInput: IResourceFilter = {
      ResourceType: mockResoureFilter,
      ContinuationToken: '',
      TopicIds: mockResoureFilter,
      ResourceIds: mockResoureFilter,
      PageNumber: 1,
      Location: mockResoureFilter
    };
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });

  it('should not fail PagedResources when the TopicIds are null in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    let resourceInput: IResourceFilter = {
      ResourceType: mockResoureFilter,
      ContinuationToken: mockResoureFilter,
      TopicIds: '',
      ResourceIds: mockResoureFilter,
      PageNumber: 1,
      Location: mockResoureFilter
    };
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });
  it('should not fail PagedResources when the ResourceIds are null in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    let resourceInput: IResourceFilter = {
      ResourceType: mockResoureFilter,
      ContinuationToken: mockResoureFilter,
      TopicIds: mockResoureFilter,
      ResourceIds: '',
      PageNumber: 1,
      Location: mockResoureFilter
    };
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });
  it('should not fail PagedResources when the PageNumber is zero in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    let resourceInput: IResourceFilter = {
      ResourceType: mockResoureFilter,
      ContinuationToken: mockResoureFilter,
      TopicIds: mockResoureFilter,
      ResourceIds: '',
      PageNumber: 0,
      Location: mockResoureFilter
    };
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });
  it('should not fail PagedResources when the Location is null in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    let resourceInput: IResourceFilter = {
      ResourceType: mockResoureFilter,
      ContinuationToken: mockResoureFilter,
      TopicIds: mockResoureFilter,
      ResourceIds: '',
      PageNumber: 1,
      Location: mockResoureFilter
    };
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });
});
