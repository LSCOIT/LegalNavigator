import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { TopicComponent } from '../topics-resources/topic.component';
import { GuidedAssistantComponent } from './guided-assistant.component';
import { QuestionComponent } from './question.component';

import { TopicService } from '../topics-resources/topic.service';

describe('GuidedAssistantComponent', () => {
  let component: GuidedAssistantComponent;
  let fixture: ComponentFixture<GuidedAssistantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        TopicComponent,
        GuidedAssistantComponent,
        QuestionComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'guidedassistant/123', component: QuestionComponent }
        ]),
        FormsModule,
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
    fixture = TestBed.createComponent(GuidedAssistantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
