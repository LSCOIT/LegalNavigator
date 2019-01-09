import { APP_BASE_HREF } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { GuidedAssistantSidebarComponent } from '../shared/sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { MapService } from '../shared/map/map.service';
import { NavigateDataService } from '../shared/services/navigate-data.service';
import { RouterModule } from '@angular/router';
import { ServiceOrgSidebarComponent } from '../shared/sidebars/service-org-sidebar/service-org-sidebar.component';
import { ShowMoreService } from '../shared/sidebars/show-more/show-more.service';
import { TopicsComponent } from './topic/topics.component';
import { TopicService } from './shared/topic.service';
import { TopicsResourcesComponent } from './topics-resources.component';
import { PaginationService } from '../shared/pagination/pagination.service';
import { NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { of } from 'rxjs/observable/of';
import { Global } from '../global';
import { StateCodeService } from '../shared/services/state-code.service';

describe('TopicsResourcesComponent', () => {
  let component: TopicsResourcesComponent;
  let fixture: ComponentFixture<TopicsResourcesComponent>;
  let mockTopicService;
  let mockShowMoreService: ShowMoreService;
  let mockTopics = [
    {
      "id": "e3bdf5d8-8755-46d9-b13b-e28546fcd27e",
      "name": "Abuse & Harassment",
      "parentTopicId": [
      ],
      "resourceType": "Topics",
      "keywords": null,
      "location": [
        {
          "state": "Hawaii",
          "city": "Kalawao",
          "zipCode": "96761"
        }
      ],
      "icon": "",
    }
  ];
  beforeEach(async(() => {
    mockTopicService = jasmine.createSpyObj(['getTopics']);
    mockTopicService.getTopics.and.returnValue(of(mockTopics));
    mockShowMoreService = jasmine.createSpyObj(['clickSeeMoreOrganizations']);
    TestBed.configureTestingModule({
      declarations: [
        TopicsResourcesComponent,
        TopicsComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent
      ],
      imports: [
        RouterModule.forRoot([
          {
            path: 'topics/:topic',
            component: TopicsComponent
          }
        ]),
        HttpClientModule
      ],
      providers: [
        Global,
        MapService,
        NavigateDataService,
        PaginationService,
        StateCodeService,
        {
          provide: APP_BASE_HREF,
          useValue: '/'
        },
        {
          provide: TopicService,
          useValue: mockTopicService
        },
        {
          provide: ShowMoreService,
          useValue: mockShowMoreService
        }
      ],
      schemas: [
        NO_ERRORS_SCHEMA,
        CUSTOM_ELEMENTS_SCHEMA]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicsResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call clickSeeMoreOrganizationsFromTopic', () => {    
    let resourceType = 'test';
    spyOn(component, 'clickSeeMoreOrganizationsFromTopic');
    component.clickSeeMoreOrganizationsFromTopic(resourceType);
    expect(component.clickSeeMoreOrganizationsFromTopic).toHaveBeenCalled();
  });
});
