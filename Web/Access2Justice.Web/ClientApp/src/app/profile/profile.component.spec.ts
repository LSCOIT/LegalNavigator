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
import { Global } from '../global';
import { ToastrModule } from 'ngx-toastr';

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
  let mockGlobal;
  let mockPersonalizedPlanService;
  beforeEach(async(() => {
    mockGlobal = jasmine.createSpyObj(['externalLogin']);
    mockPersonalizedPlanService = jasmine.createSpyObj(['getActionPlanConditions', 'displayPlanDetails','getUserSavedResources']);

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
        { provide: Global, useValue: mockGlobal }
      ]
    }).compileComponents();

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
});
