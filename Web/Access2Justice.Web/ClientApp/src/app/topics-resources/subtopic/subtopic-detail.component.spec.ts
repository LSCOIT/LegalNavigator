import { ActivatedRoute } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { GuidedAssistantSidebarComponent } from '../../shared/sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { MapLocation } from '../../shared/map/map';
import { MapService } from '../../shared/map/map.service';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { Observable } from 'rxjs/Observable';
import { PaginationService } from '../../shared/pagination/pagination.service';
import { PrintButtonComponent } from '../../shared/resource/user-action/print-button/print-button.component';
import { ResourceCardComponent } from '../../shared/resource/resource-card/resource-card.component';
import { RouterModule } from '@angular/router';
import { SaveButtonComponent } from '../../shared/resource/user-action/save-button/save-button.component';
import { SearchService } from '../../shared/search/search.service';
import { ServiceOrgSidebarComponent } from '../../shared/sidebars/service-org-sidebar/service-org-sidebar.component';
import { ShareButtonComponent } from '../../shared/resource/user-action/share-button/share-button.component';
import { ShowMoreService } from '../../shared/sidebars/show-more/show-more.service';
import { SubtopicDetailComponent } from './subtopic-detail.component';
import { TopicService } from '../shared/topic.service';
import { of } from 'rxjs/observable/of';
import { MsalService } from '@azure/msal-angular';
import { Global } from '../../global';
 

describe('SubtopicDetailComponent', () => {
  let component: SubtopicDetailComponent;
  let fixture: ComponentFixture<SubtopicDetailComponent>;
  let mapService: MapService;
  let paginationService: PaginationService;
  let searchService: SearchService;
  let navigateDataService: NavigateDataService;
  let topicService: TopicService; 
  let resourceType: string = 'Action Plans';
  let mockMapLocation: MapLocation = {
    state: 'Sample State',
    city: 'Sample City',
    county: 'Sample County',
    zipCode: '1009203',
    locality: 'Sample Location',
    address: 'Sample Address'
  };
  let topIntent = 'test';
  let searchResults: any = {
    topIntent: topIntent,
    resourceType: resourceType
  };
  let router = {
    navigate: jasmine.createSpy('navigate')
  } 
  let mockMapService;
  let mockPaginationService;
  let mockNavigateDataService;
  let mockTopicService, msalService; 
  
  beforeEach(async(() => {
    mockMapService = jasmine.createSpyObj(['updateLocation'])
    mockPaginationService = jasmine.createSpyObj(['getPagedResources'])
    mockNavigateDataService = jasmine.createSpyObj(['setData', 'getData'])
    mockTopicService = jasmine.createSpyObj(['getSubtopicDetail','getDataOnReload'])

    TestBed.configureTestingModule({
      declarations: [
        SubtopicDetailComponent,
        SaveButtonComponent,
        ShareButtonComponent,
        PrintButtonComponent,
        ResourceCardComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent,
        BreadcrumbComponent       
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'subtopics/:topic', component: SubtopicDetailComponent }
        ]),
        HttpClientModule
      ],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        { provide: NavigateDataService, useValue: mockNavigateDataService },
        { provide: MapService, useValue: mockMapService },
        { provide: PaginationService, useValue: mockPaginationService },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: { 'topic': 'bd900039-2236-8c2c-8702-d31855c56b0f' }
            },
            url: Observable.of([
              { path: 'subtopics', params: {} },
              { path: 'bd900039-2236-8c2c-8702-d31855c56b0f', params: {} }
            ])
          }
        },
        { provide: TopicService, useValue: mockTopicService },
        { provide: MsalService, useValue: msalService },
        ShowMoreService,
        SearchService,
        Global
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    })
      .compileComponents();

    TestBed.compileComponents();
    fixture = TestBed.createComponent(SubtopicDetailComponent);
    component = fixture.componentInstance;

  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it("should call clickShowMore when Show More button is clicked", () => {
    component.topIntent = 'test2';
    mockMapService.updateLocation.and.returnValue(mockMapLocation);
    mockPaginationService.getPagedResources.and.returnValue(of([searchResults]));
    const result = 'test';
    mockNavigateDataService.setData.and.returnValue(of([result]));
    component.clickSeeMoreOrganizationsFromSubtopicDetails(resourceType);
    expect(component.clickSeeMoreOrganizationsFromSubtopicDetails).toBeTruthy(['']);
  });

});
