import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { SearchResultsComponent } from './search-results.component';
import { SearchFilterComponent } from '../search-filter/search-filter.component';
import { ResourceCardComponent } from '../../resource/resource-card/resource-card.component';
import { GuidedAssistantSidebarComponent } from '../../sidebars/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../../sidebars/service-org-sidebar.component';
import { SaveButtonComponent } from '../../resource/user-action/save-button.component';
import { ShareButtonComponent } from '../../resource/user-action/share-button.component';
import { NavigateDataService } from '../../navigate-data.service';
import { WebResourceComponent } from './web-resource/web-resource.component';
import { PaginationComponent } from '../../../shared/pagination/pagination.component';
import { ResourceResult } from './search-result';
import { IResourceFilter, ILuisInput } from './search-results.model';
import { SearchFilterPipe } from '../search-filter.pipe';
import { SearchService } from '../search.service';
import { PaginationService } from '../pagination.service';
import { ServiceOrgService } from '../../sidebars/service-org.service';
import { LocationService } from '../../location/location.service';
import { JitCompiler } from '@angular/compiler/src/jit/compiler';
import { Observable } from 'rxjs';

describe('SearchResultsComponent', () => {
  let component: SearchResultsComponent;
  let fixture: ComponentFixture<SearchResultsComponent>;
  let searchService: SearchService;
  let paginationService: PaginationService;
  let navigateDataService: NavigateDataService;
  let resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', ResourceIds: '', PageNumber: 0, Location: '' };
  let currentPage: number = 0;
  let mockPageNumber: number = 0;
  let mockResourceName = 'Videos'
  let mockResourceNameUndifined = 'NoVideos'
  let mockResourceTypeFilter = [{ ResourceName: "Videos", ResourceCount: 1 }, { ResourceName: "Organizations", ResourceCount: 2 }, { ResourceName: "Forms", ResourceCount: 1 }];
  let mockResourceTypeFilterTemp = [{ ResourceName: "test", ResourceCount: 1 }, { ResourceName: "test", ResourceCount: 2 }, { ResourceName: "test", ResourceCount: 1 }];
  let mockResourceListFilterAll = [{ ResourceCount: 10, ResourceList: [{ continuationToken: {}, resources: [{ address: "Houston County, Texas, United States", name: "Tenant Action Plan for Eviction" }, { address: "Houston County, Texas, United States", name: "Landlord Action Plan" }] }], ResourceName: "All" }];
  let mockResourceTypeFilterWithResourceList = [{ ResourceList: [{ continuationToken: [{ test: '2' }], resources: [{ address: "Houston County, Texas, United States", name: "Tenant Action Plan for Eviction" }, { address: "Houston County, Texas, United States", name: "Landlord Action Plan" }] }] }, { ResourceName: "Organizations", ResourceCount: 2 }, { ResourceName: "Forms", ResourceCount: 1 }];
  let mockResourceTypeFilter2 = [{ ResourceName: "Videos", ResourceCount: 1, ResourceList: [{ continuationToken: [{ test: '2' }], resources: [{ address: "Houston County, Texas, United States", name: "Tenant Action Plan for Eviction" }, { address: "Houston County, Texas, United States", name: "Landlord Action Plan" }] }] }, { ResourceName: "Organizations", ResourceCount: 2 }, { ResourceName: "Forms", ResourceCount: 1 }];
  let mockSearchText = 'eviction'
  let mockSearchResults2 = [{ continuationToken: [{ test: '2' }], resources: [{ address: "Houston County, Texas, United States", name: "Tenant Action Plan for Eviction" }, { address: "Houston County, Texas, United States", name: "Landlord Action Plan" }] }]

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        SearchResultsComponent,
        SearchFilterComponent,
        SearchFilterPipe,
        ResourceCardComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        WebResourceComponent,
        PaginationComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'search', component: SearchResultsComponent }
        ]),
        HttpClientModule
      ],
      providers: [NavigateDataService,
        SearchService,
        PaginationService,
        ServiceOrgService,
        LocationService,
        { provide: APP_BASE_HREF, useValue: '/' }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    })
      .compileComponents();

  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    // SearchService provided to the TestBed
    searchService = TestBed.get(SearchService);
    paginationService = TestBed.get(PaginationService);
    navigateDataService = TestBed.get(NavigateDataService);

    component.isWebResource = false;
    component.isInternalResource = true;
    component.isLuisResponse = false;
    component.pagesToShow = 10;
    component.searchResults = {};
    component.total = 0;
    component.page = 1;
    component.limit = 0;
    component.offset = 0;
    component.resourceResults = [{}];
    component.searchResults = {
      resources: {}, webResources: { webPages: { value: {} } }, topIntent: ''
    };

  });

  it('should create search result component', () => {
    expect(component).toBeTruthy();
  });

  it('should define search result component', () => {
    expect(component).toBeDefined();
  });

  it('should call filterSearchResults with event defined', () => {
    spyOn(component, 'getInternalResource');
    let event = { filterParam: "All" };
    component.filterSearchResults(event);
    expect(component.getInternalResource).toHaveBeenCalled();
  });

  it('should call filterSearchResults with event undefined', () => {
    spyOn(component, 'getInternalResource');
    component.filterSearchResults(event);
    expect(component.getInternalResource).toHaveBeenCalledTimes(0);
  });

  it('should call checkResource with resource name defined and resource list undefined', () => {
    component.resourceTypeFilter = mockResourceTypeFilter;
    component.currentPage = mockPageNumber;
    component.checkResource(mockResourceName, mockPageNumber);
    expect(component.checkResource(mockResourceName, mockPageNumber)).toBeTruthy();
  });

  it('should call checkResource with resource name undefined and resource list undefined', () => {
    component.resourceTypeFilter = mockResourceTypeFilterTemp;
    component.currentPage = mockPageNumber;
    component.checkResource(mockResourceName, mockPageNumber);
    expect(component.checkResource(mockResourceName, mockPageNumber)).toBeFalsy();
  });

  it('should call checkResource with resource name defined and resource list defined', () => {
    component.resourceTypeFilter = mockResourceListFilterAll;
    component.currentPage = mockPageNumber;
    component.checkResource(mockResourceName, mockPageNumber);
    expect(component.checkResource(mockResourceName, mockPageNumber)).toBeFalsy();
  });

  it('should call AddResource with resource name defined', () => {
    component.resourceTypeFilter = mockResourceTypeFilter;
    component.searchResults = mockSearchResults2;
    component.addResource(mockResourceName);
    expect(component.addResource).toBeTruthy();
  });

  it('should call AddResource with resource name defined and resource list defined ', () => {
    component.resourceTypeFilter = mockResourceTypeFilter2;
    component.searchResults = mockResourceTypeFilterWithResourceList;
    component.addResource(mockResourceName);
    expect(component.addResource).toBeTruthy();
  });

  it('should call AddResource with resource name undefined and resource list defined', () => {
    component.resourceTypeFilter = mockResourceTypeFilter2;
    component.addResource(mockResourceNameUndifined);
    expect(component.addResource).toBeTruthy();
  });

  it('should call AddResource with resource list undefined', () => {
    component.resourceTypeFilter = mockResourceTypeFilter;
    component.addResource(mockResourceNameUndifined);
    expect(component.addResource).toBeTruthy();
  });

  it('should call Search Resource by offset', () => {
    spyOn(paginationService, 'searchByOffset').and.returnValue(Observable.of());
    component.searchText = mockSearchText;
    component.offset = 1
    component.searchResource(1);
    expect(paginationService.searchByOffset).toHaveBeenCalled();
  });

  it('should call goToPage - web resource page', () => {
    spyOn(component, 'getInternalResource');
    component.goToPage(currentPage);
    expect(component.getInternalResource).toHaveBeenCalled();
  });

  it("should call bindData and notifyLocationChange on ngOnInit", () => {
    spyOn(component, 'bindData');
    spyOn(component, 'notifyLocationChange');
    component.ngOnInit();
    component.bindData();
    component.notifyLocationChange();
    expect(component.bindData).toHaveBeenCalled();
    expect(component.notifyLocationChange).toHaveBeenCalled();
  });

});
