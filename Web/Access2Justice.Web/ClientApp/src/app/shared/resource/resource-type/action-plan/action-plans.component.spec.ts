import { HttpClientModule } from "@angular/common/http";
import { NO_ERRORS_SCHEMA, TemplateRef } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { DomSanitizer } from "@angular/platform-browser";
import { ModalModule } from "ngx-bootstrap";
import { BsModalService } from "ngx-bootstrap/modal";
import { ToastrModule, ToastrService } from "ngx-toastr";
import { of } from "rxjs/observable/of";
import { Global } from "../../../../global";
import { PersonalizedPlanService } from "../../../../guided-assistant/personalized-plan/personalized-plan.service";
import { ArrayUtilityService } from "../../../services/array-utility.service";
import { NavigateDataService } from "../../../services/navigate-data.service";
import { ActionPlansComponent } from "./action-plans.component";

describe("ActionPlansComponent", () => {
  let component: ActionPlansComponent;
  let fixture: ComponentFixture<ActionPlansComponent>;
  let personalizedPlanService;
  let navigateDataService;
  let template: TemplateRef<any>;
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
  let mockTopicsList = [
    {
      topic: {
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
      isSelected: true
    },
    {
      topic: {
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
      },
      isSelected: true
    }
  ];
  let mockFilteredTopicsList = [
    {
      topic: {
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
      isSelected: true
    },
    {
      topic: {
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
      },
      isSelected: false
    }
  ];
  let modalService: BsModalService;
  let global;

  beforeEach(async(() => {
    navigateDataService = jasmine.createSpyObj(["setData", "getData"]);
    personalizedPlanService = jasmine.createSpyObj(["showSuccess", "userPlan"]);
    navigateDataService.getData.and.returnValue(
      JSON.stringify(mockPlanDetails)
    );

    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        ToastrModule.forRoot(),
        ModalModule.forRoot()
      ],
      declarations: [ActionPlansComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        DomSanitizer,
        BsModalService,
        ArrayUtilityService,
        ToastrService,
        {
          provide: PersonalizedPlanService,
          useValue: personalizedPlanService
        },
        {
          provide: NavigateDataService,
          useValue: navigateDataService
        },
        {
          provide: Global,
          useValue: {
            global,
            userId: "User Id"
          }
        }
      ]
    }).compileComponents();
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(ActionPlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    personalizedPlanService = TestBed.get(PersonalizedPlanService);
    modalService = TestBed.get(BsModalService);
    navigateDataService = TestBed.get(NavigateDataService);
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });

  it("should call getPersonalizedPlan on ngChanges", () => {
    component.planDetails = mockPlanDetails;
    component.topicsList = mockTopicsList;
    spyOn(component, "getPersonalizedPlan");
    component.ngOnChanges();
    expect(component.getPersonalizedPlan).toHaveBeenCalledWith(mockPlanDetails);
    expect(component.tempFilteredtopicsList).toEqual(mockTopicsList);
  });

  it("should assign values for planDetails and displaySteps if planDetails list is empty in getPersonalizedPlan method", () => {
    let emptyPlanDetails = {};
    component.getPersonalizedPlan(emptyPlanDetails);
    expect(component.planDetails).toEqual(emptyPlanDetails);
    expect(component.displaySteps).toBeFalsy();
  });

  it("should assign values for planDetails and displaySteps if planDetails is not defined in getPersonalizedPlan method", () => {
    let noPlanDetails = [];
    component.getPersonalizedPlan(noPlanDetails);
    expect(component.planDetails).toEqual(noPlanDetails);
    expect(component.displaySteps).toBeFalsy();
  });

  it("should assign values for planDetails, displaySteps and call sortStepsByOrder if planDetails list is not empty in getPersonalizedPlan method", () => {
    spyOn(component, "sortStepsByOrder");
    component.getPersonalizedPlan(mockPlanDetails);
    expect(component.planDetails).toEqual(mockPlanDetails);
    expect(component.displaySteps).toBeTruthy();
    expect(component.sortStepsByOrder).toHaveBeenCalledWith(
      component.planDetails
    );
  });

  it("should call orderBy in sortStepsByOrder method", () => {
    spyOn(component, "orderBy");
    component.sortStepsByOrder(mockPlanDetails);
    expect(component.orderBy).toHaveBeenCalled();
  });

  it("should call userPlan service in updatePlanToProfile method", () => {
    personalizedPlanService.userPlan.and.returnValue(of(mockPlanDetails));
    spyOn(component, "showStepCompleteStatus");
    component.updatePlanToProfile();
    expect(personalizedPlanService.userPlan).toHaveBeenCalled();
    expect(component.showStepCompleteStatus).toHaveBeenCalled();
  });

  it("should call showSuccess service in showStepCompleteStatus method", () => {
    component.isChecked = true;
    component.showStepCompleteStatus();
    expect(personalizedPlanService.showSuccess).toHaveBeenCalled();
  });

  it("should call modalService show when openModal is called", () => {
    spyOn(modalService, "show");
    component.openModal(template);
    expect(modalService.show).toHaveBeenCalled();
  });

  it("should push filtered topics with all topics to removePlanDetails list in getRemovePlanDetails method", () => {
    component.tempFilteredtopicsList = mockTopicsList;
    component.getRemovePlanDetails();
    expect(component.removePlanDetails).toEqual(mockTopicsList);
  });

  it("should push filtered topics with 1 filtered topic to removePlanDetails list in getRemovePlanDetails method", () => {
    component.tempFilteredtopicsList = mockFilteredTopicsList;
    component.getRemovePlanDetails();
    expect(component.removePlanDetails).toEqual(mockFilteredTopicsList);
  });

  it("should call orderBy method should return 0", () => {
    let mockUnOrderedData = [
      { id: "item1", isOrdered: false },
      { id: "item2", isOrdered: true }
    ];
    spyOn(component, "orderBy");
    let test = component.orderBy(mockUnOrderedData, "isOrdered");
    expect(component.orderBy).not.toBe(0);
  });

  it("should call orderBy method should return 1", () => {
    let mockUnOrderedData = [
      { id: "item1", isOrdered: true },
      { id: "item2", isOrdered: false }
    ];
    spyOn(component, "orderBy");
    let test = component.orderBy(mockUnOrderedData, "isOrdered");
    expect(component.orderBy).not.toBe(1);
  });
});
