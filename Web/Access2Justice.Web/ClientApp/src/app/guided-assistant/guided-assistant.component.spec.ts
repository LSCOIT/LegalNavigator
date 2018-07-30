import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { GuidedAssistantComponent } from './guided-assistant.component';
import { QuestionComponent } from './question/question.component';
import { TopicService } from '../topics-resources/shared/topic.service';
import { ProgressbarModule } from 'ngx-bootstrap/progressbar';
import { TopicsComponent } from '../topics-resources/topic/topics.component';

describe('GuidedAssistantComponent', () => {
  let component: GuidedAssistantComponent;
  let fixture: ComponentFixture<GuidedAssistantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        TopicsComponent,
        GuidedAssistantComponent,
        QuestionComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'guidedassistant/123', component: QuestionComponent }
        ]),
        FormsModule,
        HttpClientModule,
        ProgressbarModule
      ],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        TopicService
      ],
      schemas: [
        NO_ERRORS_SCHEMA,
        CUSTOM_ELEMENTS_SCHEMA
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuidedAssistantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create guided assisstant component', () => {
    expect(component).toBeTruthy();
  });
});
