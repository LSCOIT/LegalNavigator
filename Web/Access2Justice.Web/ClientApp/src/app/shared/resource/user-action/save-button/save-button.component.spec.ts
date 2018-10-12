import { ActivatedRoute, Router } from '@angular/router';
import { ArrayUtilityService } from '../../../array-utility.service';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BsModalService } from 'ngx-bootstrap';
import { EventUtilityService } from '../../../event-utility.service';
import { Global } from '../../../../global';
import { HttpClientModule } from '@angular/common/http';
import { PersonalizedPlanComponent } from '../../../../guided-assistant/personalized-plan/personalized-plan.component';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { ProfileComponent } from '../../../../profile/profile.component';
import { SaveButtonComponent } from './save-button.component';
import { ToastrService } from 'ngx-toastr';
import { MsalService } from '@azure/msal-angular';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../../../../environments/environment';
import { Mock } from 'protractor/built/driverProviders';
fdescribe('SaveButtonComponent', () => {
  let component: SaveButtonComponent;
  let fixture: ComponentFixture<SaveButtonComponent>;
  let mockToastr;
  let mockGlobal;
  let mockRouter;
  let msalService;
  let loginService;
  let mockPersonalizedPlanService;
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
  let mockPersonalizedPlanSteps = [{ stepId: '', title: '', description: '', order: 1, isComplete: false, resources: [], topicIds: [] },
  {
    stepId: '', title: '', description: '', order: 2, isComplete: false, resources: ['d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef'], topicIds: ['d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef']
  }];
  let mockPlanSteps = { stepId: 'testStepId', title: 'testStepTitle', description: 'testdescription', order: 1, isComplete: false, resources: ['d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef'], topicIds: ['d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef'] };
  let mockUserData = {
    displayableId: "mockUser",
    idToken: {
      name: "mockUser",
      preferred_username: "mockUser@microsoft.com"
    },
    identityProvider: "https://login.microsoftonline.com/123test",
    name: "mockUser",
    userIdentifier: "1234567890ABC"
  }
  let mockUserDataEmpty = null;
  let mockPlanId = '1fb1b006-a8bc-487d-98a0-2457d9d9f78d';
  let mockSavedResource = {
    "itemId": "1d7fc811-dabe-4468-a179-c43435bd22b6",
    "resourceType": "Articles",
    "resourceDetails": {}
  };
  beforeEach(async(() => {
    mockPersonalizedPlanService = jasmine.createSpyObj(['getActionPlanConditions', 'saveResourcesToProfile']);
    msalService = jasmine.createSpyObj(['getUser']);

    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [SaveButtonComponent],
      providers: [
        PersonalizedPlanService,
        ArrayUtilityService,
        ProfileComponent,
        EventUtilityService,
        PersonalizedPlanComponent,
        BsModalService,
        { provide: ToastrService, useValue: mockToastr },
        { provide: Global, useValue: mockGlobal },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { params: { 'id': '123' } } }
        },
        { provide: Router, useValue: mockRouter },
        { provide: MsalService, useValue: msalService },
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaveButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

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

    spyOn(sessionStorage, 'getItem')
      .and.callFake(mockSessionStorage.getItem);
    spyOn(sessionStorage, 'setItem')
      .and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, 'removeItem')
      .and.callFake(mockSessionStorage.removeItem);
    spyOn(sessionStorage, 'clear')
      .and.callFake(mockSessionStorage.clear);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should define', () => {
    expect(component).toBeDefined();
  });

  it('should get no topics for the given plan', () => {
    spyOn(component, 'getPersonalizedPlanSteps');
    component.getPlan(mockPlanId);
    mockPersonalizedPlanService.getActionPlanConditions.and.callFake(() => {
      return Observable.from([mockPlanDetails]);
    });
    expect(component.planTopic.topicId).toEqual('');
  });

  it('should get called PlanResources post login', () => {
    spyOn(component, 'getPersonalizedPlanSteps');
    msalService.getUser.and.returnValue(mockUserData);
    component.type = 'Plan'
    component.id = mockPlanId;
    component.savePlanResources();
    expect(component.planId).toEqual(mockPlanId);
  });

  it('should get called PlanResources post login have been called', () => {
    spyOn(component, 'savePlanResourcesPostLogin');
    msalService.getUser.and.returnValue(mockUserData);
    component.savePlanResources();
    expect(component.savePlanResourcesPostLogin).toHaveBeenCalled();
  });

  it('should get called PlanResources pre login', () => {
    spyOn(component, 'savePlanResourcesPreLogin');
    spyOn(component, 'externalLogin');
    msalService.getUser.and.returnValue(mockUserDataEmpty);
    component.savePlanResources();
    expect(component.savePlanResourcesPreLogin).toHaveBeenCalled();
  });

  it('should set plan session key if plan type exists - savePlanResourcesPreLogin', () => {
    component.type = 'Plan'
    component.id = mockPlanId;
    component.savePlanResourcesPreLogin();
    expect(JSON.parse(sessionStorage.getItem("bookmarkPlanId"))).toEqual(mockPlanId);
  });

  it('should set plan session key if plan type not exists - savePlanResourcesPreLogin', () => {
    component.type = 'NoPlan'
    component.id = mockPlanId;
    component.savePlanResourcesPreLogin();
    expect(component.savedResources.itemId).toEqual(mockPlanId);
    expect(component.savedResources.resourceType).toEqual('NoPlan');
  });

  it('should get topic ids from plan or topics', () => {
    component.getTopicIds(mockPlanDetails.topics);
    expect(component.topicIds).toContain('d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef');
  });

 });
