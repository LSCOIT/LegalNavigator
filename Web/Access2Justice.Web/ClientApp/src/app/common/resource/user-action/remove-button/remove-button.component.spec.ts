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
import { RemoveButtonComponent } from "./remove-button.component";

describe("UnshareButtonComponent", () => {
  let component: RemoveButtonComponent;
  let fixture: ComponentFixture<RemoveButtonComponent>;
  let mockToastr, mockRouter, msalService;
  let mockGlobal: Global;
  let mockNavigateDataService;
  let mockPersonalizedPlanComponent;
  let mockPersonalizedPlanService;
  let mockProfileComponent;
  let mockEventUtilityService;
  const mockPlanDetails = {
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
  const  mockSelectedPlanDetails = {
    planDetails: mockPlanDetails,
    topic: "69be8e7c-975b-43c8-9af3-f61887a33ad3"
  };
  const mockPlanDetailsAfterRemove = {
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
  };
  const mockSavedResources = {
    itemId: "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
    resourceType: "Topics",
    resourceDetails: {},
    url: undefined
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
      declarations: [RemoveButtonComponent],
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
    fixture = TestBed.createComponent(RemoveButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create component", () => {
    expect(component).toBeTruthy();
  });

  it("should call removeSavedPlan if selected plandetails exists", () => {
    component.selectedPlanDetails = mockSelectedPlanDetails;
    spyOn(component, "removeSavedPlan");
    component.removeSavedResources();
    expect(component.removeSavedPlan).toHaveBeenCalled();
  });

  it("should call removedSavedResource if selected plandetails exists", () => {
    component.resourceId = "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef";
    spyOn(component, "removeSavedResource");
    component.removeSavedResources();
    expect(component.removeSavedResource).toHaveBeenCalled();
  });

  it("should call createRemovePlanTag, if selected plandetails exists", () => {
    component.selectedPlanDetails = mockSelectedPlanDetails;
    spyOn(component, "createRemovePlanTag");
    spyOn(component, "removePersonalizedPlan");
    component.removeSavedPlan();
    expect(component.createRemovePlanTag).toHaveBeenCalled();
    expect(component.removePersonalizedPlan).toHaveBeenCalled();
  });

  it("should splice topics of plan details in createRemovePlanTag", () => {
    component.selectedPlanDetails = mockSelectedPlanDetails;
    component.createRemovePlanTag();
    expect(component.selectedPlanDetails.planDetails.topics).toEqual([
      mockPlanDetailsAfterRemove
    ]);
    expect(component.topicIndex).toEqual(
      mockSelectedPlanDetails.planDetails.topics.length
    );
  });

  it("should remove from plan before saving it to profile", () => {
    component.selectedPlanDetails = mockSelectedPlanDetails;
    const router = TestBed.get(Router);
    router.url = "/plan/id";
    component.removePersonalizedPlan();
    expect(mockPersonalizedPlanComponent.getTopics).toHaveBeenCalled();
    expect(mockPersonalizedPlanService.showSuccess).toHaveBeenCalled();
  });

  it("should remove from plan after saving it to profile", () => {
    component.selectedPlanDetails = mockSelectedPlanDetails;
    const router = TestBed.get(Router);
    router.url = "/profile";
    mockGlobal.userId = "UserId";
    component.removePersonalizedPlan();
    expect(mockProfileComponent.getPersonalizedPlan).toHaveBeenCalled();
    expect(mockPersonalizedPlanService.showSuccess).toHaveBeenCalled();
  });

  it("should call removeUserSavedResourceFromProfile with resources in removedSavedResource method", () => {
    component.personalizedResources = { resources: mockSavedResources };
    spyOn(component, "removeUserSavedResourceFromProfile");
    spyOn(component, "saveResourceToProfile");
    component.removeSavedResource();
    expect(component.removeUserSavedResourceFromProfile).toHaveBeenCalledWith(
      mockSavedResources,
      true
    );
    expect(component.saveResourceToProfile).toHaveBeenCalled();
  });

  it("should call removeUserSavedResourceFromProfile with webresources in removedSavedResource method", () => {
    component.personalizedResources = { webResources: mockSavedResources };
    spyOn(component, "removeUserSavedResourceFromProfile");
    spyOn(component, "saveResourceToProfile");
    component.removeSavedResource();
    expect(component.removeUserSavedResourceFromProfile).toHaveBeenCalledWith(
      mockSavedResources,
      false
    );
    expect(component.saveResourceToProfile).toHaveBeenCalled();
  });

  it("should call removeUserSavedResource if type is resource removeUserSavedResourceFromProfile method", () => {
    component.resourceId = "123";
    mockSavedResources.resourceType = "resources";
    spyOn(component, "removeUserSavedResource");
    component.removeUserSavedResourceFromProfile([mockSavedResources], true);
    expect(component.removeUserSavedResource).toHaveBeenCalled();
  });

  it("should call removeUserSavedResourceFromProfile with topics in removedSavedResource method", () => {
    component.personalizedResources = { topics: mockSavedResources };
    spyOn(component, "removeUserSavedResourceFromProfile");
    spyOn(component, "saveResourceToProfile");
    component.removeSavedResource();
    expect(component.removeUserSavedResourceFromProfile).toHaveBeenCalledWith(
      mockSavedResources,
      false
    );
    expect(component.saveResourceToProfile).toHaveBeenCalled();
  });

  it("should call removeUserSavedResource if type is not resource removeUserSavedResourceFromProfile method", () => {
    component.resourceId = "123";
    spyOn(component, "removeUserSavedResource");
    component.removeUserSavedResourceFromProfile([mockSavedResources], false);
    expect(component.removeUserSavedResource).toHaveBeenCalled();
  });

  it("should form remove resources in removeUserSavedResource method", () => {
    let mockResource = {
      id: "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
      resourceType: "Topics",
      resourceDetails: {}
    };
    let mockRemovedResources = {
      itemId: "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
      resourceType: "Topics",
      resourceDetails: {},
      url: undefined
    };
    component.removeUserSavedResource(mockResource);
    expect(component.removeResource).toEqual(mockRemovedResources);
    expect(component.profileResources.resourceTags).toEqual([
      mockRemovedResources
    ]);
  });

  it("should call service methods in saveResourceToProfile method", () => {
    let mockProfileResource = {
      oId: mockGlobal.userId,
      resourceTags: [mockSavedResources],
      type: "resources"
    };
    component.saveResourceToProfile([mockSavedResources]);
    expect(component.profileResources).toEqual(mockProfileResource);
    expect(mockPersonalizedPlanService.saveResources).toHaveBeenCalled();
    expect(mockPersonalizedPlanService.showSuccess).toHaveBeenCalled();
    expect(mockEventUtilityService.updateSavedResource).toHaveBeenCalled();
  });
});
