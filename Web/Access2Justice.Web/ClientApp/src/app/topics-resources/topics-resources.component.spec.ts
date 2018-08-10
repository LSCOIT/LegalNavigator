import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { TopicsResourcesComponent } from './topics-resources.component';
import { TopicsComponent } from './topic/topics.component';
import { GuidedAssistantSidebarComponent } from '../shared/sidebars/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../shared/sidebars/service-org-sidebar.component';
import { TopicService } from './shared/topic.service';
import { ServiceOrgService } from '../shared/sidebars/service-org.service';
import { MapService } from '../shared/map/map.service';

describe('TopicsResourcesComponent', () => {
  let component: TopicsResourcesComponent;
  let fixture: ComponentFixture<TopicsResourcesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        TopicsResourcesComponent,
        TopicsComponent,
        GuidedAssistantSidebarComponent,
        ServiceOrgSidebarComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'topics/:topic', component: TopicsComponent }
        ]),
        HttpClientModule
      ],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        TopicService,
        ServiceOrgService,
        MapService
      ]
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
});
