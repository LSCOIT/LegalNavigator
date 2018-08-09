import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule, HttpHeaders } from '@angular/common/http';
import { PersonalizedPlanService } from './personalized-plan.service';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { api } from '../../../api/api';
import { Observable } from 'rxjs/Rx';
import { ServiceOrgService } from '../../shared/sidebars/service-org.service';
import { ArrayUtilityService } from '../../shared/array-utility.service';
import { catchError } from 'rxjs/operators/catchError';

describe(' Service:PersonalizedPlan', () => {
  const mockHttpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  let mockupdateplan = {
    "planId": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
    "oId": "User Id",
    "type": "plans",
    "planTags": [
      {
        "topicId": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
        "stepTags": [
          {
            "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
            "order": 1,
            "markCompleted": true
          }]
      }]
  };
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
  let mockResourceInput = {
    "ResourceType": "testtype",
    "ContinuationToken": "",
    "TopicIds": "",
    "ResourceIds": "",
    "PageNumber": 1,
    "Location": "testlocation"
  };
  let mockProfileData = {
    "planId": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
    "oId": "User Id",
    "type": "plans",
    "planTags": [{
      "topicId": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
      "stepTags": [
        {
          "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
          "order": 1,
          "markCompleted": true
        }]
    }]
  };
  let mockPlanDetailTags = [{
    "id": "393994-133-33",
    "oId": "93004kdf033keke;",
    "planTags": [{}],
    "type": "testtype"
  }];
  let mockActiveRoute: ActivatedRoute;
  let resoureStorage = sessionStorage.getItem(this.mockSessionKey);
  let mockResponse = Observable.of(mockPlanDetailsJson);
  let profileStorage = sessionStorage.getItem(this.mockProfileData);
  let mockplan = {
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
      }] 
  }
  let mockTopics = ["topic1", "topic2"];
  let mockTopicsList = [{ topic: "topic1", isSelected: true }, { topic: "topic2", isSelected: true }];
  let mockUserPlan = [
    {
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
          ]
        }
      ]
    }
  ]
  let oid = "123456789";
  let mockPlanId = "993jfk3003j"
  let mockSessionKey = "883833";
  let mockObjects = false;
  let mockObject = false;
  let service: PersonalizedPlanService;
  let arrayUtilityService: ArrayUtilityService;
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  var originalTimeout;  

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [PersonalizedPlanService, ArrayUtilityService]
    });
    service = new PersonalizedPlanService(httpSpy, arrayUtilityService);
    arrayUtilityService = new ArrayUtilityService();
    httpSpy.get.calls.reset();
  });

  beforeEach(function () {
    originalTimeout = jasmine.DEFAULT_TIMEOUT_INTERVAL;
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 10000;
    it('should call getBookmarkedData when getPersonalizedPlan called', (done) => {
      spyOn(service, 'getBookmarkedData');
      service.topics = mockTopics;
      service.getBookmarkedData();
      expect(service.getBookmarkedData).toBeDefined();
    });
  });

  afterEach(function () {
    jasmine.DEFAULT_TIMEOUT_INTERVAL = originalTimeout;
  });

  it('service should be created', () => {
    expect(service).toBeTruthy();
  });

  it('service should be defined ', () => {
    expect(service).toBeDefined();
  });

  it('should retrieve plan steps when the id  value is id', (done) => {
    let mockid = "bf8d7e7e-2574-7b39-efc7-83cb94adae07";
    const mockResponse = Observable.of(mockplan);
    httpSpy.get.and.returnValue(mockResponse);
    service.getActionPlanConditions(mockid).subscribe(plans => {
    expect(httpSpy.get).toHaveBeenCalledWith(`${api.planUrl}` + '/' + mockid);
    expect(service.getActionPlanConditions(mockid)).toEqual(mockResponse);
    done();
    });
  });

  it('should return user profile plan details', (done) => {
    const mockResponse = Observable.of(mockUserPlan);
    httpSpy.get.and.returnValue(mockResponse);
    service.getUserPlanId("userId").subscribe(userPlan => {
    expect(httpSpy.get).toHaveBeenCalledWith(`${api.getProfileUrl}/userId`);
    expect(userPlan).toEqual(mockUserPlan);
    done();
    });
  });

  it('should return mark completed updated plan when passed value is update plan', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    service.getMarkCompletedUpdatedPlan(mockupdateplan).subscribe(updateplan => {
    expect(httpSpy.post).toHaveBeenCalled();
    expect(updateplan).toEqual(mockPlanDetailsJson);
    done();
    });
  });

  it('should return userplan when value passed is plan', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    //service.userPlan(mockupdateplan).subscribe(userplan => {
    //expect(httpSpy.post).toHaveBeenCalled();
    //expect(userplan).toEqual(mockPlanDetailsJson);
    //done();
    //});
  });

  it('should call getPersonalizedResources when value is  resource input', () => {
    spyOn(service, 'getBookmarkedData');
    spyOn(service, 'getPersonalizedResources');
    expect(service.getPersonalizedResources).toBeDefined();
  });

  it('should return resources when saveResourcesToSession method called', () => {
    spyOn(service, 'saveResourcesToSession');
    spyOn(arrayUtilityService, 'checkObjectExistInArray');
    service.saveResourcesToSession(resoureStorage);
    arrayUtilityService.checkObjectExistInArray(mockObjects, mockObject);
    expect(service.saveResourcesToSession).toHaveBeenCalled();
    expect(arrayUtilityService.checkObjectExistInArray).toHaveBeenCalled();
  });

  it('should return topics list when createTopicsList method called', () => {
    const mockResponse = Observable.of(mockTopicsList);
    spyOn(service, 'createTopicsList').and.returnValue(mockResponse);
    service.createTopicsList(mockTopicsList);
    expect(service.createTopicsList).toBeDefined();
  });
});
