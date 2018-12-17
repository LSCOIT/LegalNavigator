import { ActivatedRoute, Router } from '@angular/router';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { GuidedAssistantSidebarComponent } from '../../../sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { MapService } from '../../../map/map.service';
import { NavigateDataService } from '../../../navigate-data.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { PaginationService } from '../../../pagination/pagination.service';
import { ServiceOrgSidebarComponent } from '../../../sidebars/service-org-sidebar/service-org-sidebar.component';
import { VideosComponent } from './videos.component';
import { Global } from '../../../../global';
import { StateCodeService } from '../../../state-code.service';
import { ShowMoreService } from '../../../sidebars/show-more/show-more.service';

describe('VideosComponent', () => {
  let component: VideosComponent;
  let fixture: ComponentFixture<VideosComponent>;
  let mockRouter;
  let mockShowMoreService;
  let mockGlobal;
  let mockResource =
  {
    "id": "6f6511e3-4c15-4664-9125-4d025ec52cf5",
    "name": "Moving In",
    "type": "Evictions and Tenant Issues",
    "description": "Conversations about Landlord Tenant Law in Alaska",
    "resourceType": "Videos",
    "url": "https://www.youtube.com/watch?v=3g1Tu2Ulrk0",
    "topicTags": [
      {
        "id": "62a93f03-8234-46f1-9c35-b3146a96ca8b"
      }
    ],
    "location": [
      {
        "state": "Hawaii",
        "city": "Kalawao",
        "zipCode": "96761"
      }
    ],
    "icon": "./assets/images/resources/resource.png",
    "overview": "This video covers some issues everyone should think about when moving into a new rental housing."
  };

  beforeEach(async(() => {
    mockShowMoreService = jasmine.createSpyObj(['clickSeeMoreOrganizations']);

    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [
        VideosComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent
      ],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { params: { 'id': '123' } } }
        },
        { provide: Router, useValue: mockRouter },
        MapService,
        NavigateDataService,
        PaginationService,
        { provide: Global, useValue: { mockGlobal, activeSubtopicParam: '123', topIntent: 'Divorce' } },
        StateCodeService,
        { provide: ShowMoreService, useValue: mockShowMoreService }
      ]
    });
    TestBed.compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VideosComponent);
    component = fixture.componentInstance;
    component.resource = mockResource;
    mockShowMoreService = TestBed.get(ShowMoreService);
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('should call resourceUrl on ngOnInit', () => {
    spyOn(component, 'resourceUrl');
    component.ngOnInit();
    expect(component.resourceUrl).toHaveBeenCalledWith("https://www.youtube.com/watch?v=3g1Tu2Ulrk0");
  });

  it('should derive a new url for YouTube videos', () => {
    component.resourceUrl(mockResource.url);
    expect(component.url).toEqual("https://www.youtube.com/embed/3g1Tu2Ulrk0");
  });

  it('should call clickSeeMoreOrganizations service method', () => {
    component.clickSeeMoreOrganizationsFromVideos("test");
    expect(mockShowMoreService.clickSeeMoreOrganizations).toHaveBeenCalled();
  });
});
