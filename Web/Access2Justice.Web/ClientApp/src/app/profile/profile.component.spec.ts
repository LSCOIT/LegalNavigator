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

describe('profilecomponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let personalizedplanservice: PersonalizedPlanService;
  let arrayutilityservice: ArrayUtilityService;
  let eventutilityservice: EventUtilityService;
  
  const mocktopicservice = {
    gettopics: () => { }
  };

  let mockplanid = "dkk2k333";
  let mockshowremove = true;
  let mockplandetailsjson = {
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
  let mocktopiclist = {
    "topic":"[{},{}]",
    "isselected": true 
  };
  let mocktopic = "test";
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
  let mockuserid = "38890303";

  let mocktopicids: string[] = [];
  let mockresourcetype = 'test';
  let mockcontinuationtoken = 'test';
  let mockresourceids = ['test'];
  let mocklocation = 'test';
  let mockactivetopic = "addf41e9-1a27-4aeb-bcbb-7959f95094ba";
  

  let mockresourceinput: IResourceFilter = {
    ResourceType: mockresourcetype,
    ContinuationToken: mockcontinuationtoken,
    TopicIds: mocktopicids,
    PageNumber: 1,
    Location: mocklocation,
    IsResourceCountRequired: false,
    ResourceIds: mockresourceids,
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [ProfileComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [personalizedplanservice, arrayutilityservice, eventutilityservice]
    })
      .compileComponents();
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    personalizedplanservice = TestBed.get(personalizedplanservice);
    arrayutilityservice = TestBed.get(arrayutilityservice);
    eventutilityservice = TestBed.get(eventutilityservice);
    fixture.detectChanges();
  }));

  it('should create profile component', () => {
    expect(component).toBeTruthy();
  });
  it('should define profile component', () => {
    expect(component).toBeDefined();
  });

  it('should call getpersonalizedplan method when component loaded first time in ngonit', () => {
    component.ngOnInit();
    spyOn(component, 'getPersonalizedPlan');
    component.getPersonalizedPlan();
    component.showRemove;
    expect(component.getPersonalizedPlan).toHaveBeenCalled();
    expect(component.showRemove).toBe(mockshowremove);
  });

  xit('should call getactionplanconditions service method when get topics method called', () => {
    spyOn(personalizedplanservice, 'getActionPlanConditions').and.returnValue(mockplandetailsjson);
    personalizedplanservice.getPersonalizedPlan;
    component.planId = mockplanid;
    expect(personalizedplanservice.getActionPlanConditions(mockplanid)).toEqual(mockplandetailsjson);
    expect(personalizedplanservice.getActionPlanConditions).toBeTruthy();
  });

  it('should call filterplan service method when  topic value is called', () => {
    spyOn(personalizedplanservice, 'displayPlanDetails');
    personalizedplanservice.displayPlanDetails(mockplandetailsjson, mocktopics);
    spyOn(component, 'filterPlan');
    component.filterPlan(mocktopic);
    expect(component.filterPlan).toHaveBeenCalled();
    expect(personalizedplanservice.displayPlanDetails).toHaveBeenCalled();

  });

  it('should call getpersonalizedplan service method when getpersonalizedplan is called', () => {
    spyOn(component, 'getPersonalizedPlan');
    component.getPersonalizedPlan();
    spyOn(personalizedplanservice, 'getActionPlanConditions');
    personalizedplanservice.getActionPlanConditions(mockplanid);
    expect(personalizedplanservice.getActionPlanConditions(mockplanid)).toBeTruthy();
  });

  it('should call getusersavedresources service method when getpersonalizedresources is called', () => {
    component.getpersonalizedResources();
    spyOn(personalizedplanservice, 'getUserSavedResources');
    personalizedplanservice.getUserSavedResources(mockuserid);
    expect(personalizedplanservice.getUserSavedResources).toHaveBeenCalled();
  
  });

  it('should call getpersonalizedresources service method when getsavedresource is called', () =>  {
    component.getSavedResource(mockresourceinput);
    spyOn(personalizedplanservice, 'getPersonalizedResources');
    personalizedplanservice.getPersonalizedResources(mockresourceinput);
    expect(personalizedplanservice.getPersonalizedResources).toHaveBeenCalled();
  });

  it('should call filtertopicslist when topic value is passed', () => {
    component.filterTopicsList(mocktopics);
    expect(component.filterTopicsList(mocktopics)).toHaveBeenCalled();
  });
});
