import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ProfileComponent } from './profile.component';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { ArrayUtilityService } from '../shared/array-utility.service';
import { EventUtilityService } from '../shared/event-utility.service';
import { IResourceFilter } from '../shared/search/search-results/search-results.model';
import { ToastrService, ToastrModule } from 'ngx-toastr';

describe('component:profile', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let personalizedplanservice: PersonalizedPlanService;
  let arrayutilityservice: ArrayUtilityService;
  let eventutilityservice: EventUtilityService;
  let mockplanid = "ba67afca-4236-4875";
  let mockshowremove = true;
  let mocktopic = "test";
  let mockuserid = "38890303";
  let mocktopicids: string[] = [];
  let mockresourcetype = 'test';
  let mockcontinuationtoken = 'test';
  let mockresourceids = ['test'];
  let mocklocation = 'test';
  let mockresourceinput: IResourceFilter = {
    ResourceType: mockresourcetype,
    ContinuationToken: mockcontinuationtoken,
    TopicIds: mocktopicids,
    PageNumber: 1,
    Location: mocklocation,
    IsResourceCountRequired: false,
    ResourceIds: mockresourceids,
  };
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
  let mocktopics = {
    "isselected": true,
    "topic": {
      "topicid": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
      "name": "divorce",
      "quicklinks": [
        {
          "text": "filing for dissolution or divorce - ending your marriage",
          "url": "http://courts.alaska.gov"
        },
        {
          "text": "spousal support (alimony)",
          "url": "https://alaskalawhelp.org"
        }
      ],
      "icon": "https://test.com/static-resource/assets/images/topics/housing.svg",
      "steps": [
        {
          "stepid": "66a1e288-4ab1-480f-9d29-eb16abd3ae69"
        }
      ]
    }
  }
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule, ToastrModule.forRoot()],
      declarations: [ProfileComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        PersonalizedPlanService,
        EventUtilityService,
        ArrayUtilityService,
        ToastrService
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    personalizedplanservice = TestBed.get(PersonalizedPlanService);
    arrayutilityservice = TestBed.get(ArrayUtilityService);
    eventutilityservice = TestBed.get(EventUtilityService);
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

  it('should call getactionplanconditions service method when get topics method called', () => {
    spyOn(personalizedplanservice, 'getActionPlanConditions').and.returnValue(mockplandetailsjson);
    component.planId = mockplanid;
    personalizedplanservice.getActionPlanConditions(mockplanid);
    expect(personalizedplanservice.getActionPlanConditions).toHaveBeenCalled();
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
    spyOn(personalizedplanservice, 'getPersonalizedPlan');
    personalizedplanservice.getPersonalizedPlan();
    expect(personalizedplanservice.getPersonalizedPlan).toHaveBeenCalled();
  });

  it('should call getusersavedresources service method when getpersonalizedresources is called', () => {
    component.getpersonalizedResources();
    spyOn(personalizedplanservice, 'getUserSavedResources');
    personalizedplanservice.getUserSavedResources(mockuserid);
    expect(personalizedplanservice.getUserSavedResources).toHaveBeenCalled();
  });

  it('should call getpersonalizedresources service method when getsavedresource is called', () => {
    component.getSavedResource(mockresourceinput);
    spyOn(personalizedplanservice, 'getPersonalizedResources');
    personalizedplanservice.getPersonalizedResources(mockresourceinput);
    expect(personalizedplanservice.getPersonalizedResources).toHaveBeenCalled();
  });

  it('should call filtertopicslist when topic value is passed', () => {
    spyOn(component, 'filterTopicsList');
    component.filterTopicsList(mocktopics);
    expect(component.filterTopicsList).toHaveBeenCalled();
  });
});
