import { APP_BASE_HREF } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { RouterModule } from "@angular/router";
import { ToastrModule, ToastrService } from "ngx-toastr";
import { of } from "rxjs";

import { Global } from "../../global";
import { ArrayUtilityService } from "../../shared/services/array-utility.service";
import { NavigateDataService } from "../../shared/services/navigate-data.service";
import { StaticResourceService } from "../../shared/services/static-resource.service";
import { PersonalizedPlanComponent } from "./personalized-plan.component";
import { PersonalizedPlanService } from "./personalized-plan.service";

describe("Component:PersonalizedPlan", () => {
  let component: PersonalizedPlanComponent;
  let fixture: ComponentFixture<PersonalizedPlanComponent>;
  let toastrService: ToastrService;
  let mockPersonalizedPlanService;
  let mockNavigateDataService;
  let mockStaticResourceService;
  let mockGlobalService;
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
  let mockFilteredPlanDetails = {
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
      }
    ]
  };
  let mockFilterTopicName = "Divorce";
  let mockPersonalizedDescription;
  let mockGlobalData;
  let mockDescription = "test";

  beforeEach(async(() => {
    mockPersonalizedDescription = {
      name: "PersonalizedPlanDescription",
      description: "test",
      location: [{ state: "Alaska" }]
    };
    mockGlobalData = [
      {
        name: "PersonalizedPlanDescription",
        location: [{ state: "Alaska" }]
      }
    ];
    mockPersonalizedPlanService = jasmine.createSpyObj([
      "getActionPlanConditions",
      "createTopicsList",
      "getPlanDetails",
      "displayPlanDetails"
    ]);
    mockPersonalizedPlanService.getActionPlanConditions.and.returnValue(
      of(mockPlanDetails)
    );
    mockNavigateDataService = jasmine.createSpyObj(["getData"]);
    mockNavigateDataService.getData.and.returnValue(of(mockPlanDetails));
    mockStaticResourceService = jasmine.createSpyObj([
      "getLocation",
      "getStaticContents"
    ]);
    mockGlobalService = jasmine.createSpyObj(["getData"]);
    mockGlobalService.getData.and.returnValue([mockPersonalizedDescription]);
    mockStaticResourceService.getStaticContents.and.returnValue(
      mockPersonalizedDescription
    );
    mockStaticResourceService.getLocation.and.returnValue("Alaska");

    TestBed.configureTestingModule({
      imports: [
        ToastrModule.forRoot(),
        RouterModule.forRoot([
          {
            path: "plan /: id",
            component: PersonalizedPlanComponent
          }
        ]),
        HttpClientModule
      ],
      declarations: [PersonalizedPlanComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        ArrayUtilityService,
        ToastrService,
        {
          provide: APP_BASE_HREF,
          useValue: "/"
        },
        {
          provide: PersonalizedPlanService,
          useValue: mockPersonalizedPlanService
        },
        {
          provide: NavigateDataService,
          useValue: mockNavigateDataService
        },
        {
          provide: StaticResourceService,
          useValue: mockStaticResourceService
        },
        {
          provide: Global,
          useValue: mockGlobalService
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PersonalizedPlanComponent);
    component = fixture.componentInstance;
    toastrService = TestBed.get(ToastrService);
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  }));

  it("should create personalized plan component", () => {
    expect(component).toBeTruthy();
  });

  it("should define personalized plan component", () => {
    expect(component).toBeDefined();
  });

  it("should call service methods in setPlan", () => {
    mockPersonalizedPlanService.createTopicsList.and.returnValue(
      of(mockTopicsList)
    );
    mockPersonalizedPlanService.getPlanDetails.and.returnValue(
      of(mockPlanDetails)
    );
    component.setPlan();
  });

  it("should call filterTopicsList method and  personalizedPlanService displayPlanDetails method when filterPlan is called", () => {
    component.topicsList = mockFilteredTopicsList;
    component.planDetailTags = mockPlanDetails;
    spyOn(component, "filterTopicsList");
    mockPersonalizedPlanService.displayPlanDetails.and.returnValue(
      mockFilteredPlanDetails
    );
    component.filterPlan(mockFilterTopicName);
    expect(component.filterTopicsList).toHaveBeenCalledWith(
      mockFilterTopicName
    );
    expect(mockPersonalizedPlanService.displayPlanDetails).toHaveBeenCalledWith(
      mockPlanDetails,
      mockFilteredTopicsList
    );
    expect(component.planDetails).toEqual(mockFilteredPlanDetails);
  });

  it("should return filtered topics list based on the input topic and tempTopicsList in filterTopicsList method", () => {
    component.tempTopicsList = mockTopicsList;
    component.filterTopicsList(mockFilterTopicName);
    expect(component.topicsList).toEqual(mockFilteredTopicsList);
  });

  it("should return filtered topics list infilterTopicsList  method when input is blank in topic", () => {
    let mockTopicListBlank = [
      {
        topic: "",
        isSelected: false
      }
    ];
    component.tempTopicsList = mockTopicsList;
    component.filterTopicsList(mockTopicListBlank);
    expect(component.topicsList).toEqual(mockTopicsList);
  });

  it("should return filtered topics list infilterTopicsList  method when input topic is hidden", () => {
    component.tempTopicsList = mockFilteredTopicsList;
    component.filterTopicsList(mockFilterTopicName);
    expect(component.topicsList).toEqual(mockTopicsList);
  });

  it("should assign component values in filterTopics method", () => {
    let mockEvent = { plan: mockPlanDetails, topicsList: mockTopicsList };
    spyOn(component, "filterPlan");
    component.filterTopics(mockEvent);
    expect(component.topics).toEqual(mockEvent.plan.topics);
    expect(component.planDetailTags).toEqual(mockEvent.plan);
    expect(component.topicsList).toEqual(mockEvent.topicsList);
    expect(component.filterPlan).toHaveBeenCalledWith("");
  });

  it("should set personalized plan description to static resource personalized plan description content if it exists", () => {
    mockStaticResourceService.getLocation.and.returnValue("Alaska");
    mockStaticResourceService.getStaticContents.and.returnValue(
      mockPersonalizedDescription
    );
    component.name = mockPersonalizedDescription.name;
    component.getPersonalizedPlanHeading();
    expect(component.description).toEqual(mockDescription);
  });
});
