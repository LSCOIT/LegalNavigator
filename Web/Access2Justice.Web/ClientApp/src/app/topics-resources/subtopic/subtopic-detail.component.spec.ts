import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { SubtopicDetailComponent } from './subtopic-detail.component';
import { SaveButtonComponent } from '../../shared/resource/user-action/save-button.component';
import { ShareButtonComponent } from '../../shared/resource/user-action/share-button.component';
import { PrintButtonComponent } from '../../shared/resource/user-action/print-button.component';
import { ResourceCardComponent } from '../../shared/resource/resource-card/resource-card.component';
import { GuidedAssistantSidebarComponent } from '../../shared/sidebars/guided-assistant-sidebar.component';
import { ServiceOrgSidebarComponent } from '../../shared/sidebars/service-org-sidebar.component';
import { TopicService } from '../shared/topic.service';

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
        ServiceOrgSidebarComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'topics/:topic/:subtopic', component: SubtopicDetailComponent }
        ]),
        HttpClientModule
      ],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        TopicService
      ]
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
