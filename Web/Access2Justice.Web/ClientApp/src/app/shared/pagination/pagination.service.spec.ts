import { TestBed, inject } from '@angular/core/testing';
import { PaginationService } from './pagination.service';
import { Observable } from 'rxjs/Rx';
import { api } from '../../../api/api';
import { HttpHeaders } from '@angular/common/http';
import { IResourceFilter } from '../search/search-results/search-results.model';

describe('PaginationService', () => {
  let service: PaginationService;
  const mockHttpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  let mockResourceType = 'test';
  let mockSearchText = "Lorem ipsum solor sit amet bibodem consecuter orem ipsum"; 
  let mockResponse = Observable.of(mockSearchText);
  let mockOffset = 5;
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  let mockContinuationToken = 'test';
  let mockTopicIds = ['test'];
  let mockResourceIds = ['test'];
  let mockLocation = 'test';
  let resourceInput: IResourceFilter = {
    ResourceType: mockResourceType,
    ContinuationToken: mockContinuationToken,
    TopicIds: mockTopicIds,
    ResourceIds: mockResourceIds,
    PageNumber: 1,
    Location: mockLocation,
    IsResourceCountRequired: false,
    IsOrder: true,
    OrderByField: "name",
    OrderBy: "ASC"
  }; 
  
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
    resourceInput.ResourceType = '';
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });

  it('should not fail PagedResources when the ContinuationToken is null in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    resourceInput.ContinuationToken = '';
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });

  it('should not fail PagedResources when the TopicIds are null in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    resourceInput.TopicIds = [];
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });

  it('should not fail PagedResources when the ResourceIds are null in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    resourceInput.ResourceIds = [];
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });

  it('should not fail PagedResources when the PageNumber is zero in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    resourceInput.PageNumber = 0;
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });

  it('should not fail PagedResources when the Location is null in resourceInput', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    resourceInput.Location = '';
    service.getPagedResources(resourceInput).subscribe(pagedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(pagedResource).toEqual(mockSearchText);
      done();
    });
  });
});
