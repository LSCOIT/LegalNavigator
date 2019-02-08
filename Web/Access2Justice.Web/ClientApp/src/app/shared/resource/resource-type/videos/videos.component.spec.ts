import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';

import { Global } from '../../../../global';
import { MapService } from '../../../map/map.service';
import { PaginationService } from '../../../pagination/pagination.service';
import { NavigateDataService } from '../../../services/navigate-data.service';
import { StateCodeService } from '../../../services/state-code.service';
import { GuidedAssistantSidebarComponent } from '../../../sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../../../sidebars/service-org-sidebar/service-org-sidebar.component';
import { ShowMoreService } from '../../../sidebars/show-more/show-more.service';
import { VideosComponent } from './videos.component';

describe('VideosComponent', () => {
  let component: VideosComponent;
  let fixture: ComponentFixture<VideosComponent>;
  let mockRouter;
  let mockShowMoreService;
  let mockGlobal;
  const mockVideoResource = {
    id: '6f6511e3-4c15-4664-9125-4d025ec52cf5',
    name: 'Moving In',
    type: 'Evictions and Tenant Issues',
    description: 'Conversations about Landlord Tenant Law in Alaska',
    resourceType: 'Videos',
    url: 'https://www.youtube.com/watch?v=3g1Tu2Ulrk0',
    topicTags: [
      {
        id: '62a93f03-8234-46f1-9c35-b3146a96ca8b'
      }
    ],
    location: [
      {
        state: 'Hawaii',
        city: 'Kalawao',
        zipCode: '96761'
      }
    ],
    icon: './assets/images/resources/resource.png',
    overview:
      'This video covers some issues everyone should think about when moving into a new rental housing.'
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
        MapService,
        NavigateDataService,
        PaginationService,
        StateCodeService,
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                id: '123'
              }
            }
          }
        },
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: Global,
          useValue: {
            mockGlobal,
            activeSubtopicParam: '123',
            topIntent: 'Divorce'
          }
        },
        {
          provide: ShowMoreService,
          useValue: mockShowMoreService
        }
      ]
    });
    TestBed.compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VideosComponent);
    component = fixture.componentInstance;
    component.resource = mockVideoResource;
    mockShowMoreService = TestBed.get(ShowMoreService);
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('should call resourceUrl on ngOnInit', () => {
    spyOn(component, 'resourceUrl');
    component.ngOnInit();
    expect(component.resourceUrl).toHaveBeenCalledWith('https://www.youtube.com/watch?v=3g1Tu2Ulrk0');
  });

  it('should transform resource url for embedding single YouTube video', () => {
    component.resourceUrl(mockVideoResource.url);
    expect(component.url).toEqual('https://www.youtube.com/embed/3g1Tu2Ulrk0');
  });

  it('should transform resource url for embedding YouTube playlist', () => {
    component.resourceUrl('https://www.youtube.com/watch?v=VIDEO_ID&list=LIST_ID&index=3');
    expect(component.url).toEqual('https://www.youtube.com/embed/videoseries?list=LIST_ID&index=3');
  });

  it('should call clickSeeMoreOrganizations service method', () => {
    component.clickSeeMoreOrganizationsFromVideos('test');
    expect(mockShowMoreService.clickSeeMoreOrganizations).toHaveBeenCalled();
  });
});
