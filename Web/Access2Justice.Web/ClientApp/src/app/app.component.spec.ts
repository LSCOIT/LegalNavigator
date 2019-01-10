import { HttpClientModule } from "@angular/common/http";
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { FormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { BroadcastService, MsalService } from "@azure/msal-angular";
import { MSAL_CONFIG } from "@azure/msal-angular/dist/msal.service";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { of } from "rxjs/observable/of";
import { AboutComponent } from "./about/about.component";
import { AppComponent } from "./app.component";
import { Global } from "./global";
import { GuidedAssistantComponent } from "./guided-assistant/guided-assistant.component";
import { PersonalizedPlanService } from "./guided-assistant/personalized-plan/personalized-plan.service";
import { QuestionComponent } from "./guided-assistant/question/question.component";
import { HelpFaqsComponent } from "./help-faqs/help-faqs.component";
import { HomeComponent } from "./home/home.component";
import { PrivacyPromiseComponent } from "./privacy-promise/privacy-promise.component";
import { LoginService } from "./shared/login/login.service";
import { MapService } from "./shared/map/map.service";
import { PipeModule } from "./shared/pipe/pipe.module";
import { SaveButtonService } from "./shared/resource/user-action/save-button/save-button.service";
import { SearchComponent } from "./shared/search/search.component";
import { ArrayUtilityService } from "./shared/services/array-utility.service";
import { StaticResourceService } from "./shared/services/static-resource.service";
import { TopicService } from "./topics-resources/shared/topic.service";
import { SubtopicDetailComponent } from "./topics-resources/subtopic/subtopic-detail.component";
import { SubtopicsComponent } from "./topics-resources/subtopic/subtopics.component";
import { TopicsComponent } from "./topics-resources/topic/topics.component";
import { TopicsResourcesComponent } from "./topics-resources/topics-resources.component";

describe("AppComponent", () => {
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
  let mockLoginResponse;
  let mockPersonalizedPlanService;
  let mockSaveButtonService;

  beforeEach(async(() => {
    staticContent = [
      {
        name: "HomePage",
        location: [{ state: "Default" }]
      },
      {
        name: "PrivacyPromisePage",
        location: [{ state: "Default" }]
      },
      {
        name: "HelpAndFAQPage",
        location: [{ state: "Default" }]
      },
      {
        name: "Navigation",
        location: [{ state: "Default" }]
      },
      {
        name: "AboutPage",
        location: [{ state: "Default" }]
      }
    ];
    mockLoginResponse = {
      oId: "1234567890ABC",
      name: "mockUser",
      eMail: "mockUser@microsoft.com",
      roleInformation: [{ roleName: "Admin", organizationalUnit: null }]
    };
    mockStaticResourceService = jasmine.createSpyObj([
      "getStaticContents",
      "loadStateName"
    ]);
    mockGlobal = jasmine.createSpyObj(["setData", "setProfileData"]);
    msalService = jasmine.createSpyObj(["getUser"]);
    mockLoginService = jasmine.createSpyObj([
      "upsertUserProfile",
      "getUserProfile"
    ]);
    mockPersonalizedPlanService = jasmine.createSpyObj([
      "saveResourcesToUserProfile"
    ]);
    mockSaveButtonService = jasmine.createSpyObj(["getPlan"]);

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
        HttpClientModule, 
        PipeModule.forRoot()
      ],
      providers: [
        AppComponent,
        PersonalizedPlanService,
        ArrayUtilityService,
        ToastrService,
        NgxSpinnerService,
        BroadcastService,
        TopicService,
        {
          provide: MsalService,
          useValue: msalService
        },
        {
          provide: StaticResourceService,
          useValue: mockStaticResourceService
        },
        {
          provide: Global,
          useValue: mockGlobal
        },
        {
          provide: MapService,
          useValue: mockMapService
        },
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: MSAL_CONFIG,
          useValue: {}
        },
        {
          provide: ToastrService,
          useValue: toastrService
        },
        {
          provide: LoginService,
          useValue: mockLoginService
        },
        {
          provide: PersonalizedPlanService,
          useValue: mockPersonalizedPlanService
        },
        {
          provide: SaveButtonService,
          useValue: mockSaveButtonService
        }
      ],
      schemas: [
        NO_ERRORS_SCHEMA, 
        CUSTOM_ELEMENTS_SCHEMA
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    msalService = TestBed.get(MsalService);
    component = TestBed.get(AppComponent);
    toastrService = TestBed.get(ToastrService);
  }));

  it("should create the app", async(() => {
    expect(component).toBeTruthy();
  }));
  it("should create the app", async(() => {
    expect(component).toBeDefined();
  }));

  it(`should have as title 'app'`, async(() => {
    expect(component.title).toEqual("app");
  }));

  it("should set staticContent Result after calling getStaticContents", () => {
    mockStaticResourceService.getStaticContents.and.returnValue(
      of(staticContent)
    );
    component.setStaticContentData();
    expect(component.staticContentResults).toEqual(staticContent);
    expect(mockGlobal.setData).toHaveBeenCalledWith(
      component.staticContentResults
    );
  });

  it("should call setProfileData from global", () => {
    mockLoginService.getUserProfile.and.returnValue(of(mockLoginResponse));
    component.createOrGetProfile();
    expect(mockGlobal.setProfileData).toHaveBeenCalledWith(
      "1234567890ABC",
      "mockUser",
      "mockUser@microsoft.com",
      [{ roleName: "Admin", organizationalUnit: null }]
    );
  });
});
