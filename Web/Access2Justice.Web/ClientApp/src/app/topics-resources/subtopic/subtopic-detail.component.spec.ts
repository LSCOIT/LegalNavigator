import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { SubtopicDetailComponent } from './subtopic-detail.component';
import { SaveButtonComponent } from '../../shared/resource/user-action/save-button/save-button.component';
import { ShareButtonComponent } from '../../shared/resource/user-action/share-button/share-button.component';
import { PrintButtonComponent } from '../../shared/resource/user-action/print-button.component';
import { ResourceCardComponent } from '../../shared/resource/resource-card/resource-card.component';
import { GuidedAssistantSidebarComponent } from '../../shared/sidebars/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../../shared/sidebars/service-org-sidebar.component';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';
import { TopicService } from '../shared/topic.service';
import { NavigateDataService } from '../../shared/navigate-data.service';
import { ServiceOrgService } from '../../shared/sidebars/service-org.service';
import { LocationService } from '../../shared/location/location.service';
describe('SubtopicDetailComponent', () => {
  let component: SubtopicDetailComponent;
  let fixture: ComponentFixture<SubtopicDetailComponent>;

  beforeEach(async(() => {
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
          { path: 'subtopics/:topic', component: SubtopicDetailComponent }
        ]),
        HttpClientModule
      ],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        TopicService,
        NavigateDataService,
        ServiceOrgService,
        LocationService
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubtopicDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
