import { ActivatedRoute, Router } from '@angular/router';
import { ArrayUtilityService } from '../../../array-utility.service';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BsModalService } from 'ngx-bootstrap';
import { EventUtilityService } from '../../../event-utility.service';
import { Global } from '../../../../global';
import { HttpClientModule } from '@angular/common/http';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { SaveButtonComponent } from './save-button.component';
import { ToastrService } from 'ngx-toastr';
import { MsalService } from '@azure/msal-angular';
import { of } from 'rxjs/observable/of';
import { SaveButtonService } from './save-button.service';
import { NavigateDataService } from '../../../navigate-data.service';
import { Observable } from 'rxjs/Observable';
import { expand } from 'rxjs/operators/expand';

describe('SaveButtonComponent', () => {
  let component: SaveButtonComponent;
  let fixture: ComponentFixture<SaveButtonComponent>;
  let mockGlobal;
  let mockRouter;
  let msalService;
  let mockPersonalizedPlanService;
  let mockSaveButtonService;
  let mockMavigateDataService;
  let mockUserId = "testuserid";
  let mockPlanDetails = {
    "id": "29250697-8d22-4f9d-bbf8-96c1b5b72e54",
    "isShared": false,
    "topics": [
      {
        "topicId": "69be8e7c-975b-43c8-9af3-f61887a33ad3",
        "name": "Protective Order",
        "icon": null,
        "additionalReadings": [
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
        "additionalReadings": [
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
  let mockSavedResources = {
    "itemId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
    "resourceType": "Topics",
    "resourceDetails": {}
  };

  beforeEach(async(() => {
    mockPersonalizedPlanService = jasmine.createSpyObj(['getActionPlanConditions', 'saveResourceToProfilePostLogin', 'userPlan', 'showSuccess', 'saveBookmarkedResource']);
    msalService = jasmine.createSpyObj(['getUser']);
    mockPersonalizedPlanService.getActionPlanConditions.and.returnValue(of(mockPlanDetails));
    msalService.getUser.and.returnValue(mockUserData);
    mockMavigateDataService = jasmine.createSpyObj(['getData']);
    mockMavigateDataService.getData.and.returnValue(JSON.stringify(mockPlanDetails));
    mockSaveButtonService = jasmine.createSpyObj(['savePlanToUserProfile']);

    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [SaveButtonComponent],
      providers: [
        { provide: PersonalizedPlanService, useValue: mockPersonalizedPlanService },
        { provide: Global, useValue: { mockGlobal, userid: mockUserId, planSessionKey: "planKey" } },
        { provide: Router, useValue: { url: '/plan/id' }},
        { provide: MsalService, useValue: msalService },
        { provide: SaveButtonService, useValue: mockSaveButtonService },
        { provide: NavigateDataService, useValue: mockMavigateDataService },
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

  it('should get called PlanResources post login have been called', () => {
    spyOn(component, 'savePlanResourcesPostLogin');
    component.savePlanResources();
    expect(component.savePlanResourcesPostLogin).toHaveBeenCalled();
  });

  it('should get called PlanResources pre login', () => {
    spyOn(component, 'savePlanResourcesPreLogin');
    msalService.getUser.and.returnValue(null);
    component.savePlanResources();
    expect(component.savePlanResourcesPreLogin).toHaveBeenCalled();
  });

  it('should set plan session key if plan type exists - savePlanResourcesPreLogin', () => {
    const router = TestBed.get(Router);
    router.url = '/plan/id';
    spyOn(component, 'savePersonalizationPlan');
    component.savePlanResourcesPreLogin();
    expect(sessionStorage.setItem).toHaveBeenCalled();
    expect(component.savePersonalizationPlan).toHaveBeenCalled();
  });

  it('should set plan session key if plan type not exists - savePlanResourcesPreLogin', () => {
    const router = TestBed.get(Router);
    router.url = '/noPlan/id';
    component.id = mockSavedResources.itemId;
    component.type = mockSavedResources.resourceType;
    component.resourceDetails = mockSavedResources.resourceDetails;
    component.savePlanResourcesPreLogin();
    expect(component.savedResources).toEqual(mockSavedResources);
    expect(mockPersonalizedPlanService.saveBookmarkedResource).toHaveBeenCalledWith(mockSavedResources);
  });

  it('should call userPlan service in savePersonalizationPlan method', () => {
    mockPersonalizedPlanService.userPlan.and.returnValue(of(mockPlanDetails));
    component.savePersonalizationPlan();
    expect(mockPersonalizedPlanService.userPlan).toHaveBeenCalled();
    expect(mockPersonalizedPlanService.showSuccess).toHaveBeenCalled();
  });

  it('should call savePlanPostLogin - savePlanResourcesPostLogin', () => {
    const router = TestBed.get(Router);
    router.url = '/plan/id';
    spyOn(component, 'savePlanPostLogin');
    component.savePlanResourcesPostLogin();
    expect(component.savePlanPostLogin).toHaveBeenCalled();
  });

  it('should call saveResourcesPostLogin - savePlanResourcesPostLogin', () => {
    const router = TestBed.get(Router);
    router.url = '/noPlan/id';
    spyOn(component, 'saveResourcesPostLogin');
    component.savePlanResourcesPostLogin();
    expect(component.saveResourcesPostLogin).toHaveBeenCalled();
  });

  it('should call savePlanToUserProfile service in savePlanPostLogin', () => {
    component.savePlanPostLogin();
    expect(mockSaveButtonService.savePlanToUserProfile).toHaveBeenCalled();
  });

  it('should call getActionPlanConditions, savePlanToUserProfile service in savePlanPostLogin', () => {
    mockMavigateDataService.getData.and.returnValue(null);
    component.savePlanPostLogin();
    expect(mockPersonalizedPlanService.getActionPlanConditions).toHaveBeenCalled();
    expect(mockSaveButtonService.savePlanToUserProfile).toHaveBeenCalled();
  });

  it('should call savePlanToUserProfile service in saveResourcesPostLogin', () => {
    component.id = mockSavedResources.itemId;
    component.type = mockSavedResources.resourceType;
    component.resourceDetails = mockSavedResources.resourceDetails;
    component.saveResourcesPostLogin();
    expect(component.tempResourceStorage).toEqual([mockSavedResources]);
    expect(mockPersonalizedPlanService.saveResourceToProfilePostLogin).toHaveBeenCalled();
  });

});
