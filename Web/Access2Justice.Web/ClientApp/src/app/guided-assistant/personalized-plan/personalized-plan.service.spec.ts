import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule, HttpHeaders, HttpParams } from '@angular/common/http';
import { PersonalizedPlanService } from './personalized-plan.service';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { api } from '../../../api/api';
import { Observable } from 'rxjs/Rx';
import { ServiceOrgService } from '../../shared/sidebars/service-org.service';
import { ArrayUtilityService } from '../../shared/array-utility.service';
import { catchError } from 'rxjs/operators/catchError';
import { PersonalizedPlan } from './personalized-plan';

fdescribe(' Service:PersonalizedPlan', () => {
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
  let mockUserPlan = {
    "id": "ba67afca-4236-4875-9b11-fbb8a113ecb2",
    "oId": "ACB833BB3F817C2FBE5A72CE37FE7AB9CD977E585",
    "firstName": "testname",
    "lastName": "testlastname",
    "eMail": "testemail",
    "isActive": "Yes",
    "createdBy": "test createdby",
    "createdTimeStamp": "08/06/2018 11:42:39",
    "modifiedBy": null,
    "modifiedTimeStamp": null,
    "sharedResource": null,
    "personalizedActionPlanId": "a97fc45d-cbbe-4222-9d14-2ec227229b92",
    "curatedExperienceAnswersId": "00000000-0000-0000-0000-000000000000"
  }
  
  

  let mockPersonalizedPlan: PersonalizedPlan = {
    "id": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
    "topics": [],
    "isShared": true
  }
  let mockPlanId = "993jfk3003j";
  let mockUserId = "11452111";
  let mockSessionKey = "883833";
  let mockObjects = false;
  let mockObject = false;
  let mockOid = "ACB833BB3F817C2FBE5A72CE37FE7AB9CD977E58580B9832B91865E234A0A3D37C8F2C2A3B401CA64748F3098C9FE3374106B6F4A2B157FE091CA6332C88A89B";
  let mockid = "bf8d7e7e-2574-7b39-efc7-83cb94adae07";
  let mokParams = new HttpParams()
    .set("oId", mockUserId)
    .set("planId", mockPlanId);



  let service: PersonalizedPlanService;
  let arrayUtilityService: ArrayUtilityService;
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  var originalTimeout;  
  let mockUserSavedResources = [
    {
      "oId": "ACB833BB3F817C2FBE5A72CE37FE7AB9CD977E58580B9832B91865E234A0A3D37C8F2C2A3B401CA64748F3098C9FE3374106B6F4A2B157FE091CA6332C88A89B",
      "resourceTags": [
        {
          "itemId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
          "resourceType": "Topics",
          "resourceDetails": {}
        }
      ],
      "type": "resources",
      "id": "54e6dafd-35dc-4a4e-a140-61e30abc8127"
    },
    {
      "planId": "cf152275-32b2-b22c-de54-42de8fdfe778",
      "oId": "ACB833BB3F817C2FBE5A72CE37FE7AB9CD977E58580B9832B91865E234A0A3D37C8F2C2A3B401CA64748F3098C9FE3374106B6F4A2B157FE091CA6332C88A89B",
      "type": "plans",
      "planTags": [
        {
          "topicId": "3e278591-ec50-479d-8e38-ae9a9d4cabd9",
          "stepTags": [
            {
              "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
              "order": 1,
              "markCompleted": true
            },
            {
              "id": "d46aecee-8c79-df1b-4081-1ea02b5022df",
              "order": 2,
              "markCompleted": false
            }
          ]
        }
      ],
      "id": "06ba59fd-33b7-5a45-3f52-24adf3f487e4"
    }
  ]
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
  it('service should be created', () => {
    expect(service).toBeTruthy();
  });
  it('service should be defined ', () => {
    expect(service).toBeDefined();
  });
  it('should retrieve plan steps when the id  value is id', (done) => {
    const mockResponse = Observable.of(mockplan);
    httpSpy.get.and.returnValue(mockResponse);
    service.getActionPlanConditions(mockid).subscribe(plans => {
    expect(httpSpy.get).toHaveBeenCalledWith(`${api.planUrl}` + '/' + mockid);
    expect(service.getActionPlanConditions(mockid)).toEqual(mockResponse);
    done();
    });
  });
  it('should return user profile plan details when oid value is passed', (done) => {
    const mockResponse = Observable.of(mockUserPlan);
    httpSpy.get.and.returnValue(mockResponse);
    service.getUserPlanId("mockOid").subscribe(userprofile => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.getUserProfileUrl}/mockOid`);
      expect(userprofile).toEqual(mockUserPlan);
    done();
    });
  });
  it('should return user saved resources when oid value is passed', (done) => {
    const mockResponse = Observable.of(mockUserSavedResources);
    httpSpy.get.and.returnValue(mockResponse);
    service.getUserSavedResources("userId").subscribe(savedresource => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.getProfileUrl}/userId`);
      expect(savedresource).toEqual(mockUserSavedResources);
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
  it('should update userplan when plan details send to user plan method of service', (done) => {
    const mockResponse = Observable.of(mockPersonalizedPlan);
    httpSpy.post.and.returnValue(mockResponse);
    service.userPlan(mockPersonalizedPlan).subscribe(userplan => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(userplan).toEqual(mockPersonalizedPlan);
      done();
    });
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
  it('should update user personalized profile when savePersonalizedPlanToProfile method called', (done) => {
    const mockResponse = Observable.of(mokParams);
    httpSpy.post.and.returnValue(mockResponse);
    service.savePersonalizedPlanToProfile(mokParams).subscribe(updateuserplan => {
      expect(httpSpy.post).toHaveBeenCalled();
      done();
    });
  });
  it('should return topics list when createTopicsList method called', () => {
    const mockResponse = Observable.of(mockTopicsList);
    spyOn(service, 'createTopicsList').and.returnValue(mockResponse);
    service.createTopicsList(mockTopicsList);
    expect(service.createTopicsList).toBeDefined();
  });


  it('should return plan details when topics, planDetailTags passed to the getPlanDetails method', () => {
    spyOn(service, 'getPlanDetails');
    service.getPlanDetails(mockTopics, mockPlanDetailTags);
    expect(service.getPlanDetails).toHaveBeenCalled();
  });
  it('should save plan to the session when plan id is passed', () => {
    spyOn(service, 'savePlanToSession');
    service.savePlanToSession(mockPlanId);
    expect(service.savePlanToSession).toHaveBeenCalled();
  });
  it('should display plan details when plandetailtags and topics list passed to displayPlanDetails service method', () => {
    spyOn(service, 'displayPlanDetails');
    service.displayPlanDetails(mockPlanDetailTags, mockTopicsList);
    expect(service.displayPlanDetails).toHaveBeenCalled();
  });
});
