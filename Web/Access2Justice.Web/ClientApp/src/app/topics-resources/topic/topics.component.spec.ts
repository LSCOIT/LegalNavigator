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


describe('TopicsComponent', () => {
  let component: TopicsComponent;
  let fixture: ComponentFixture<TopicsComponent>;
  let api: TopicService;
  let router: RouterModule;

  const mockTopicService = {
    getTopics: () => { }
  };

  const mockRouter = {
    navigate: () => { }
  };

  beforeEach(async(() => {
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
        { provide: BreadcrumbService, useValue: mockTopicService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicsComponent);
    component = fixture.componentInstance;
    api = fixture.debugElement.injector.get(TopicService);
    router = fixture.debugElement.injector.get(RouterModule);
  });

  describe('initial display', () => {
    it('makes a call to api.getTopics', () => {
      spyOn(api, 'getTopics').and.returnValue(Observable.of());
      fixture.detectChanges();
      expect(api.getTopics).toHaveBeenCalled();
    });
  });
});
