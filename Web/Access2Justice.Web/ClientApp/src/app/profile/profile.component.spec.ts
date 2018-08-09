import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProfileComponent } from './profile.component';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { ArrayUtilityService } from '../shared/array-utility.service';
import { EventUtilityService } from '../shared/event-utility.service';
import { Tree } from '@angular/router/src/utils/tree';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';

fdescribe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let personalizedPlanService: PersonalizedPlanService;
  let arrayUtilityService: ArrayUtilityService;
  let eventUtilityService: EventUtilityService;
  
  const mockTopicService = {
    getTopics: () => { }
  };

  let mockPlanId = "dkk2k333";
  let mockShowRemove = true;
  let mockPlanDetailsJson = {
    "id": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
    "oId": "User Id",
    "type": "plans",
    "planTags": [{
        "topicId": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
        "stepTags": [
          {
            "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
            "order": 1,
            "markCompleted": false
          },
          {
            "id": "d46aecee-8c79-df1b-4081-1ea02b5022df",
            "order": 2,
            "markCompleted": false
          }
        ],
        "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
      }
    ]
  };
  let mockTopicList = {
    "topic":"[{},{}]",
    "isSelected": true 
  };
  let mockTopic = "test";
  let mockTopics = {
    "isSelected": true,
    "topic": {
      "topicId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
      "name": "Divorce",
      "quickLinks": [
        {
          "Text": "Filing for Dissolution or Divorce - Ending Your Marriage",
          "Url": "http://courts.alaska.gov/shc/family/shcstart.htm#issues"
        },
        {
          "Text": "Spousal Support (Alimony)",
          "Url": "https://alaskalawhelp.org/resource/spousal-support?ref=OEGQX"
        }
      ],
      "icon": "https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/topics/housing.svg",
      "steps": [
        {
          "stepId": "66a1e288-4ab1-480f-9d29-eb16abd3ae69"
        }
      ]
    }
  }
  let mockUserid = "38890303";

  let mockTopicIds: string[] = [];
  let mockResourceType = 'test';
  let mockContinuationToken = 'test';
  let mockResourceIds = ['test'];
  let mockLocation = 'test';
  let mockActiveTopic = "addf41e9-1a27-4aeb-bcbb-7959f95094ba";
  

  let mockResourceInput: IResourceFilter = {
    ResourceType: mockResourceType,
    ContinuationToken: mockContinuationToken,
    TopicIds: mockTopicIds,
    PageNumber: 1,
    Location: mockLocation,
    IsResourceCountRequired: false,
    ResourceIds: mockResourceIds,
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [ProfileComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [PersonalizedPlanService, ArrayUtilityService, EventUtilityService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    personalizedPlanService = TestBed.get(PersonalizedPlanService);
    arrayUtilityService = TestBed.get(ArrayUtilityService);
    eventUtilityService = TestBed.get(EventUtilityService);
    fixture.detectChanges();
  }));

  it('should create profile component', () => {
    expect(component).toBeTruthy();
  });
  it('should define profile component', () => {
    expect(component).toBeDefined();
  });

  it('should call getPersonalizedPlan method when component loaded first time in ngOnit', () => {
    component.ngOnInit();
    spyOn(component, 'getPersonalizedPlan');
    component.getPersonalizedPlan();
    component.showRemove;
    expect(component.getPersonalizedPlan).toHaveBeenCalled();
    expect(component.showRemove).toBe(mockShowRemove);
  });

  it('should call getActionPlanConditions service method when get topics method called', () => {
    spyOn(personalizedPlanService, 'getActionPlanConditions').and.returnValue(mockPlanDetailsJson);
    personalizedPlanService.getPersonalizedPlan;
    component.planId = mockPlanId;
    expect(personalizedPlanService.getActionPlanConditions(mockPlanId)).toEqual(mockPlanDetailsJson);
    expect(personalizedPlanService.getActionPlanConditions).toBeTruthy();
  });

  it('should call filterPlan service method when  topic value is called', () => {
    spyOn(personalizedPlanService, 'displayPlanDetails');
    personalizedPlanService.displayPlanDetails(mockPlanDetailsJson, mockTopics);
    spyOn(component, 'filterPlan');
    component.filterPlan(mockTopic);
    expect(component.filterPlan).toHaveBeenCalled();
    expect(personalizedPlanService.displayPlanDetails).toHaveBeenCalled();

  });

  it('should call getPersonalizedPlan service method when getPersonalizedPlan is called', () => {
    spyOn(component, 'getPersonalizedPlan');
    component.getPersonalizedPlan();
    spyOn(personalizedPlanService, 'getPersonalizedPlan');
    personalizedPlanService.getActionPlanConditions(mockPlanId);
    expect(personalizedPlanService.getActionPlanConditions(mockPlanId)).toBeTruthy();
  });

  it('should call getUserSavedResources service method when getpersonalizedResources is called', () => {
    component.getpersonalizedResources();
    spyOn(personalizedPlanService, 'getUserSavedResources');
    personalizedPlanService.getUserSavedResources(mockUserid);
    expect(personalizedPlanService.getUserSavedResources).toHaveBeenCalled();
  
  });

  it('should call getPersonalizedResources service method when getSavedResource is called', () =>  {
    component.getSavedResource(mockResourceInput);
    spyOn(personalizedPlanService, 'getPersonalizedResources');
    personalizedPlanService.getPersonalizedResources(mockResourceInput);
    expect(personalizedPlanService.getPersonalizedResources).toHaveBeenCalled();
  });

  it('should call filterTopicsList when topic value is passed', () => {
    component.filterTopicsList(mockTopics);
    expect(component.filterTopicsList(mockTopics)).toHaveBeenCalled();
  });
});
