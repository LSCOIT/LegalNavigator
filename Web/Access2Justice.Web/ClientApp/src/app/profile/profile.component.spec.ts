import { HttpClientModule } from "@angular/common/http";
import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { ActivatedRoute, Router } from "@angular/router";
import { MsalService } from "@azure/msal-angular";
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrModule } from "ngx-toastr";
import { Observable, of, empty } from "rxjs";

import { Global } from "../global";
import { PersonalizedPlanService } from "../guided-assistant/personalized-plan/personalized-plan.service";
import { IResourceFilter } from "../common/search/search-results/search-results.model";
import { ArrayUtilityService } from "../common/services/array-utility.service";
import { EventUtilityService } from "../common/services/event-utility.service";
import { ProfileComponent } from "./profile.component";

describe("ProfileComponent", () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let mockRouter;
  let mockGlobal;
  let mockPersonalizedPlanService;
  let msalService;
  let mockIsShared = false;
  let mockIsLoggedIn = true;
  let mockUserData = {
    idToken: {
      aio:
        "DZDuEYiMtCxKoHcGm3TJ07gibmKOCd6rr1l7or7fHQNC9wnEM9Uv2SIH8gvR1m!7Stj16*869uxH4lBU4tEGGcEZGZ8KXLr9w9gqxwbr2uwn",
      aud: "15c4cbff-86ef-4c76-9b08-874ce0b55c8a",
      iss:
        "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0",
      name: "Rakesh Reddy",
      oid: "00000000-0000-0000-f1c2-518ad4e9deaa",
      preferred_username: "turpu.rakesh@outlook.com",
      sub: "AAAAAAAAAAAAAAAAAAAAAIuYJFn2-HRwaHcsLPrzhYE",
      tid: "9188040d-6c67-4c5b-b112-36a304b66dad",
      ver: "2.0"
    }
  };
  let mockUserId = "User Id";
  let mockUserName = "User Name";
  let mockSharedUserId = "Shared User Id";
  let mockSharedUserName = "Shared User Name";
  let mocktopicids: string[] = ["d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef"];
  let mockresourcetype = "ALL";
  let mockcontinuationtoken = null;
  let mockresourceids = ["1d7fc811-dabe-4468-a179-c55075bd22b6"];
  let mocklocation = null;
  let mockresourceinput: IResourceFilter = {
    ResourceType: mockresourcetype,
    ContinuationToken: mockcontinuationtoken,
    TopicIds: mocktopicids,
    PageNumber: 0,
    Location: mocklocation,
    IsResourceCountRequired: false,
    ResourceIds: mockresourceids,
    IsOrder: false,
    OrderByField: "",
    OrderBy: ""
  };
  let mockwebresources = ["test1", "test2"];
  let mockresources = "testResources";
  let mocktopics = "testTopics";
  let mockresponse = {
    resources: mockresources,
    topics: mocktopics
  };
  let mockPersonalizedPlanResources = {
    resources: mockresources,
    topics: mocktopics,
    webResources: mockwebresources
  };
  let mockUserSavedResources = [
    {
      id: "1fb1b006-a8bc-487d-98a0-2457d9d9f78d",
      topics: [
        {
          topicId: "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
          name: "Divorce",
          quickLinks: [
            {
              text: "Filing for Dissolution or Divorce - Ending Your Marriage",
              url: "http://courts.alaska.gov/shc/family/shcstart.htm#issues"
            }
          ],
          icon:
            "https://cs4892808efec24x447cx944.blob.core.windows…/static-resource/assets/images/topics/housing.svg",
          steps: [
            {
              stepId: "f05ace00-c1cc-4618-a224-56aa4677d2aa",
              title:
                "File a motion to modify if there has been a change of circumstances.",
              description:
                "The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.",
              order: 1,
              isComplete: false,
              resources: [
                {
                  id: "19a02209-ca38-4b74-bd67-6ea941d41518",
                  resourceType: "Organizations"
                },
                {
                  id: "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5",
                  resourceType: "Articles"
                }
              ]
            },
            {
              description:
                "Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:",
              isComplete: true,
              order: 2,
              resources: [
                {
                  id: "19a02209-ca38-4b74-bd67-6ea941d41518",
                  resourceType: "Organizations"
                },
                {
                  id: "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5",
                  resourceType: "Articles"
                }
              ],
              stepId: "f05ace00-c1cc-4618-a224-56aa4677d2aa",
              title: "Jurisdiction"
            }
          ]
        },
        {
          topicId: "e1fdbbc6-d66a-4275-9cd2-2be84d303e12",
          name: "Eviction",
          quickLinks: [
            {
              text: "Filing for Dissolution or Divorce - Ending Your Marriage",
              url: "http://courts.alaska.gov/shc/family/shcstart.htm#issues"
            }
          ],
          icon:
            "https://cs4892808efec24x447cx944.blob.core.windows…/static-resource/assets/images/topics/housing.svg",
          steps: [
            {
              stepId: "f79305c1-8767-4485-9e9b-0b5a573ea7b3",
              title:
                "File a motion to modify if there has been a change of circumstances.",
              description:
                "The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.",
              order: 1,
              isComplete: false,
              resources: [
                {
                  id: "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5",
                  resourceType: "Forms"
                },
                {
                  id: "49779468-1fe0-4183-850b-ff365e05893e",
                  resourceType: "Organizations"
                }
              ]
            },
            {
              stepId: "e9337765-81fc-4d10-8850-8e872cde4ee8",
              title: "Jurisdiction",
              description:
                "Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:",
              order: 2,
              isComplete: false,
              resources: [
                {
                  id: "be0cb3e1-7054-403a-baac-d119ea5be007",
                  resourceType: "Articles"
                },
                {
                  id: "2fe9f117-bfb5-469f-b80c-877640a29f75",
                  resourceType: "Forms"
                }
              ]
            }
          ]
        }
      ],
      resources: [
        {
          itemId: "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
          resourceType: "Topics",
          resourceDetails: {}
        },
        {
          itemId: "1d7fc811-dabe-4468-a179-c55075bd22b6",
          resourceType: "Organizations",
          resourceDetails: {}
        },
        {
          itemId: "Pro Bono Innovation Fund Grants 2016 | LSC - Legal ...",
          resourceType: "WebResources",
          resourceDetails: {
            id: "https://api.cognitive.microsoft.com/api/v7/#WebPages.9",
            name: "Pro Bono Innovation Fund Grants 2016 | LSC - Legal ...",
            url: "https://www.lsc.gov/pro-bono-innovation-fund-grants-2016"
          }
        }
      ]
    }
  ];
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
  let mockFilterTopicName = "Divorce";

  class ActivateRouteStub {
    params: Observable<any> = empty();
  }
  beforeEach(async(() => {
    mockGlobal = jasmine.createSpyObj(["externalLogin"]);
    mockPersonalizedPlanService = jasmine.createSpyObj([
      "getPersonalizedResources",
      "getUserSavedResources",
      "getActionPlanConditions",
      "createTopicsList",
      "getPlanDetails"
    ]);
    msalService = jasmine.createSpyObj(["getUser", "loginRedirect"]);
    TestBed.configureTestingModule({
      imports: [HttpClientModule, ToastrModule.forRoot()],
      declarations: [ProfileComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        EventUtilityService,
        ArrayUtilityService,
        NgxSpinnerService,
        {
          provide: PersonalizedPlanService,
          useValue: mockPersonalizedPlanService
        },
        {
          provide: Global,
          useValue: {
            mockGlobal,
            userId: mockUserId,
            userName: mockUserName,
            isShared: mockIsShared,
            isLoggedIn: mockIsLoggedIn,
            sharedUserId: mockSharedUserId,
            sharedUserName: mockSharedUserName
          }
        },
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: MsalService,
          useValue: msalService
        },
        {
          provide: ActivatedRoute,
          useValue: ActivateRouteStub
        }
      ]
    }).compileComponents();
    msalService = TestBed.get(MsalService);
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it("should create profile component", () => {
    expect(component).toBeTruthy();
  });

  it("should define profile component", () => {
    expect(component).toBeDefined();
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

  it("should not call loginRedirect msal service method and assign values of logged in user details in ngOninit", () => {
    msalService.getUser.and.returnValue(of(null));
    component.ngOnInit();
    expect(component.userId).toEqual(mockUserId);
    expect(component.userName).toEqual(mockUserName);
  });

  it("should call loginRedirect msal service method and assign values of logged in user details in ngOninit", () => {
    msalService.getUser.and.returnValue(of(mockUserData));
    component.ngOnInit();
    mockIsShared = true;
    expect(msalService.loginRedirect).toHaveBeenCalled();
    expect(component.userId).toEqual(mockUserId);
    expect(component.userName).toEqual(mockUserName);
  });
});
