import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { SubtopicsComponent } from './subtopics.component';
import { ServiceOrgSidebarComponent } from '../../shared/sidebars/service-org-sidebar.component';
import { GuidedAssistantSidebarComponent } from '../../shared/sidebars/guided-assistant-sidebar.component';
import { TopicService } from '../shared/topic.service';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { BreadcrumbService } from '../shared/breadcrumb.service';
import { ServiceOrgService } from '../../shared/sidebars/service-org.service';
import { MapService } from '../../shared/map/map.service';
describe('SubtopicsComponent', () => {
  let component: SubtopicsComponent;
  let fixture: ComponentFixture<SubtopicsComponent>;

  beforeEach(async(() => {
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
        TopicService,
        NavigateDataService,
        BreadcrumbService,
        ServiceOrgService,
        MapService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubtopicsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
