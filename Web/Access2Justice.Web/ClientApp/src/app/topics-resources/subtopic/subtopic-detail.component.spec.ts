import { APP_BASE_HREF } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { BsDropdownModule } from 'ngx-bootstrap';
import { Observable, of } from 'rxjs';

import { Global } from '../../global';
import { MapService } from '../../common/map/map.service';
import { PaginationService } from '../../common/pagination/pagination.service';
import { ResourceCardComponent } from '../../common/resource/resource-card/resource-card.component';
import { PrintButtonComponent } from '../../common/resource/user-action/print-button/print-button.component';
import { SaveButtonComponent } from '../../common/resource/user-action/save-button/save-button.component';
import { ShareButtonComponent } from '../../common/resource/user-action/share-button/share-button.component';
import { SearchService } from '../../common/search/search.service';
import { NavigateDataService } from '../../common/services/navigate-data.service';
import { GuidedAssistantSidebarComponent } from '../../common/sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../../common/sidebars/service-org-sidebar/service-org-sidebar.component';
import { ShowMoreService } from '../../common/sidebars/show-more/show-more.service';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { TopicService } from '../shared/topic.service';
import { SubtopicDetailComponent } from './subtopic-detail.component';
import { SharedModule } from '../../shared/shared.module';

describe('SubtopicDetailComponent', () => {
  let component: SubtopicDetailComponent;
  let fixture: ComponentFixture<SubtopicDetailComponent>;
  let resourceType: string = 'Action Plans';
  let mockSubTopicDetailData = [
    {
      id: '88b5060a-61e7-4739-aad2-df76a088fe35',
      name: 'Self-Help Centers & Access to Justice Rooms',
      organizationalUnit: 'Hawaii',
      resourceType: 'Related Links',
      url:
        'https://www.lawhelp.org/hi/resource/self-help-centers-access-to-justice-rooms'
    },
    {
      id: '64926e4e-2ea1-4d47-9b00-9e8f1bec501c',
      name: 'Family Court - What to do if Your Projective Order is Violated',
      organizationalUnit: 'Hawaii',
      resourceType: 'Additional Readings',
      url:
        'http://www.courts.state.hi.us/self-help/protective_orders/violations/order_violations'
    }
  ];
  let mockgetData = [];
  let mockGetDocumentData = {
    icon:
      'https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/categories/family.svg | https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/categories/abuse.svg',
    id: '567d4bb8-a228-4281-b7af-8acc61b89754',
    keywords:
      'Domestic Violence | Dating Violence | Abuse | Violence | Name-calling | Emotional Abuse | Verbal Abuse | Temporary Restraining Order | TRO | Threats | Sexual Abuse | Physical Abuse | Protection Order | Financial Abuse | Stalking | Sexual Assault | Safety Plan | Emergency Shelter',
    name: 'Domestic Violence',
    organizationalUnit: 'Hawaii',
    overview:
      'Domestic violence and emotional abuse are behaviors used in an intimate or dating relationship to control the other. Partners may be married or not; heterosexual or gay, lesbian or transgendered; living together, separated or dating. Some common examples are hitting, shoving, kicking, slapping, threatening to harm, throwing objects, scaring the other person, name-calling and stalking. ↵↵If you are in immediate danger, call 911. For domestic violence assistance, call the Domestic Violence Action Center legal helpline at 808-531-3771 or 1-800-690-6200 (Toll-free). It is available between 8:30am-4:30pm Mon-Fri. ↵↵↵For 24/7 assistance call:↵Oahu: 808-841-0822 or 808-526-2200 or 808-528-0606↵Kauai: 808-245-6362↵Maui: 808-579-9581 or 808-873-8624↵Lanai: 808-565-6700↵Molokai: 808-567-6888↵Hawaii Island: 808-959-8864 or 808-322-7233↵National hotline: 1-800-799-7233',
    resourceType: 'Topics'
  };
  let global;
  let mockMapService;
  let mockPaginationService;
  let mockNavigateDataService;
  let mockTopicService, msalService;
  let mockShowMoreService;

  beforeEach(async(() => {
    mockMapService = jasmine.createSpyObj(['updateLocation']);
    mockPaginationService = jasmine.createSpyObj(['getPagedResources']);
    mockNavigateDataService = jasmine.createSpyObj(['setData', 'getData']);
    mockTopicService = jasmine.createSpyObj([
      'getSubtopicDetail',
      'getDocumentData'
    ]);
    mockTopicService.getSubtopicDetail.and.returnValue(
      of(mockSubTopicDetailData)
    );
    mockTopicService.getDocumentData.and.returnValue(of([mockGetDocumentData]));
    mockNavigateDataService.getData.and.returnValue(of(mockgetData));
    mockShowMoreService = jasmine.createSpyObj(['clickSeeMoreOrganizations']);

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
          {
            path: 'subtopics/:topic',
            component: SubtopicDetailComponent
          }
        ]),
        HttpClientModule,
        SharedModule,
        BsDropdownModule.forRoot()
      ],
      providers: [
        SearchService,
        {
          provide: APP_BASE_HREF,
          useValue: '/'
        },
        {
          provide: NavigateDataService,
          useValue: mockNavigateDataService
        },
        {
          provide: MapService,
          useValue: mockMapService
        },
        {
          provide: PaginationService,
          useValue: mockPaginationService
        },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                topic: 'bd900039-2236-8c2c-8702-d31855c56b0f'
              }
            },
            url: Observable.of([
              {
                path: 'subtopics',
                params: {}
              },
              {
                path: 'bd900039-2236-8c2c-8702-d31855c56b0f',
                params: {}
              }
            ])
          }
        },
        {
          provide: TopicService,
          useValue: mockTopicService
        },
        {
          provide: MsalService,
          useValue: msalService
        },
        {
          provide: ShowMoreService,
          useValue: mockShowMoreService
        },
        {
          provide: Global,
          useValue: {
            global,
            displayResources: 5
          }
        }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    }).compileComponents();

    TestBed.compileComponents();
    fixture = TestBed.createComponent(SubtopicDetailComponent);
    component = fixture.componentInstance;
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call clickShowMore when Show More button is clicked', () => {
    component.topIntent = 'test2';
    component.activeSubtopicParam = '123';
    component.clickSeeMoreOrganizationsFromSubtopicDetails(resourceType);
    expect(mockShowMoreService.clickSeeMoreOrganizations).toHaveBeenCalled();
  });

  it('should call filterSubtopicDetail in getSubtopicDetail method', () => {
    component.activeSubtopicParam = 'bd900039-2236-8c2c-8702-d31855c56b0f';
    spyOn(component, 'filterSubtopicDetail');
    component.getSubtopicDetail();
    expect(component.subtopicDetails).toEqual(mockSubTopicDetailData);
    expect(component.filterSubtopicDetail).toHaveBeenCalled();
  });

  it('should assign component values in getDataOnReload method', () => {
    let mockGuidedSutopicDetailsInput = {
      activeId: 'bd900039-2236-8c2c-8702-d31855c56b0f',
      name: 'Domestic Violence'
    };
    spyOn(component, 'getSubtopicDetail');
    component.getDataOnReload();
    expect(component.activeSubtopicParam).toEqual(
      'bd900039-2236-8c2c-8702-d31855c56b0f'
    );
    // expect(component.subtopics).toEqual(mockGetDocumentData);
    // expect(component.topIntent).toEqual(mockGetDocumentData.name);
    // expect(component.guidedSutopicDetailsInput).toEqual(
    //   mockGuidedSutopicDetailsInput
    // );
    expect(component.getSubtopicDetail).toHaveBeenCalled();
  });
});
