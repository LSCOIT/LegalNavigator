import { TestBed, async } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AboutComponent } from './about/about.component';
import { CreateAccountComponent } from './account/create-account.component';
import { GuidedAssistantComponent } from './guided-assistant/guided-assistant.component';
import { HelpFaqsComponent } from './help-faqs/help-faqs.component';
import { HomeComponent } from './home/home.component';
import { LogInComponent } from './account/log-in.component';
import { PrivacyPromiseComponent } from './privacy-promise/privacy-promise.component';
import { QuestionComponent } from './guided-assistant/curated-experience/question.component';
import { SearchComponent } from './shared/search/search.component';
import { SubtopicsComponent } from './topics-resources/subtopic/subtopics.component';
import { SubtopicDetailComponent } from './topics-resources/subtopic/subtopic-detail.component';
import { TopicsResourcesComponent } from './topics-resources/topics-resources.component';
import { TopicsComponent } from './topics-resources/topic/topics.component';


describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        AboutComponent,
        CreateAccountComponent,
        GuidedAssistantComponent,
        HelpFaqsComponent,
        HomeComponent,
        LogInComponent,
        PrivacyPromiseComponent,
        QuestionComponent,
        SearchComponent,
        TopicsComponent,
        TopicsResourcesComponent,
        SubtopicDetailComponent,
        SubtopicsComponent
      ],
      imports: [
        FormsModule
      ],
      schemas: [
        NO_ERRORS_SCHEMA,
        CUSTOM_ELEMENTS_SCHEMA
      ]
    }).compileComponents();
  }));
  it('should create the app', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
  it(`should have as title 'app'`, async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('app');
  }));
});
