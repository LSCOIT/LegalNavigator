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

fdescribe('component:profile', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;  
  let arrayutilityservice: ArrayUtilityService;
  let eventutilityservice: EventUtilityService;
  let mockRouter;
  let mockPlanId = 'bf8d7e7e-2574-7b39-efc7-83cb94adae07';
  let mockshowremove = true;
  let mocktopic = "test";
  let mockuserid = "38890303";
  let mocktopicids: string[] = [];
  let mockresourcetype = 'test';
  let mockcontinuationtoken = 'test';
  let mockresourceids = ['test'];
  let mocklocation = 'test';
  let mockUserProfileData = [{
    "id": "ba67afca-4236-4875-9b11-fbb8a113ecb2",
    "oId": "ACB833BB3F817C2FBE5A72CE3",
    "firstName": "test name",
    "lastName": "last name",
    "eMail": "test@email.com",
    "isActive": "Yes",
    "createdBy": "full name",
    "createdTimeStamp": "08/06/2018 11:42:39",
    "modifiedBy": null,
    "modifiedTimeStamp": null,
    "sharedResource": null,
    "personalizedActionPlanId": "a97fc45d-cbbe-4222-9d14-2ec227229b92",
    "curatedExperienceAnswersId": "00000000-0000-0000-0000-000000000000"
  }];
  let mockresourceinput: IResourceFilter = {
    ResourceType: mockresourcetype,
    ContinuationToken: mockcontinuationtoken,
    TopicIds: mocktopicids,
    PageNumber: 1,
    Location: mocklocation,
    IsResourceCountRequired: false,
    ResourceIds: mockresourceids,
  };
  let mockPlanDetailsjson = {
    "id": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
    "oid": "user id",
    "type": "plans",
    "plantags": [{
      "topicid": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
      "steptags": [
        {
          "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
          "order": 1,
          "markcompleted": false
        },
        {
          "id": "d46aecee-8c79-df1b-4081-1ea02b5022df",
          "order": 2,
          "markcompleted": false
        }
      ],
      "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
    }
    ]
  };
  let mocktopics = {
    "isselected": true,
    "topic": {
      "topicid": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
      "name": "divorce",
      "quicklinks": [
        {
          "text": "filing for dissolution or divorce - ending your marriage",
          "url": "http://courts.alaska.gov/shc/family/shcstart.htm#issues"
        },
        {
          "text": "spousal support (alimony)",
          "url": "https://alaskalawhelp.org/resource/spousal-support?ref=oegqx"
        }
      ],
      "icon": "https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/topics/housing.svg",
      "steps": [
        {
          "stepid": "66a1e288-4ab1-480f-9d29-eb16abd3ae69"
        }
      ]
    }
  }
  let mockPlanDetailsWithTopics = {
    "id": "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805",
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
    ],
    "isShared": false
  };
  let mockPlanDetails = {
    "id": "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805",
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
  let mockFilteredTopicsList = [
    {
      "isSelected": false,
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
  let mockTopicListBlank = [{
    topic: '',
    isSelected: false
  }];
  let mockFilterTopicName = "Divorce";
  let mockGlobal;
  let mockPersonalizedPlanService;
  let msalService;
  class ActivateRouteStub {
    params: Observable<any> = Observable.empty();
  }
  let activeRoute: ActivatedRoute;
  beforeEach(async(() => {
    mockGlobal = jasmine.createSpyObj(['externalLogin']);
    mockPersonalizedPlanService = jasmine.createSpyObj(['getActionPlanConditions', 'displayPlanDetails', 'getUserSavedResources', 'createTopicsList', 'getPlanDetails']);
    msalService = jasmine.createSpyObj(['getUser']);
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
        { provide: Global, useValue: mockGlobal },
        NgxSpinnerService,
        { provide: Router, useValue: mockRouter },
        { provide: MsalService, useValue: msalService },
        { provide: ActivatedRoute, useValue: ActivateRouteStub }
      ]
    }).compileComponents();
    msalService = TestBed.get(MsalService);
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  }));

  it('should create profile component', () => {
    expect(component).toBeTruthy();
  });

  it('should define profile component', () => {
    expect(component).toBeDefined();
  });

  it('should get plan details tags for given plan id', () => {
    component.planId = mockPlanId;;
    mockPersonalizedPlanService.getActionPlanConditions.and.callFake(() => {
      return Observable.from([mockPlanDetailsjson]);
    });
    component.getTopics();
    expect(component.planDetailTags.id).toEqual(mockPlanId);
  });

  it('should get topics for given plan id', () => {
    component.planId = mockPlanId;;
    mockPersonalizedPlanService.getActionPlanConditions.and.callFake(() => {
      return Observable.from([mockPlanDetailsWithTopics]);
    });
    component.getTopics();
    expect(component.topics.length).toEqual(1);
  });

  it('should assign component values in filterTopics method', () => {
    let mockEvent = { "plan": mockPlanDetails, "topicsList": mockTopicsList };
    spyOn(component, 'filterPlan');
    component.filterTopics(mockEvent);
    expect(component.topics.length).toBeGreaterThan(1);
    expect(component.planDetailTags).toEqual(mockEvent.plan);
    expect(component.topicsList).toEqual(mockEvent.topicsList);
    expect(component.filterPlan).toHaveBeenCalledWith("");
  });

  it('should return filtered topics list infilterTopicsList  method when input topic is hidden', () => {
    component.tempTopicsList = mockFilteredTopicsList;
    component.filterTopicsList(mockFilterTopicName);
    expect(component.topicsList).toEqual(mockTopicsList);
  });

  it('should return filtered topics list based on the input topic and tempTopicsList in filterTopicsList method', () => {
    component.tempTopicsList = mockTopicsList;
    component.filterTopicsList(mockFilterTopicName);
    expect(component.topicsList).toEqual(mockFilteredTopicsList);
  });

  it('should return filtered topics list infilterTopicsList  method when input is blank in topic', () => {
    component.tempTopicsList = mockTopicsList;
    component.filterTopicsList(mockTopicListBlank);
    expect(component.topicsList).toEqual(mockTopicsList);
  });

});
