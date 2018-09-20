import { TestBed } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { PersonalizedPlanService } from './personalized-plan.service';
import { api } from '../../../api/api';
import { Observable } from 'rxjs/Rx';
import { ArrayUtilityService } from '../../shared/array-utility.service';
import { PersonalizedPlan, ProfileResources } from './personalized-plan';
import { ToastrService } from 'ngx-toastr';
import { Global } from '../../global';

fdescribe('Service:PersonalizedPlan', () => {
  let mockPlanDetailsJson = {
    "id": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
    "oId": "User Id",
    "type": "plans",
    "planTags": [
      {
        "topicId": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
        "stepTags": [
          {
            "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
            "order": 1,
            "markCompleted": false
          }
        ],
        "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba"
      }
    ]
  };
  let mockPersonalizedPlan: PersonalizedPlan = {
    "id": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
    "topics": [],
    "isShared": true
  }
  let mockProfileResources: ProfileResources = {
    "oId": "mockUserId",
    "resourceTags": [],
    "type": "resources"
  }
  let mockUserSavedResources = {
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
  }
  let mockProfileSavedResources = {
    "resources": [{ "id": "afbb8dc1-f721-485f-ab92-1bab60137e24", "address": "Girdwood, Anchorage, Hawii 99587", "resourceType": "Organizations" }],
    "topics": [{
      "icon": "https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/categories/money_debt.svg",
      "id": "5970328f-ceea-4903-b4dc-1ea559623582", "resourceType": "Topics"
    }]
  };
  let mockResourceInput = {
    "ContinuationToken": null, "IsResourceCountRequired": false, "Location": null,
    "PageNumber": 0, "ResourceIds": ["afbb8dc1-f721-485f-ab92-1bab60137e24"], "ResourceType": "ALL",
    "TopicIds": ["5970328f-ceea-4903-b4dc-1ea559623582"]
  };
  let mockTopics = [
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
    }];
  let mockTopicsList = [
    {
      "isSelected": true,
      "topic": {
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
  let mockResponse = Observable.of(mockPlanDetailsJson);
  let mockid = "bf8d7e7e-2574-7b39-efc7-83cb94adae07";
  let service: PersonalizedPlanService;
  let arrayUtilityService: ArrayUtilityService;
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post', 'put']);
  let toastrService: ToastrService;
  let global: Global;
  var originalTimeout;

  beforeEach(() => {

    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [PersonalizedPlanService, ArrayUtilityService,
        { provide: Global, useValue: { role: '', shareRouteUrl: '', userId:'UserId' } }]
    });
    service = new PersonalizedPlanService(httpSpy, arrayUtilityService, toastrService, global);
    global = TestBed.get(Global);
    arrayUtilityService = new ArrayUtilityService();
    httpSpy.get.calls.reset();

    originalTimeout = jasmine.DEFAULT_TIMEOUT_INTERVAL;
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 1000000;
  });

  it('service should be created', () => {
    expect(service).toBeTruthy();
  });

  it('service should be defined ', () => {
    expect(service).toBeDefined();
  });

  it('should retrieve plan steps when the id  value is id', (done) => {
    httpSpy.get.and.returnValue(mockResponse);
    service.getActionPlanConditions(mockid).subscribe(plans => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.planUrl}` + '/' + mockid);
      expect(service.getActionPlanConditions(mockid)).toEqual(mockResponse);
      done();
    });
  });

  it('should return user saved resources when oid value is passed', (done) => {
    const mockResponse = Observable.of(mockUserSavedResources);
    httpSpy.post.and.returnValue(mockResponse);
    service.getUserSavedResources("userId").subscribe(savedresource => {
      expect(httpSpy.post).toHaveBeenCalledWith(`${api.getProfileUrl}`, "userId");
      expect(savedresource).toEqual(mockUserSavedResources);
      done();
    });
  });

  it('should save resources and return updated data in saveResources service method', (done) => {
    const mockResponse = Observable.of(mockUserSavedResources);
    httpSpy.post.and.returnValue(mockResponse);
    service.saveResources(mockProfileResources).subscribe(savedResource => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(savedResource).toEqual(mockUserSavedResources);
      done();
    });
  });

  it('should update userplan when plan details send to user plan method of service', (done) => {
    const mockResponse = Observable.of(mockPersonalizedPlan);
    spyOn(service, 'userPlan').and.returnValue(mockResponse);
    httpSpy.post.and.returnValue(mockResponse);
    service.userPlan(mockPersonalizedPlan).subscribe(userplan => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(userplan).toEqual(mockPersonalizedPlan);
      done();
    });
  });

  it('should user Saved Resources in getPersonalizedResources method', (done) => {
    const mockResponse = Observable.of(mockProfileSavedResources);
    httpSpy.put.and.returnValue(mockResponse);
    service.getPersonalizedResources(mockResourceInput).subscribe(userResources => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(userResources).toEqual(mockProfileSavedResources);
      done();
    });
  });

  it('should return plan details when topics, planDetailTags passed to the getPlanDetails method', () => {
    spyOn(service, 'createTopicsList').and.returnValue(mockTopicsList);
    spyOn(service, 'displayPlanDetails').and.returnValue(mockPlanDetails);
    service.getPlanDetails(mockTopics, mockPlanDetails);
    expect(service.planDetails).toEqual(mockPlanDetails);
  });

  it('should return topics list when createTopicsList method called', () => {
    service.createTopicsList(mockTopics);
    expect(service.topicsList).toEqual(mockTopicsList);
  });

  it('should display plan details when plandetailtags and topics list passed to displayPlanDetails service method', () => {
    service.displayPlanDetails(mockPlanDetails, mockTopicsList);
    expect(service.tempPlanDetailTags).toEqual(mockPlanDetails);
  });

  it('should call showWarning if saved resources already exists in saveResourcesToProfile method', () => {
    spyOn(service, 'showWarning');
    let mockResponse = [mockUserSavedResources];
    spyOn(service, 'getUserSavedResources').and.callFake(() => {
      return Observable.from([mockResponse]);
    });

    let mockExists = true;
    spyOn(arrayUtilityService, 'checkObjectExistInArray').and.callFake(() => {
      return Observable.from([mockExists]);
    });
    let mockSavedResource = {
      "itemId": "1d7fc811-dabe-4468-a179-c55075bd22b6",
      "resourceType": "Organizations",
      "resourceDetails": {}
    };
    sessionStorage.setItem(service.sessionKey, "test");
    let mockUserId = "mockUserId";
    spyOn(service, 'saveResourcesToProfile');
    service.saveResourcesToProfile(mockSavedResource);
    expect(service.showWarning).toBeTruthy();
    expect(sessionStorage.getItem(service.sessionKey)).toBeNull;
  });

  it('should add resource by calling saveResourceToProfile if saved resources doesnot exists in saveResourcesToProfile method', () => {
    let mockResponse = [mockUserSavedResources];
    spyOn(service, 'getUserSavedResources').and.callFake(() => {
      return Observable.from([mockResponse]);
    });
    spyOn(service, 'saveResourceToProfile');
    let mockExists = false;
    spyOn(arrayUtilityService, 'checkObjectExistInArray').and.callFake(() => {
      return Observable.from([mockExists]);
    });
    let mockSavedResource = {
      "itemId": "1d7fc811-dabe-4468-a179-c43435bd22b6",
      "resourceType": "Articles",
      "resourceDetails": {}
    };
    sessionStorage.setItem(service.sessionKey, "test");
    service.saveResourcesToProfile(mockSavedResource);
    expect(service.saveResourceToProfile).toHaveBeenCalledWith(service.resourceTags);
    expect(sessionStorage.getItem(service.sessionKey)).toBeNull;
  });

  it('should call showSuccess service method on saveResourceToProfile', () => {
    let mockResourceTags = [];
    spyOn(service, 'showSuccess');
    spyOn(service, 'saveResources').and.callFake(() => {
      return Observable.from([mockUserSavedResources]);
    });
    service.saveResourceToProfile(mockResourceTags);
    expect(service.showSuccess).toHaveBeenCalled();
  });

  it('should return resource ids from resources list', () => {
    let mockResources = [{ "id": "resource1", "resourceType": "Organizations" }, { "id": "resource2", "resourceType": "Articles" }];
    let mockResourceIds = ["resource1", "resource2"];
    service.getResourceIds(mockResources);
    expect(service.resourceIds).toEqual(mockResourceIds);
  });
});
