import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TopicsComponent } from './topics.component';
import { TopicService } from '../shared/topic.service';
import { SubtopicsComponent } from '../subtopic/subtopics.component';
import { GuidedAssistantSidebarComponent } from '../../shared/sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../../shared/sidebars/service-org-sidebar/service-org-sidebar.component';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { BreadcrumbService } from '../shared/breadcrumb.service';
import { MapService } from '../../shared/map/map.service';
import { of } from 'rxjs/observable/of';
import { Global } from '../../global';
import { QueryList } from '@angular/core';

describe('TopicsComponent', () => {
  let component: TopicsComponent;
  let fixture: ComponentFixture<TopicsComponent>;
  let mockTopicService;
  let mockGlobal;
  let mockTopics: any = [
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
  const mockRouter = {
    navigate: () => { }
  };

  beforeEach(async(() => {

    mockTopicService = jasmine.createSpyObj(['getTopics']);
    mockTopicService.getTopics.and.returnValue(of(mockTopics));
    mockGlobal = {};

    TestBed.configureTestingModule({
      declarations: [
        TopicsComponent,
        SubtopicsComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent,
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
        { provide: RouterModule, useValue: mockRouter },
        BreadcrumbService,
        MapService,
        { provide: Global, useValue: mockGlobal }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app', async(() => {
    expect(component).toBeTruthy();
  }));

  it('should define the app', async(() => {
    expect(component).toBeDefined();
  }));

  it('Bind data topics property from cached topics Data Oninit', async () => {
    component.ngOnInit();
    expect(component.topics).toEqual(mockTopics);
  });

  it('makes a call to getTopics Oninit', async () => {
    mockGlobal.topicsData = undefined;
    spyOn(component, 'getTopics');
    component.subscription.next(1);
    component.ngOnInit();
    expect(component.getTopics).toHaveBeenCalled();
  });

  it('should calculate call setBottomBorder with correct parameter when findBottomRowTopics is called', () => {
    spyOn(component, 'setBottomBorder');
    component.windowWidth = 777;
    component.bottomRowTopics = 1;
    component.findBottomRowTopics();
    expect(component.setBottomBorder).toHaveBeenCalledWith(1);
  });
});
