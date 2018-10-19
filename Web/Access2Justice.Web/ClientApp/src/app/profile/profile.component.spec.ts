import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProfileComponent } from './profile.component';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { ArrayUtilityService } from '../shared/array-utility.service';
import { EventUtilityService } from '../shared/event-utility.service';
import { Tree } from '@angular/router/src/utils/tree';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';
import { Global } from '../global';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, ActivatedRoute } from '@angular/router';
import { MsalService } from '@azure/msal-angular';
import { of } from 'rxjs/observable/of';
import { environment } from '../../environments/environment.prod';

describe('component:profile', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let arrayutilityservice: ArrayUtilityService;
  let eventutilityservice: EventUtilityService;
  let mockRouter;
  let mockGlobal;
  let mockPersonalizedPlanService;
  let msalService;
  let mockIsShared = false;
  let mockIsLoggedIn = true;
  let mockUserData = {
    idToken: {
      "aio": "DZDuEYiMtCxKoHcGm3TJ07gibmKOCd6rr1l7or7fHQNC9wnEM9Uv2SIH8gvR1m!7Stj16*869uxH4lBU4tEGGcEZGZ8KXLr9w9gqxwbr2uwn",
      "aud": "15c4cbff-86ef-4c76-9b08-874ce0b55c8a",
      "iss": "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0",
      "name": "Rakesh Reddy",
      "oid": "00000000-0000-0000-f1c2-518ad4e9deaa",
      "preferred_username": "turpu.rakesh@outlook.com",
      "sub": "AAAAAAAAAAAAAAAAAAAAAIuYJFn2-HRwaHcsLPrzhYE",
      "tid": "9188040d-6c67-4c5b-b112-36a304b66dad",
      "ver": "2.0"
    }
  };
  let mockUserId = "User Id";
  let mockUserName = "User Name";
  let mockSharedUserId = "Shared User Id";
  let mockSharedUserName = "Shared User Name";
  let mocktopicids: string[] = ["d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef"];
  let mockresourcetype = 'ALL';
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
  };
  let mockwebresources = ["test1", "test2"];
  let mockresources = "testResources";
  let mocktopics = "testTopics";
  let mockresponse = { resources: mockresources, topics: mocktopics };
  let mockPersonalizedPlanResources = { resources: mockresources, topics: mocktopics, webResources: mockwebresources };
  let mockUserSavedResources = [{
    "id": "1fb1b006-a8bc-487d-98a0-2457d9d9f78d",
    "resources": [
      {
        "itemId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
        "resourceType": "Topics",
        "resourceDetails": {}
      },
      {
        "itemId": "1d7fc811-dabe-4468-a179-c55075bd22b6",
        "resourceType": "Organizations",
        "resourceDetails": {}
      },
      {
        "itemId": "Pro Bono Innovation Fund Grants 2016 | LSC - Legal ...",
        "resourceType": "WebResources",
        "resourceDetails": {
          "id": "https://api.cognitive.microsoft.com/api/v7/#WebPages.9",
          "name": "Pro Bono Innovation Fund Grants 2016 | LSC - Legal ...",
          "url": "https://www.lsc.gov/pro-bono-innovation-fund-grants-2016"
        }
      }
    ]
  }];
  let mockPlanId = '1fb1b006-a8bc-487d-98a0-2457d9d9f78d';
  let mockPlanDetails = {
    "id": "1fb1b006-a8bc-487d-98a0-2457d9d9f78d",
    "topics": [
      {
        "topicId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
        "name": "Divorce",
        "quickLinks": [
          {
            "text": "Filing for Dissolution or Divorce - Ending Your Marriage",
            "url": "http://courts.alaska.gov/shc/family/shcstart.htm#issues"
          }
        ],
        "icon": "https://cs4892808efec24x447cx944.blob.core.windows…/static-resource/assets/images/topics/housing.svg",
        "steps": [
          {
            "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
            "title": "File a motion to modify if there has been a change of circumstances.",
            "description": "The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.",
            "order": 1,
            "isComplete": false,
            "resources": [
              {
                "id": "19a02209-ca38-4b74-bd67-6ea941d41518",
                "resourceType": "Organizations"
              },
              {
                "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5",
                "resourceType": "Articles"
              }
            ]
          },
          {
            "description": "Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:",
            "isComplete": true,
            "order": 2,
            "resources": [
              {
                "id": "19a02209-ca38-4b74-bd67-6ea941d41518",
                "resourceType": "Organizations"
              },
              {
                "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5",
                "resourceType": "Articles"
              }
            ],
            "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
            "title": "Jurisdiction"
          }
        ]
      },
      {
        "topicId": "e1fdbbc6-d66a-4275-9cd2-2be84d303e12",
        "name": "Eviction",
        "quickLinks": [
          {
            "text": "Filing for Dissolution or Divorce - Ending Your Marriage",
            "url": "http://courts.alaska.gov/shc/family/shcstart.htm#issues"
          }
        ],
        "icon": "https://cs4892808efec24x447cx944.blob.core.windows…/static-resource/assets/images/topics/housing.svg",
        "steps": [
          {
            "stepId": "f79305c1-8767-4485-9e9b-0b5a573ea7b3",
            "title": "File a motion to modify if there has been a change of circumstances.",
            "description": "The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.",
            "order": 1,
            "isComplete": false,
            "resources": [
              {
                "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5",
                "resourceType": "Forms"
              },
              {
                "id": "49779468-1fe0-4183-850b-ff365e05893e",
                "resourceType": "Organizations"
              }
            ]
          },
          {
            "stepId": "e9337765-81fc-4d10-8850-8e872cde4ee8",
            "title": "Jurisdiction",
            "description": "Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:",
            "order": 2,
            "isComplete": false,
            "resources": [
              {
                "id": "be0cb3e1-7054-403a-baac-d119ea5be007",
                "resourceType": "Articles"
              },
              {
                "id": "2fe9f117-bfb5-469f-b80c-877640a29f75",
                "resourceType": "Forms"
              }
            ]
          }
        ]
      }
    ],
    "isShared": false
  };
  let mockTopicsList = [
    {
      "isSelected": true,
      "topic":
        {
          "topicId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
          "name": "Divorce",
          "quickLinks": [
            {
              "text": "Filing for Dissolution or Divorce - Ending Your Marriage",
              "url": "http://courts.alaska.gov/shc/family/shcstart.htm#issues"
            }
          ],
          "icon": "https://cs4892808efec24x447cx944.blob.core.windows…/static-resource/assets/images/topics/housing.svg",
          "steps": [
            {
              "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
              "title": "Jurisdiction",
              "description": "Jurisdiction is a very complicated subject and you…are some resources that could help you with this:",
              "order": 2,
              "isComplete": false,
              "resources": [
                {
                  "id": "19a02209-ca38-4b74-bd67-6ea941d41518"
                },
                {
                  "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"
                }
              ]
            },
            {
              "description": "Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:",
              "isComplete": true,
              "order": 2,
              "resources": [
                {
                  "id": "19a02209-ca38-4b74-bd67-6ea941d41518"
                },
                {
                  "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"
                }
              ],
              "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
              "title": "Jurisdiction"
            }
          ]
        }
    },
    {
      "isSelected": true,
      "topic": {
        "topicId": "e1fdbbc6-d66a-4275-9cd2-2be84d303e12",
        "name": "Eviction",
        "quickLinks": [
          {
            "text": "Filing for Dissolution or Divorce - Ending Your Marriage",
            "url": "http://courts.alaska.gov/shc/family/shcstart.htm#issues"
          }
        ],
        "icon": "https://cs4892808efec24x447cx944.blob.core.windows…/static-resource/assets/images/topics/housing.svg",
        "steps": [
          {
            "stepId": "f79305c1-8767-4485-9e9b-0b5a573ea7b3",
            "title": "File a motion to modify if there has been a change of circumstances.",
            "description": "The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.",
            "order": 1,
            "isComplete": false,
            "resources": [
              {
                "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5",
                "resourceType": "Forms"
              },
              {
                "id": "49779468-1fe0-4183-850b-ff365e05893e",
                "resourceType": "Organizations"
              }
            ]
          },
          {
            "stepId": "e9337765-81fc-4d10-8850-8e872cde4ee8",
            "title": "Jurisdiction",
            "description": "Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:",
            "order": 2,
            "isComplete": false,
            "resources": [
              {
                "id": "be0cb3e1-7054-403a-baac-d119ea5be007",
                "resourceType": "Articles"
              },
              {
                "id": "2fe9f117-bfb5-469f-b80c-877640a29f75",
                "resourceType": "Forms"
              }
            ]
          }
        ]
      }
    }];
  class ActivateRouteStub {
    params: Observable<any> = Observable.empty();
  }
  let activeRoute: ActivatedRoute;
  beforeEach(async(() => {
    mockGlobal = jasmine.createSpyObj(['externalLogin']);
    mockPersonalizedPlanService = jasmine.createSpyObj(['getPersonalizedResources', 'getUserSavedResources', 'getActionPlanConditions', 'createTopicsList', 'getPlanDetails']);
    msalService = jasmine.createSpyObj(['getUser', 'loginRedirect']);
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        ToastrModule.forRoot()
      ],
      declarations: [
        ProfileComponent
      ],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: PersonalizedPlanService, useValue: mockPersonalizedPlanService },
        EventUtilityService,
        ArrayUtilityService,
        {
          provide: Global, useValue: {
            mockGlobal, userId: mockUserId, userName: mockUserName,
            isShared: mockIsShared, isLoggedIn: mockIsLoggedIn,
            sharedUserId: mockSharedUserId, sharedUserName: mockSharedUserName
          }
        },
        NgxSpinnerService,
        { provide: Router, useValue: mockRouter },
        { provide: MsalService, useValue: msalService },
        { provide: ActivatedRoute, useValue: ActivateRouteStub }
      ]
    }).compileComponents();
    msalService = TestBed.get(MsalService);
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create profile component', () => {
    expect(component).toBeTruthy();
  });

  it('should define profile component', () => {
    expect(component).toBeDefined();
  });

  it('should not call loginRedirect msal service method and assign values of logged in user details in ngOninit', () => {
    msalService.getUser.and.returnValue(of(null));
    component.ngOnInit();
    expect(component.userId).toEqual(mockUserId);
    expect(component.userName).toEqual(mockUserName);
  });

  it('should call loginRedirect msal service method and assign values of logged in user details in ngOninit', () => {
    msalService.getUser.and.returnValue(of(mockUserData));
    component.ngOnInit();
    mockIsShared = true;
    expect(msalService.loginRedirect).toHaveBeenCalled();
    expect(component.userId).toEqual(mockUserId);
    expect(component.userName).toEqual(mockUserName);
  });

  it('should not call loginRedirect msal service method and assign values of logged in user details in ngOninit', () => {
    msalService.getUser.and.returnValue(of(mockUserData));
    component.ngOnInit();
    expect(component.userId).toEqual(mockSharedUserId);
    expect(component.userName).toEqual(mockSharedUserName);
  });

  it('should assign component values in getSavedResource method', () => {
    component.webResources = mockwebresources;
    mockPersonalizedPlanService.getPersonalizedResources.and.returnValue(of(mockresponse));
    component.getSavedResource(mockresourceinput);
    expect(component.personalizedResources).toEqual(mockPersonalizedPlanResources);
    expect(component.isSavedResources).toBeTruthy();
  });

  it('should assign component values in getpersonalizedResources method', () => {    
    spyOn(component, 'getSavedResource');
    mockPersonalizedPlanService.getUserSavedResources.and.returnValue(of(mockUserSavedResources));
    component.getpersonalizedResources();
    expect(component.resourceFilter).toEqual(mockresourceinput);
    expect(component.getSavedResource).toHaveBeenCalledWith(mockresourceinput);
  });

  it('should get topics for personalized plan of a user', () => {
    mockPersonalizedPlanService.getUserSavedResources.and.returnValue(of(mockUserSavedResources));
    mockPersonalizedPlanService.getActionPlanConditions.and.returnValue(of(mockPlanDetails));
    component.getPersonalizedPlan();    
    mockPersonalizedPlanService.createTopicsList.and.returnValue(mockTopicsList);
    mockPersonalizedPlanService.getPlanDetails.and.returnValue(mockPlanDetails);
    component.getTopics();
    expect(component.planId).toEqual(mockPlanId);
    expect(component.topics[0]['topicId']).toEqual(mockPlanDetails.topics[0].topicId);
    expect(component.planDetailTags).toEqual(mockPlanDetails);
  });
});
