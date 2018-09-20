import { AboutComponent } from './about/about.component';
import { AppComponent } from './app.component';
import { async, TestBed, ComponentFixture } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Global } from './global';
import { GuidedAssistantComponent } from './guided-assistant/guided-assistant.component';
import { HelpFaqsComponent } from './help-faqs/help-faqs.component';
import { HomeComponent } from './home/home.component';
import { PrivacyPromiseComponent } from './privacy-promise/privacy-promise.component';
import { QuestionComponent } from './guided-assistant/question/question.component';
import { SearchComponent } from './shared/search/search.component';
import { StaticResourceService } from './shared/static-resource.service';
import { SubtopicDetailComponent } from './topics-resources/subtopic/subtopic-detail.component';
import { SubtopicsComponent } from './topics-resources/subtopic/subtopics.component';
import { TopicsComponent } from './topics-resources/topic/topics.component';
import { TopicsResourcesComponent } from './topics-resources/topics-resources.component';
import { MapService } from './shared/map/map.service';
import { of } from 'rxjs/observable/of';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { MsalService, BroadcastService } from '@azure/msal-angular';
import { MSAL_CONFIG } from '@azure/msal-angular/dist/msal.service';
import { PersonalizedPlanService } from './guided-assistant/personalized-plan/personalized-plan.service';
import { HttpClientModule } from '@angular/common/http';
import { ArrayUtilityService } from './shared/array-utility.service';
import { ToastrService } from 'ngx-toastr';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let mockStaticResourceService;
  let mockGlobal;
  let staticContent;
  let mockMapService;
  let mockRouter;
  let msalService;
  let toastrService: ToastrService;

  beforeEach(async(() => {
    staticContent = [
      {
        "name": "HomePage",
        "location": [
          { "state": "Default" }
        ]
      },
      {
        "name": "PrivacyPromisePage",
        "location": [
          { "state": "Default" }
        ]
      },
      {
        "name": "HelpAndFAQPage",
        "location": [
          { "state": "Default" }
        ]
      },
      {
        "name": "Navigation",
        "location": [
          { "state": "Default" }
        ]
      },
      {
        "name": "AboutPage",
        "location": [
          { "state": "Default" }
        ]
      }
    ]
    mockStaticResourceService = jasmine.createSpyObj(['getStaticContents']);
    mockGlobal = jasmine.createSpyObj(['setData']);
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        AboutComponent,
        GuidedAssistantComponent,
        HelpFaqsComponent,
        HomeComponent,
        PrivacyPromiseComponent,
        QuestionComponent,
        SearchComponent,
        TopicsComponent,
        TopicsResourcesComponent,
        SubtopicDetailComponent,
        SubtopicsComponent
      ],
      imports: [
        FormsModule,
        HttpClientModule

      ],
      providers: [AppComponent, PersonalizedPlanService, ArrayUtilityService, ToastrService,
        { provide: MsalService, useValue: msalService },
        { provide: StaticResourceService, useValue: mockStaticResourceService },
        { provide: Global, useValue: mockGlobal },
        { provide: MapService, useValue: mockMapService },
        { provide: Router, useValue: mockRouter },
        { provide: MSAL_CONFIG, useValue: {} },
        { provide: ToastrService, useValue: toastrService },
        NgxSpinnerService,
        BroadcastService,
      ],
      schemas: [
        NO_ERRORS_SCHEMA,
        CUSTOM_ELEMENTS_SCHEMA
      ]
    }).compileComponents();

    //create component and test fixture
    fixture = TestBed.createComponent(AppComponent);

    // UserService provided to the TestBed
    msalService = TestBed.get(MsalService);
    component = TestBed.get(AppComponent);
    toastrService = TestBed.get(ToastrService);
    
  }));

   it('should create the app', async(() => {
    expect(component).toBeTruthy();
  }));
  it('should create the app', async(() => {
    expect(component).toBeDefined();
  }));

  it(`should have as title 'app'`, async(() => {
    expect(component.title).toEqual('app');
  }));

  it('should set staticContent Result after calling getStaticContents', () => {
    mockStaticResourceService.getStaticContents.and.returnValue(of(staticContent));
    component.setStaticContentData();
    expect(component.staticContentResults).toEqual(staticContent);
    expect(mockGlobal.setData).toHaveBeenCalledWith(component.staticContentResults);
  });
});

