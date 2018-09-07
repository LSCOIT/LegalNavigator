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

describe('TopicsComponent', () => {
  let component: TopicsComponent;
  let fixture: ComponentFixture<TopicsComponent>;
  let api: TopicService;
  let router: RouterModule;
  let mockTopicService;
  let mockTopics = [
    {
       "id":"e3bdf5d8-8755-46d9-b13b-e28546fcd27e",
       "name":"Abuse & Harassment",
       "parentTopicId":[
 
       ],
       "resourceType":"Topics",
       "keywords":null,
       "location":[
          {
             "state":"Hawaii",
             "city":"Kalawao",
             "zipCode":"96761"
          }
       ],
       "icon":"www.test.com/static-resource/assets/images/categories/abuse.svg",
    }
 ]
  const mockRouter = {
    navigate: () => { }
  };

  beforeEach(async(() => {
    mockTopicService = jasmine.createSpyObj(['getTopics']);
    mockTopicService.getTopics.and.returnValue(of(mockTopics));
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
        MapService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicsComponent);
    component = fixture.componentInstance;
  });

  // it('makes a call to getTopics Oninit', () => {
  //   spyOn(component, 'getTopics');
  //   component.ngOnInit();
  //   expect(component.getTopics).toHaveBeenCalled();
  // });

  // it('should set topic variable to the response from topic service', () => {
  //   component.getTopics();
  //   expect(component.topics).toEqual(mockTopics);
  // })
});
