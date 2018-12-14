import { TestBed } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { PersonalizedPlanService } from './personalized-plan.service';
import { api } from '../../../api/api';
import { Observable } from 'rxjs/Observable';
import { ArrayUtilityService } from '../../shared/array-utility.service';
import { PersonalizedPlan, ProfileResources } from './personalized-plan';
import { ToastrService } from 'ngx-toastr';
import { Global } from '../../global';
import { of } from 'rxjs/observable/of';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { NgxSpinnerService } from 'ngx-spinner';

fdescribe('Service:PersonalizedPlan', () => {
  let mockPlanDetails = {
    "id": "29250697-8d22-4f9d-bbf8-96c1b5b72e54",
    "isShared": false,
    "topics": [
      {
        "topicId": "69be8e7c-975b-43c8-9af3-f61887a33ad3",
        "name": "Protective Order",
        "icon": null,
        "essentialReadings": [
          {
            "text": "Domestic Violence - What it is",
            "url": "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          },
          {
            "text": "Safety Planning Tips",
            "url": "https://www.thehotline.org/help/path-to-safety/"
          }
        ],
        "steps": [
          {
            "description": "Short Term",
            "isComplete": false,
            "order": 1,
            "resources": [],
            "stepId": "ec78414b-2616-4d65-ae07-e08f3e2f697a",
            "title": "Short Term Protective Order"
          },
          {
            "description": "Long Term",
            "isComplete": false,
            "order": 2,
            "resources": [],
            "stepId": "ed56894b-2616-4d65-ae07-e08f3e2f697a",
            "title": "Long Term Protective Order"
          }
        ]
      },
      {
        "topicId": "ba74f857-eb7b-4dd6-a021-5b3e4525e3e4",
        "name": "Divorce",
        "icon": null,
        "essentialReadings": [
          {
            "text": "Domestic Violence - What it is",
            "url": "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          }
        ],
        "steps": [
          {
            "description": "Step Description",
            "isComplete": false,
            "order": 1,
            "resources": [],
            "stepId": "df822558-73c2-4ac8-8259-fabe2334eb71",
            "title": "Step Title"
          }
        ]
      }
    ]
  };
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
  };
  let mockProfileResources: ProfileResources = {
    "oId": "mockUserId",
    "resourceTags": [],
    "type": "resources"
  };
  let mockProfileSavedResources = {
    "resources": [{ "id": "afbb8dc1-f721-485f-ab92-1bab60137e24", "address": "Girdwood, Anchorage, Hawii 99587", "resourceType": "Organizations" }],
    "topics": [{
      "icon": "https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/categories/money_debt.svg",
      "id": "5970328f-ceea-4903-b4dc-1ea559623582", "resourceType": "Topics"
    }]
  };
  let mockSavedResources = [
    {
      "itemId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
      "resourceType": "Topics",
      "resourceDetails": {}
    },
    {
      "itemId": "14a7dd22-f33d-4ed0-a4c3-9bad146f4fa6",
      "resourceType": "Articles",
      "resourceDetails": {}
    }
  ];
  let mockResourceInput = {
    "ContinuationToken": null, "IsResourceCountRequired": false, "Location": null,
    "PageNumber": 0, "ResourceIds": ["afbb8dc1-f721-485f-ab92-1bab60137e24"], "ResourceType": "ALL",
    "TopicIds": ["5970328f-ceea-4903-b4dc-1ea559623582"]
  };
  let mockTopicsList = [
    {
      "topic": {
        "topicId": "69be8e7c-975b-43c8-9af3-f61887a33ad3",
        "name": "Protective Order",
        "icon": null,
        "essentialReadings": [
          {
            "text": "Domestic Violence - What it is",
            "url": "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          },
          {
            "text": "Safety Planning Tips",
            "url": "https://www.thehotline.org/help/path-to-safety/"
          }
        ],
        "steps": [
          {
            "description": "Short Term",
            "isComplete": false,
            "order": 1,
            "resources": [],
            "stepId": "ec78414b-2616-4d65-ae07-e08f3e2f697a",
            "title": "Short Term Protective Order"
          },
          {
            "description": "Long Term",
            "isComplete": false,
            "order": 2,
            "resources": [],
            "stepId": "ed56894b-2616-4d65-ae07-e08f3e2f697a",
            "title": "Long Term Protective Order"
          }
        ]
      },
      "isSelected": true
    },
    {
      "topic": {
        "topicId": "ba74f857-eb7b-4dd6-a021-5b3e4525e3e4",
        "name": "Divorce",
        "icon": null,
        "essentialReadings": [
          {
            "text": "Domestic Violence - What it is",
            "url": "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          }
        ],
        "steps": [
          {
            "description": "Step Description",
            "isComplete": false,
            "order": 1,
            "resources": [],
            "stepId": "df822558-73c2-4ac8-8259-fabe2334eb71",
            "title": "Step Title"
          }
        ]
      },
      "isSelected": true
    }
  ];
  let mockTopics = [{
    "topicId": "69be8e7c-975b-43c8-9af3-f61887a33ad3",
    "name": "Protective Order",
    "icon": null,
    "essentialReadings": [
      {
        "text": "Domestic Violence - What it is",
        "url": "https://www.thehotline.org/is-this-abuse/abuse-defined/"
      },
      {
        "text": "Safety Planning Tips",
        "url": "https://www.thehotline.org/help/path-to-safety/"
      }
    ],
    "steps": [
      {
        "description": "Short Term",
        "isComplete": false,
        "order": 1,
        "resources": [],
        "stepId": "ec78414b-2616-4d65-ae07-e08f3e2f697a",
        "title": "Short Term Protective Order"
      },
      {
        "description": "Long Term",
        "isComplete": false,
        "order": 2,
        "resources": [],
        "stepId": "ed56894b-2616-4d65-ae07-e08f3e2f697a",
        "title": "Long Term Protective Order"
      }
    ]
  },
  {
    "topicId": "ba74f857-eb7b-4dd6-a021-5b3e4525e3e4",
    "name": "Divorce",
    "icon": null,
    "essentialReadings": [
      {
        "text": "Domestic Violence - What it is",
        "url": "https://www.thehotline.org/is-this-abuse/abuse-defined/"
      }
    ],
    "steps": [
      {
        "description": "Step Description",
        "isComplete": false,
        "order": 1,
        "resources": [],
        "stepId": "df822558-73c2-4ac8-8259-fabe2334eb71",
        "title": "Step Title"
      }
    ]
  }
  ];
  let mockFilteredTopicsList = [
    {
      "topic": {
        "topicId": "69be8e7c-975b-43c8-9af3-f61887a33ad3",
        "name": "Protective Order",
        "icon": null,
        "essentialReadings": [
          {
            "text": "Domestic Violence - What it is",
            "url": "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          },
          {
            "text": "Safety Planning Tips",
            "url": "https://www.thehotline.org/help/path-to-safety/"
          }
        ],
        "steps": [
          {
            "description": "Short Term",
            "isComplete": false,
            "order": 1,
            "resources": [],
            "stepId": "ec78414b-2616-4d65-ae07-e08f3e2f697a",
            "title": "Short Term Protective Order"
          },
          {
            "description": "Long Term",
            "isComplete": false,
            "order": 2,
            "resources": [],
            "stepId": "ed56894b-2616-4d65-ae07-e08f3e2f697a",
            "title": "Long Term Protective Order"
          }
        ]
      },
      "isSelected": true
    },
    {
      "topic": {
        "topicId": "ba74f857-eb7b-4dd6-a021-5b3e4525e3e4",
        "name": "Divorce",
        "icon": null,
        "essentialReadings": [
          {
            "text": "Domestic Violence - What it is",
            "url": "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          }
        ],
        "steps": [
          {
            "description": "Step Description",
            "isComplete": false,
            "order": 1,
            "resources": [],
            "stepId": "df822558-73c2-4ac8-8259-fabe2334eb71",
            "title": "Step Title"
          }
        ]
      },
      "isSelected": false
    }
  ];
  let mockFilteredPlanDetails = {
    "id": "29250697-8d22-4f9d-bbf8-96c1b5b72e54",
    "isShared": false,
    "topics": [
      {
        "topicId": "69be8e7c-975b-43c8-9af3-f61887a33ad3",
        "name": "Protective Order",
        "icon": null,
        "essentialReadings": [
          {
            "text": "Domestic Violence - What it is",
            "url": "https://www.thehotline.org/is-this-abuse/abuse-defined/"
          },
          {
            "text": "Safety Planning Tips",
            "url": "https://www.thehotline.org/help/path-to-safety/"
          }
        ],
        "steps": [
          {
            "description": "Short Term",
            "isComplete": false,
            "order": 1,
            "resources": [],
            "stepId": "ec78414b-2616-4d65-ae07-e08f3e2f697a",
            "title": "Short Term Protective Order"
          },
          {
            "description": "Long Term",
            "isComplete": false,
            "order": 2,
            "resources": [],
            "stepId": "ed56894b-2616-4d65-ae07-e08f3e2f697a",
            "title": "Long Term Protective Order"
          }
        ]
      }
    ]
  };
  let mockResponse = Observable.of(mockPlanDetails);
  let service: PersonalizedPlanService;
  let arrayUtilityService: ArrayUtilityService;
  let ngxSpinnerService: NgxSpinnerService;
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post', 'put']);
  let toastrService: ToastrService;
  let global: Global;
  var originalTimeout;
  let mockMapLocation = {
    state: "California",
    city: "Riverside County",
    county: "Indio",
    zipCode: "92201"
  };

  let mockDisplayLocationDetails = {
    locality: "Indio",
    address: "92201"
  };

  let mockLocationDetails = {
    location: mockMapLocation,
    displayLocationDetails: mockDisplayLocationDetails,
    country: "United States",
    formattedAddress: "Hjorth St, Indio, California 92201, United States"
  };
  let mockSavedTopics = ["Divorce", "ChildCustody"];
  let mockTopicDetails = [
    {
      "name": "Divorce", "id": "Divorce", "resourceType": "Topics"
    },
    {
      "name": "ChildCustody", "id": "ChildCustody", "resourceType": "Topics"
    }];

  beforeEach(() => {

    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [PersonalizedPlanService, ArrayUtilityService,
        { provide: Global, useValue: { global, role: '', shareRouteUrl: '', userId: 'UserId', sessionKey: 'test', topicsSessionKey:'test2' } },
        NgxSpinnerService]
    });
    service = new PersonalizedPlanService(httpSpy, arrayUtilityService, toastrService, global, ngxSpinnerService);
    global = TestBed.get(Global);
    arrayUtilityService = new ArrayUtilityService();
    httpSpy.get.calls.reset();

    originalTimeout = jasmine.DEFAULT_TIMEOUT_INTERVAL;
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 1000000;

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

    spyOn(sessionStorage, 'setItem')
      .and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, 'removeItem')
      .and.callFake(mockSessionStorage.removeItem);
    spyOn(sessionStorage, 'clear')
      .and.callFake(mockSessionStorage.clear);
  });

  it('service should be created', () => {
    expect(service).toBeTruthy();
  });

  it('service should be defined ', () => {
    expect(service).toBeDefined();
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
    const mockResponse = Observable.of(mockPlanDetails);
    spyOn(service, 'userPlan').and.returnValue(mockResponse);
    httpSpy.post.and.returnValue(mockResponse);
    service.userPlan(mockPlanDetails).subscribe(userplan => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(userplan).toEqual(mockPlanDetails);
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

  it('should get topics details in getTopicDetails method of service', (done) => {
    let mockIntentInput = { location: mockMapLocation, intents: mockSavedTopics };
    const mockResponse = Observable.of(mockTopicDetails);
    spyOn(service, 'getTopicDetails').and.returnValue(mockTopicDetails);
    httpSpy.post.and.returnValue(mockResponse);
    service.getTopicDetails(mockIntentInput).subscribe(topics => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(topics).toEqual(mockTopicDetails);
      done();
    });
  })

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

  it('should display filtered plan details when plandetailtags and topics list passed to displayPlanDetails service method', () => {
    service.displayPlanDetails(mockPlanDetails, mockFilteredTopicsList);
    expect(service.tempPlanDetailTags).toEqual(mockFilteredPlanDetails);
  });

  it('should call getTopicsFromGuidedAssistant with session stored resources in saveResourcesToUserProfile method', () => {
    spyOn(sessionStorage, 'getItem')
      .and.returnValue(JSON.stringify(mockSavedResources));
    spyOn(service, 'saveResourceToProfilePostLogin');
    service.saveResourcesToUserProfile();
    expect(service.resourceTags).toEqual(mockSavedResources);
    expect(service.getTopicsFromGuidedAssistant).toHaveBeenCalled();
  });

  it('checkExistingSavedResources for saved resource exists', () => {
    let mockExists = true;
    spyOn(arrayUtilityService, 'checkObjectExistInArray').and.callFake(() => {
      return Observable.from([mockExists]);
    });
    service.checkExistingSavedResources(mockSavedResources);
    expect(service.showWarning).toBeTruthy();
    expect(sessionStorage.getItem(global.sessionKey)).toBeNull;
  });

  it('should return resource ids from resources list', () => {
    let mockResources = [{ "id": "resource1", "resourceType": "Organizations" }, { "id": "resource2", "resourceType": "Articles" }];
    let mockResourceIds = ["resource1", "resource2"];
    service.getResourceIds(mockResources);
    expect(service.resourceIds).toEqual(mockResourceIds);
  });

  it('', () = {

  });

});
