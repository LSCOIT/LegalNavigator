import { HttpClientModule } from "@angular/common/http";
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { MsalService } from "@azure/msal-angular/dist/msal.service";
import { BsModalService } from "ngx-bootstrap";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from "ngx-toastr";
import { of } from "rxjs";

import { Global } from "../../../../global";
import { PersonalizedPlanComponent } from "../../../../guided-assistant/personalized-plan/personalized-plan.component";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { ProfileComponent } from "../../../../profile/profile.component";
import { ArrayUtilityService } from "../../../services/array-utility.service";
import { EventUtilityService } from "../../../services/event-utility.service";
import { NavigateDataService } from "../../../services/navigate-data.service";
import { UnshareButtonComponent } from "./unshare-button.component";

describe("UnshareButtonComponent", () => {
  let component: UnshareButtonComponent;
  let fixture: ComponentFixture<UnshareButtonComponent>;
  let mockToastr, mockRouter, msalService;
  let mockGlobal: Global;
  let mockNavigateDataService;
  let mockPersonalizedPlanComponent;
  let mockPersonalizedPlanService;
  let mockProfileComponent;
  let mockEventUtilityService;
  let mockPlanDetails = {
    id: "29250697-8d22-4f9d-bbf8-96c1b5b72e54",
    isShared: false,
    topics: [
      {
        topicId: "69be8e7c-975b-43c8-9af3-f61887a33ad3",
        name: "Protective Order",
        icon: null,
        additionalReadings: [
          {
            text: "Domestic Violence - What it is",
            url: "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          },
          {
            text: "Safety Planning Tips",
            url: "https://www.thehotline.org/help/path-to-safety/"
          }
        ],
        steps: [
          {
            description: "Short Term",
            isComplete: false,
            order: 1,
            resources: [],
            stepId: "ec78414b-2616-4d65-ae07-e08f3e2f697a",
            title: "Short Term Protective Order"
          },
          {
            description: "Long Term",
            isComplete: false,
            order: 2,
            resources: [],
            stepId: "ed56894b-2616-4d65-ae07-e08f3e2f697a",
            title: "Long Term Protective Order"
          }
        ]
      },
      {
        topicId: "ba74f857-eb7b-4dd6-a021-5b3e4525e3e4",
        name: "Divorce",
        icon: null,
        additionalReadings: [
          {
            text: "Domestic Violence - What it is",
            url: "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          }
        ],
        steps: [
          {
            description: "Step Description",
            isComplete: false,
            order: 1,
            resources: [],
            stepId: "df822558-73c2-4ac8-8259-fabe2334eb71",
            title: "Step Title"
          }
        ]
      }
    ]
  };
  let mockSelectedPlanDetails = {
    planDetails: mockPlanDetails,
    topic: "Protective Order"
  };

  beforeEach(async(() => {
    mockToastr = jasmine.createSpyObj(["success"]);
    msalService = jasmine.createSpyObj(["getUser"]);
    mockPersonalizedPlanService = jasmine.createSpyObj([
      "showSuccess",
      "userPlan",
      "saveResources"
    ]);
    mockPersonalizedPlanService.userPlan.and.returnValue(of(mockPlanDetails));
    mockPersonalizedPlanService.saveResources.and.returnValue(
      of(mockPlanDetails)
    );
    mockPersonalizedPlanComponent = jasmine.createSpyObj(["getTopics"]);
    mockProfileComponent = jasmine.createSpyObj(["getPersonalizedPlan"]);
    mockNavigateDataService = jasmine.createSpyObj(["setData"]);
    mockEventUtilityService = jasmine.createSpyObj(["updateSavedResource"]);

    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [UnshareButtonComponent],
      providers: [
        BsModalService,
        NgxSpinnerService,
        ArrayUtilityService,
        {
          provide: PersonalizedPlanService,
          useValue: mockPersonalizedPlanService
        },
        {
          provide: ProfileComponent,
          useValue: mockProfileComponent
        },
        {
          provide: EventUtilityService,
          useValue: mockEventUtilityService
        },
        {
          provide: PersonalizedPlanComponent,
          useValue: mockPersonalizedPlanComponent
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        },
        {
          provide: Global,
          useValue: {
            mockGlobal,
            userId: "UserId"
          }
        },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                id: "123"
              }
            }
          }
        },
        {
          provide: Router,
          useValue: { url: "/plan/id" }
        },
        {
          provide: MsalService,
          useValue: msalService
        },
        {
          provide: NavigateDataService,
          useValue: mockNavigateDataService
        }
      ],
      schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
    mockGlobal = TestBed.get(Global);
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnshareButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create component", () => {
    expect(component).toBeTruthy();
  });

  it("should call removeSavedPlan if selected plandetails exists", () => {
    component.selectedPlanDetails = mockSelectedPlanDetails;
    spyOn(component, "unshareSharedResources");
    component.unshareSharedResources();
    expect(component.unshareSharedResources).toHaveBeenCalled();
  });

  it("should call removedSavedResource if selected plandetails exists", () => {
    component.resourceId = "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef";
    spyOn(component, "unshareSharedToResources");
    component.unshareSharedToResources();
    expect(component.unshareSharedToResources).toHaveBeenCalled();
  });


});
