import { ShowMoreService } from './show-more.service';
import { NavigateDataService } from '../../navigate-data.service';
import { PaginationService } from '../../pagination/pagination.service';
import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

describe('Service:ServiceOrgService', () => {
  let showMoreService: ShowMoreService;
  let navigateDataService: NavigateDataService;
  let paginationService: PaginationService;
  let router: Router;

  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  let mockActiveId: any = "5d7f773f-ef75-4fb7-9681-cc7c81dc2be7";
  let mockResourceType = 'Organizations';
  let mockTopicIds = ['test'];

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        showMoreService,
        NavigateDataService,
        PaginationService]
    });
    showMoreService = new ShowMoreService(httpSpy, navigateDataService, router, paginationService);
    navigateDataService = new NavigateDataService();
    paginationService = new PaginationService(httpSpy);
  });

  it('should create service organization service component', () => {
    expect(ShowMoreService).toBeTruthy();
  });

  it('should define service organization service component', () => {
    expect(ShowMoreService).toBeDefined();
  });

  it("should call clickSeeMoreOrganizations when See More button is clicked", () => {
    spyOn(paginationService, 'getPagedResources');
    spyOn(showMoreService, 'clickSeeMoreOrganizations');
    showMoreService.resourceFilter.TopicIds = mockTopicIds;
    showMoreService.resourceFilter.Location = 'Test';
    showMoreService.resourceFilter.IsResourceCountRequired = false;
    showMoreService.clickSeeMoreOrganizations(mockResourceType, mockActiveId);
    expect(showMoreService.clickSeeMoreOrganizations).toHaveBeenCalled();
  });
});
