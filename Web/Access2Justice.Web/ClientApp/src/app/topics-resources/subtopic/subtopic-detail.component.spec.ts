import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { SubtopicDetailComponent } from './subtopic-detail.component';
import { SaveButtonComponent } from '../../shared/resource/user-action/save-button/save-button.component';
import { ShareButtonComponent } from '../../shared/resource/user-action/share-button/share-button.component';
import { PrintButtonComponent } from '../../shared/resource/user-action/print-button.component';
import { ResourceCardComponent } from '../../shared/resource/resource-card/resource-card.component';
import { GuidedAssistantSidebarComponent } from '../../shared/sidebars/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../../shared/sidebars/service-org-sidebar.component';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { TopicService } from '../shared/topic.service';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ServiceOrgService } from '../../shared/sidebars/service-org.service';
import { LocationService } from '../../shared/location/location.service';
import { PaginationService } from '../../shared/search/pagination.service';
import { SearchService } from '../../shared/search/search.service';
import { ActivatedRoute, Route, ActivatedRouteSnapshot, UrlSegment, Params, Data, Router } from '@angular/router';
import { MapLocation } from '../../shared/location/location';
import 'rxjs/add/observable/from';
import { Observable } from 'rxjs/Observable'; 

describe('SubtopicDetailComponent', () => {
  let component: SubtopicDetailComponent;
  let fixture: ComponentFixture<SubtopicDetailComponent>;
  let topicService: TopicService;
  let activeRoute: ActivatedRoute;
  let locationService: LocationService;
  let paginationService: PaginationService;
  let searchService: SearchService;
  let navigateDataService: NavigateDataService;
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

  beforeEach(async(() => {
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
        TopicService,
        NavigateDataService,
        ServiceOrgService,
        LocationService,
        SearchService,
        PaginationService,
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
        } 
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubtopicDetailComponent);
    paginationService = TestBed.get(PaginationService);
    searchService = TestBed.get(SearchService);
    locationService = TestBed.get(LocationService);
    navigateDataService = TestBed.get(NavigateDataService); 
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it("should call clickShowMore when Show More button is clicked", () => {
    component.topIntent = 'test2';
    spyOn(locationService, 'updateLocation').and.returnValue(mockMapLocation);
    spyOn(paginationService, 'getPagedResources').and.callFake(() => {
      return Observable.from([searchResults]);
    })
    const result = 'test';
    spyOn(navigateDataService, 'setData').and.callFake(() => {
      return Observable.from([result]);
    })
    component.clickShowMore(resourceType);
    expect(router.navigate).toHaveBeenCalledWith(['/search']);
    expect(component.clickShowMore).toBeTruthy(['']);
  });

});
