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
import { LoginService } from './shared/login/login.service';
import { IUserProfile } from './shared/login/user-profile.model';
import { TopicService } from './topics-resources/shared/topic.service';
import { SaveButtonService } from './shared/resource/user-action/save-button/save-button.service';

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
  let mockLoginService;
  let mockUserData;
  let mockLoginResponse;
  let mockPersonalizedPlanService;
  let mockSaveButtonService;

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
    ];

    mockUserData = {
      displayableId: "mockUser",
      idToken: {
        name: "mockUser",
        preferred_username: "mockUser@microsoft.com"
      },
      identityProvider: "https://login.microsoftonline.com/123test",
      name: "mockUser",
      userIdentifier: "1234567890ABC"
    }

    mockLoginResponse = {
      oId: "1234567890ABC",
      name: "mockUser",
      eMail: "mockUser@microsoft.com",
      roleInformation: [
        { roleName: "Admin"}
      ]
    }

    mockStaticResourceService = jasmine.createSpyObj(['getStaticContents', 'loadStateName']);
    mockGlobal = jasmine.createSpyObj(['setData','setProfileData']);
    msalService = jasmine.createSpyObj(['getUser']);
    mockLoginService = jasmine.createSpyObj(['upsertUserProfile']);
    mockPersonalizedPlanService = jasmine.createSpyObj(['saveResourcesToUserProfile']);
    mockSaveButtonService = jasmine.createSpyObj(['getPlan']);

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
        { provide: LoginService, useValue: mockLoginService },
        { provide: PersonalizedPlanService, useValue: mockPersonalizedPlanService },
        { provide: SaveButtonService, useValue: mockSaveButtonService },
        NgxSpinnerService,
        BroadcastService,
        TopicService
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
  
  it('should call setProfileData from global', () => {
    mockLoginService.getUserProfile.and.returnValue(of(mockLoginResponse));
    component.createOrGetProfile();
    expect(mockGlobal.setProfileData).toHaveBeenCalledWith("1234567890ABC", "mockUser", "mockUser@microsoft.com", [({ roleName: "Admin" })]);
  });
});

