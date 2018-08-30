import { APP_BASE_HREF } from '@angular/common';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { GuidedAssistantSidebarComponent } from '../shared/sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { MapService } from '../shared/map/map.service';
import { NavigateDataService } from '../shared/navigate-data.service';
import { RouterModule } from '@angular/router';
import { ServiceOrgSidebarComponent } from '../shared/sidebars/service-org-sidebar/service-org-sidebar.component';
import { ShowMoreService } from '../shared/sidebars/show-more/show-more.service';
import { TopicsComponent } from './topic/topics.component';
import { TopicService } from './shared/topic.service';
import { TopicsResourcesComponent } from './topics-resources.component';
import { PaginationService } from '../shared/pagination/pagination.service';

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
        ShowMoreService,
        MapService,
        NavigateDataService,
        PaginationService
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
