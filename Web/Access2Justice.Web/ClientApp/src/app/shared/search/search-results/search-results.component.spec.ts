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

describe('SearchResultsComponent', () => {
  let component: SearchResultsComponent;
  let fixture: ComponentFixture<SearchResultsComponent>;
  let pagesToShow: number;
  let searchService: SearchService;
  let paginationService: PaginationService;
  let navigateDataService: NavigateDataService;

  let isInternalResource: boolean;
  let isWebResource: boolean;
  let isLuisResponse: boolean;
  let searchText: string;
  let searchResults: any;
  let sortType: any;
  let resourceResults: ResourceResult[] = [];
  let filterType: string = 'All';
  let resourceTypeFilter: any[];
  let resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: '', ResourceIds: '', PageNumber: 0, Location: '' };
  let topicIds: any[];
  let isServiceCall: boolean;
  let currentPage: number = 0;

  let loading = false;
  let total: number;
  let page: number;
  let limit: number;
  let offset: number;
  let mockResourceName = 'Videos'
  let mockResourceTypeFilter = [{ ResourceName: "Videos", ResourceCount: 1 }, { ResourceName: "Organizations", ResourceCount: 2 }, { ResourceName: "Forms", ResourceCount: 1 }];
  let mockResourceTypeFilterTemp = [{ ResourceName: "test", ResourceCount: 1 }, { ResourceName: "test", ResourceCount: 2 }, { ResourceName: "test", ResourceCount: 1 }];
  let mockResourceListFilterAll = [{ ResourceCount: 10, ResourceList: [{ continuationToken: {}, resources: [{ address: "Houston County, Texas, United States", name: "Tenant Action Plan for Eviction" }, { address: "Houston County, Texas, United States", name: "Landlord Action Plan" }] }], ResourceName: "All" }];

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
    let mockPageNumber = 0;
    component.resourceTypeFilter = mockResourceTypeFilter;
    component.currentPage = 0;
    component.checkResource(mockResourceName, mockPageNumber);
    expect(component.checkResource(mockResourceName, 0)).toBeTruthy;
  });

  it('should call checkResource with resource name undefined and resource list undefined', () => {
    let mockPageNumber = 0;
    component.resourceTypeFilter = mockResourceTypeFilterTemp;
    component.currentPage = 0;
    component.checkResource(mockResourceName, mockPageNumber);
    expect(component.checkResource(mockResourceName, 0)).toBeFalsy;
  });

  it('should call checkResource with resource name defined and resource list defined', () => {
    let mockPageNumber = 0;
    component.resourceTypeFilter = mockResourceListFilterAll;
    component.currentPage = 0;
    component.checkResource(mockResourceName, mockPageNumber);
    expect(component.checkResource(mockResourceName, 0)).toBeFalsy;
  });
    
  it('should call AddResource with resource list defined', () => {

  });

  it('should call AddResource with resource list undefined', () => {

  });

  it('should call Search Resource by offset', () => {

  });

  it('should call check Resource by resource name and page number', () => {

  });

  it('should call gotoPage', () => {

  });

  it('should call onNext', () => {

  });

  it('should call onPrevious', () => {

  });

  it('should call notifyLocationChange', () => {

  });

  it('should call mapInternalResource', () => {

  });

  it('should call bindData', () => {

  });



});
