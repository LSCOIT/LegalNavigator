import { ActivatedRoute, RouterModule } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { BreadcrumbService } from '../shared/breadcrumb.service';
import { GuidedAssistantSidebarComponent } from '../../shared/sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { MapService } from '../../shared/map/map.service';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { of } from 'rxjs/observable/of';
import { PaginationService } from '../../shared/pagination/pagination.service';
import { ServiceOrgSidebarComponent } from '../../shared/sidebars/service-org-sidebar/service-org-sidebar.component';
import { ShowMoreService } from '../../shared/sidebars/show-more/show-more.service';
import { SubtopicsComponent } from './subtopics.component';
import { TopicService } from '../shared/topic.service';

describe('SubtopicsComponent', () => {
  let component: SubtopicsComponent;
  let fixture: ComponentFixture<SubtopicsComponent>;
  let topicService: TopicService;
  let navigateDataService: NavigateDataService;
  let mockactiveTopic = "123";
  let mockDocumentData = [
    {
      "name": "Family1",
      "parentTopicId": [
        {
          "id": "111"
        },
        {
          "id": "222"
        }
      ],
      "keywords": "HOUSING",
      "location": [
        {
          "state": "Hawaii",
          "county": "Kalawao County",
          "city": "Kalawao",
          "zipCode": "96742"
        }

      ],
      "icon": "",
    }
  ];
  let mockSubTopics = [
    {
      "id": "333",
      "name": "Custody / Visitation",
      "overview": "Overview of the Custody/Visitation topic",
      "quickLinks": [],
      "parentTopicId": [
        {
          "id": "123"
        }
      ],
      "resourceType": "Topics",
      "keywords": "Custody | Child Abuse | Child Custody",
      "location": [
        {
          "state": "Hawaii",
          "city": "Kalawao",
          "zipCode": "96761"
        }
      ],
      "icon": ""
    }
  ];
  let mockTopicService;
  let mockNavigateDataService;
  let mockBreadcrumbService;

  beforeEach(async(() => {
    mockTopicService = jasmine.createSpyObj(['getDocumentData', 'getSubtopics']);
    mockNavigateDataService = jasmine.createSpyObj(['getData', 'setData']);   
    mockTopicService.getDocumentData.and.returnValue(of(mockDocumentData));
    mockTopicService.getSubtopics.and.returnValue(of(mockSubTopics));
    
    TestBed.configureTestingModule({
      declarations: [
        SubtopicsComponent,
        ServiceOrgSidebarComponent,
        GuidedAssistantSidebarComponent,
        BreadcrumbComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'topics/:topic', component: SubtopicsComponent }
        ]),
        HttpClientModule
      ],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        { provide: TopicService, useValue: mockTopicService },
        { provide: NavigateDataService, useValue: mockNavigateDataService },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: { 'topic': 'bd900039-2236-8c2c-8702-d31855c56b0f' }
            },
            url: of([
              { path: 'subtopics', params: {} },
              { path: 'bd900039-2236-8c2c-8702-d31855c56b0f', params: {} }
            ])
          }
        }, 
        { provide: BreadcrumbService, useValue: mockBreadcrumbService },
        ShowMoreService,
        MapService,
        PaginationService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubtopicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create subtopics component', () => {
    expect(component).toBeTruthy();
  });
});
