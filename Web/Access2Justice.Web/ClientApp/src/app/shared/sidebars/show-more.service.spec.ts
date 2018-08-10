import { Observable } from 'rxjs/Rx';
import { api } from '../../../api/api';
import { HttpHeaders, HttpClientModule } from '@angular/common/http';
import { ShowMoreService } from './show-more.service';
import { NavigateDataService } from '../navigate-data.service';
import { PaginationService } from '../search/pagination.service';
import { TestBed } from '@angular/core/testing';
import { Router, RouterModule } from '@angular/router';
import { MapLocation } from '../location/location';
import { TopicsResourcesComponent } from '../../topics-resources/topics-resources.component';
import { SubtopicsComponent } from '../../topics-resources/subtopic/subtopics.component';
import { IResourceFilter } from '../search/search-results/search-results.model';

describe('Service:ServiceOrgService', () => {
  let showMoreService: ShowMoreService;
  let navigateDataService: NavigateDataService;
  let paginationService: PaginationService;
  let router: Router;

  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  let mockActiveId: any = "5d7f773f-ef75-4fb7-9681-cc7c81dc2be7";
  let route = {
    navigate: jasmine.createSpy('navigate')
  }
  let topIntent = 'test';
  let mockResourceType = 'Organizations';
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
    IsResourceCountRequired: false
  };
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
