import { APP_BASE_HREF } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, RouterModule } from "@angular/router";
import { Subject, of } from "rxjs";

import { Global } from "../../../global";
import { MapLocation } from "../../map/map";
import { MapService } from "../../map/map.service";
import { PaginationService } from "../../pagination/pagination.service";
import { IResourceFilter } from "../../search/search-results/search-results.model";
import { NavigateDataService } from "../../services/navigate-data.service";
import { StateCodeService } from "../../services/state-code.service";
import { ServiceOrgSidebarComponent } from "./service-org-sidebar.component";

describe("Component:ServiceOrgSidebar", () => {
  let component: ServiceOrgSidebarComponent;
  let fixture: ComponentFixture<ServiceOrgSidebarComponent>;
  let mockMapService;
  let mockPaginationService;
  let mockNavigateDataService;
  let mockactiveTopic = "123";
  let mockTopicId = "bd900039-2236-8c2c-8702-d31855c56b0f";
  let mockRouter = { url: "/topics" };
  let mockMapLocation: MapLocation = {
    state: "teststate",
    city: "testcity",
    county: "testcounty",
    zipCode: "000007"
  };
  let mockLocationDetails = { location: mockMapLocation };
  let mockresourcesInput: IResourceFilter = {
    ResourceType: "Organizations",
    ContinuationToken: "",
    TopicIds: [mockactiveTopic],
    ResourceIds: [],
    PageNumber: 0,
    Location: mockMapLocation,
    IsResourceCountRequired: true,
    IsOrder: true,
    OrderByField: "name",
    OrderBy: "ASC",
    url: '',
    sharedTo: []
  };
  let mockOrganizations = {
    resources: [
      {
        id: "afbb8dc1-f721-485f-ab92-1bab60137e24",
        name: "Cook Inlet Tribal Council Inc./Family Services Department",
        type: "Interim Housing Assistance",
        description: "Funding provided by Cook Inlet Housing Authority, Inc.",
        resourceType: "Organizations",
        url: "https://citci.org/child-family/",
        topicTags: [
          {
            id: "62a93f03-8234-46f1-9c35-b3146a96ca8b"
          }
        ],
        location: [
          {
            state: "Hawaii",
            county: "Kalawao County",
            city: "Kalawao",
            zipCode: "96742"
          }
        ],
        icon: "",
        address: "Girdwood, Anchorage, Hawii 99587",
        telephone: "907-793-3600",
        overview:
          "CITC offers a wide variety of programs and services to support individuals in their journey",
        eligibilityInformation:
          "helping Alaska Native and American Indian people residing in the Cook Inlet ",
        reviewedByCommunityMember:
          "CITC collaborates with the eight federally recognized tribes within Cook ",
        reviewerFullName: "John Smith",
        reviewerTitle: "Tribal Leader",
        reviewerImage: ""
      },
      {
        id: "afbb8dc1-f721-485f-ab92-1bab60137e24",
        name: "Cook Inlet Tribal Council Inc./Family Services Department",
        type: "Interim Housing Assistance",
        description: "Funding provided by Cook Inlet Housing Authority, Inc.",
        resourceType: "Organizations",
        url: "https://citci.org/child-family/"
      },
      {
        id: "afbb8dc1-f721-485f-ab92-1bab60137e24",
        name: "Cook Inlet Tribal Council Inc./Family Services Department",
        type: "Interim Housing Assistance",
        description: "Funding provided by Cook Inlet Housing Authority, Inc.",
        resourceType: "Organizations",
        url: "https://citci.org/child-family/"
      },
      {
        id: "afbb8dc1-f721-485f-ab92-1bab60137e24",
        name: "Cook Inlet Tribal Council Inc./Family Services Department",
        type: "Interim Housing Assistance",
        description: "Funding provided by Cook Inlet Housing Authority, Inc.",
        resourceType: "Organizations",
        url: "https://citci.org/child-family/"
      }
    ]
  };
  let mockLessOrganizations = {
    resources: [
      {
        id: "afbb8dc1-f721-485f-ab92-1bab60137e24",
        name: "Cook Inlet Tribal Council Inc./Family Services Department",
        type: "Interim Housing Assistance",
        description: "Funding provided by Cook Inlet Housing Authority, Inc.",
        resourceType: "Organizations",
        url: "https://citci.org/child-family/"
      },
      {
        id: "afbb8dc1-f721-485f-ab92-1bab60137e24",
        name: "Cook Inlet Tribal Council Inc./Family Services Department",
        type: "Interim Housing Assistance",
        description: "Funding provided by Cook Inlet Housing Authority, Inc.",
        resourceType: "Organizations",
        url: "https://citci.org/child-family/"
      }
    ]
  };
  let nofityLocation: Subject<object> = new Subject<object>();
  let mockEvent;

  beforeEach(async(() => {
    mockPaginationService = jasmine.createSpyObj(["getPagedResources"]);
    mockNavigateDataService = jasmine.createSpyObj(["getData", "setData"]);
    mockPaginationService.getPagedResources.and.returnValue(
      of(mockOrganizations)
    );
    mockEvent = {
      preventDefault: () => {}
    };

    TestBed.configureTestingModule({
      declarations: [ServiceOrgSidebarComponent],
      imports: [
        RouterModule.forRoot([
          {
            path: "topics/:topic",
            component: ServiceOrgSidebarComponent
          }
        ]),
        HttpClientModule
      ],
      providers: [
        MapService,
        Global,
        StateCodeService,
        {
          provide: APP_BASE_HREF,
          useValue: "/"
        },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                topic: "bd900039-2236-8c2c-8702-d31855c56b0f"
              }
            },
            url: of([
              {
                path: "subtopics",
                params: {}
              },
              {
                path: "bd900039-2236-8c2c-8702-d31855c56b0f",
                params: {}
              }
            ])
          }
        },
        {
          provide: NavigateDataService,
          useValue: mockNavigateDataService
        },
        {
          provide: PaginationService,
          useValue: mockPaginationService
        }
      ],
      schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    let store = {};
    const mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      setItem: (key: string, value: string) => {
        store[key] = `${value}`;
      },
      removeItem: (key: string) => {
        delete store[key];
      },
      clear: () => {
        store = {};
      }
    };

    spyOn(sessionStorage, "setItem").and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, "removeItem").and.callFake(
      mockSessionStorage.removeItem
    );
    spyOn(sessionStorage, "clear").and.callFake(mockSessionStorage.clear);
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServiceOrgSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create service organization sidebar componet", () => {
    expect(component).toBeTruthy();
  });

  it("should define service organization sidebar componet", () => {
    expect(component).toBeDefined();
  });

  it("should emit on callOrganizations", () => {
    spyOn(component.showMoreOrganizations, "emit");
    component.callOrganizations(mockEvent);
    expect(component.showMoreOrganizations.emit).toHaveBeenCalled();
    expect(component.showMoreOrganizations.emit).toHaveBeenCalledWith(
      "Organizations"
    );
  });

  it("should return organization Details when getPagedResource method of pagination service called", () => {
    component.location = mockMapLocation;
    component.activeTopic = mockactiveTopic;
    component.getOrganizations();
    expect(component.organizations.length).toEqual(3);
  });

  it("should return organization Details when getPagedResource method of pagination service called", () => {
    component.location = mockMapLocation;
    component.activeTopic = mockactiveTopic;
    mockPaginationService.getPagedResources.and.returnValue(
      of(mockLessOrganizations)
    );
    component.getOrganizations();
    expect(component.organizations.length).toEqual(
      mockLessOrganizations.resources.length
    );
  });

  it("should assign session storage details to map location on ngInit", () => {
    spyOn(component, "getOrganizations");
    spyOn(sessionStorage, "getItem").and.returnValue(
      JSON.stringify(mockLocationDetails)
    );
    component.ngOnInit();
    expect(component.location).toEqual(mockMapLocation);
    expect(component.getOrganizations).toHaveBeenCalled();
  });

  it("should not assign session storage details to map location on ngInit there is no key for global map in session storage", () => {
    sessionStorage.removeItem("mockGlobalMapLocation");
    component.ngOnInit();
    expect(component.location).toEqual(undefined);
  });

  it("should navigate to topic and get topicid", () => {
    mockRouter.url = "/topics";
    component.location = mockMapLocation;
    component.activeTopic = mockTopicId;
    let resourceFilter: IResourceFilter = {
      ResourceType: "Organizations",
      ContinuationToken: "",
      TopicIds: [],
      ResourceIds: [],
      PageNumber: 0,
      Location: {},
      IsResourceCountRequired: false,
      IsOrder: true,
      OrderByField: "name",
      OrderBy: "ASC",
      url: '',
      sharedTo: []
    };
    component.resourceFilter = resourceFilter;
    component.getOrganizations();
    expect(component.activeTopic).toContain(mockTopicId);
  });

  it("should navigate to search and get topicid", () => {
    mockRouter.url = "/search";
    component.location = mockMapLocation;
    component.activeTopic = mockTopicId;
    let resourceFilter: IResourceFilter = {
      ResourceType: "Organizations",
      ContinuationToken: "",
      TopicIds: [],
      ResourceIds: [],
      PageNumber: 0,
      Location: {},
      IsResourceCountRequired: false,
      IsOrder: true,
      OrderByField: "name",
      OrderBy: "ASC",
      url: '',
      sharedTo: []
    };
    component.resourceFilter = resourceFilter;
    component.getOrganizations();
    expect(component.activeTopic).toContain(mockTopicId);
  });
});
