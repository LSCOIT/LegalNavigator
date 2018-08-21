import { async, ComponentFixture, TestBed, ComponentFixtureNoNgZone } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ActionPlansComponent } from './action-plans.component';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap';
import { PlanTopic, PersonalizedPlan, PlanStep, PersonalizedPlanTopic } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { assertNotNull } from '@angular/compiler/src/output/output_ast';
import { Observable } from 'rxjs/Observable';
import { TemplateRef } from '@angular/core';
import { ArrayUtilityService } from '../../../../shared/array-utility.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { DomSanitizer } from '@angular/platform-browser';
import { Title } from '@angular/platform-browser/src/browser/title';
import { ToastrService, ToastrModule, ToastPackage } from 'ngx-toastr';
import { Global, UserStatus } from '../../../../global';
import { ComponentFactoryBoundToModule } from '@angular/core/src/linker/component_factory_resolver';

const mockMarkCompletedUpdatedPlan = {
  getMarkCompletedUpdatedPlan: () => { return Observable.of(); }
};

describe('ActionPlansComponent', () => {
  let component: ActionPlansComponent;
  let fixture: ComponentFixture<ActionPlansComponent>;
  let modalService: BsModalService;
  let sharedService: ArrayUtilityService;
  let toastrService: ToastrService;
  let personalizedPlanService: PersonalizedPlanService;
  let globalService: Global;
  let mockTemplate = "testTemplate";
  let mockIsChecked = true;
  let mockTopicId = "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef";
  let mockStepId = "f05ace00-c1cc-4618-a224-56aa4677d2aa";
  let mockPlanId = "89cb57af-3d3b-49a9-990a-aa5f8d89a796";
  let mockItems = {
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
        "type": "steps",
        "title": "Jurisdiction",
        "description": "Jurisdiction is a very complicated subject and you…are some resources that could help you with this:",
        "order": 2,
        "isComplete": false,
        "resources": [
          {
            "id": "19a02209-ca38-4b74-bd67-6ea941d41518"
          },
          {
            "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"
          }
        ]
      },
      {
        "description": "Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:",
        "isComplete": true,
        "order": 2,
        "resources":
          [
            {
              "id": "19a02209-ca38-4b74-bd67-6ea941d41518"
            },
            {
              "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"
            }
          ],
        "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
        "title": "Jurisdiction",
        "type": "steps"
      }
    ]
  };
  let mockPlanDetails = {};
  let mockPlanDetailsJson = {
    "id": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
    "oId": "User Id",
    "type": "plans",
    "planTags": [
      {
        "topicId": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
        "stepTags": [
          {
            "id": {
              "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
              "type": "steps",
              "title": "Make sure your summons is real.",
              "description": "Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo.",
              "resourceTags": [
                {
                  "id": {
                    "id": "9fc75d90-7ffa-4c26-9cb7-ba271f2007ad",
                    "name": "Lorem ipsum dolor sit amet",
                    "description": "Subhead lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem",
                    "resourceType": "Videos",
                    "externalUrl": "",
                    "url": "https://channel9.msdn.com/Shows/Azure-Friday/Managing-costs-with-the-Azure-Budgets-API-and-Action-Groups/player",
                    "topicTags": [
                      {
                        "id": "f102bfae-362d-4659-aaef-956c391f79de"
                      }
                    ],
                    "location": [
                      {
                        "state": "Hawaii",
                        "city": "Kalawao",
                        "zipCode": "96742"
                      },
                      {
                        "zipCode": "96741"
                      }
                    ],
                    "icon": "./assets/images/resources/resource.png",
                    "overview": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.",
                    "isRecommended": "",
                    "createdBy": "",
                    "createdTimeStamp": "",
                    "modifiedBy": "",
                    "modifiedTimeStamp": "2018-05-01T04:18:00Z"
                  }
                },
                {
                  "id": {
                    "id": "19a02209-ca38-4b74-bd67-6ea941d41518",
                    "name": "Organization Name 1",
                    "type": "Housing Law Services",
                    "description": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.",
                    "resourceType": "Organizations",
                    "externalUrl": "",
                    "url": "websiteurl.com",
                    "topicTags": [
                      {
                        "id": "afabf032-72a8-4b04-81cb-c101bb1a0730"
                      },
                      {
                        "id": "3aa3a1be-8291-42b1-85c2-252f756febbc"
                      }
                    ],
                    "location": [
                      {
                        "zipCode": "96741"
                      },
                      {
                        "state": "Hawaii",
                        "city": "Haiku-Pauwela"
                      },
                      {
                        "state": "Alaska"
                      }
                    ],
                    "icon": "./assets/images/resources/resource.png",
                    "address": "Honolulu, Hawaii 96813, United States",
                    "telephone": "000-000-0000",
                    "overview": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.",
                    "eligibilityInformation": "Copy describing eligibility qualification lorem ipsum dolor sit amet. ",
                    "reviewedByCommunityMember": "Quote from community member consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar tempor. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.",
                    "reviewerFullName": "",
                    "reviewerTitle": "",
                    "reviewerImage": "",
                    "createdBy": "",
                    "createdTimeStamp": "",
                    "modifiedBy": "",
                    "modifiedTimeStamp": "2018-04-01T04:18:00Z"
                  }
                }
              ]
            },
            "order": 1,
            "markCompleted": false
          }
        ],
        "id": [
          {
            "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
            "name": "Family",
            "parentTopicID": "",
            "resourceType": "Topics",
            "keywords": "EVICTION",
            "location": [
              {
                "state": "Hawaii",
                "county": "Kalawao County",
                "city": "Kalawao",
                "zipCode": "96742"
              },
              {
                "zipCode": "96741"
              }
            ],
            "jsonContent": "",
            "icon": "./assets/images/topics/topic14.png",
            "createdBy": "",
            "createdTimeStamp": "",
            "modifiedBy": "",
            "modifiedTimeStamp": ""
          }
        ]
      }

    ]
  };
  let mockPlanDetailsArray = [mockPlanDetailsJson];
  let mockPlanSteps = [{
    "stepTags": [
      {
        "id": {
          "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
          "type": "steps",
          "title": "Make sure your summons is real.",
          "description": "Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo.",
        },
        "order": 1,
        "markCompleted": false
      },
      {
        "id": {
          "id": "d46aecee-8c79-df1b-4081-1ea02b5022df",
          "type": "steps",
          "title": "Try to resolve the issue with your landlord to see if you can come to an agreement",
          "description": "Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo.",
        },
        "order": 2,
        "markCompleted": false
      }
    ],
    "topicId": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
    "topicName": "Family"
  },
  {
    "stepTags": [
      {
        "id": {
          "id": "2705d544-6af7-bd69-4f19-a1b53e346da2",
          "type": "steps",
          "title": "Take to your partner to see if you can come to an agreement",
          "description": "Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo.",
        },
        "order": 1,
        "markCompleted": false
      },
      {
        "id": {
          "id": "3d64b676-cc4b-397d-a5bb-f4a0ea6d3040",
          "type": "steps",
          "title": "Submit a complaint for custody form",
          "description": "Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo.",
        },
        "order": 2,
        "markCompleted": false
      }
    ],
    "topicId": "932abb0a-c6bb-46da-a3d8-5f52c2c914a0",
    "topicName": "DivorceWithChildren"
  }];
  let template: TemplateRef<any>;
  let mockplanSteps: Array<PlanStep>;
  let mockplanStep: PlanStep;
  let mockdisplaySteps: boolean = false
  let mockupdatePlan: PlanTopic;
  let mockPlanTags = "testPlanTags";
  let mockEventChecked = {
    "target": { "checked": true }
  }
 
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule, ModalModule.forRoot(), ToastrModule.forRoot()
      ],
      declarations: [ActionPlansComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        PersonalizedPlanService,
        BsModalService,
        ArrayUtilityService,
        ToastrService,
        Global]
    })
      .compileComponents();
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(ActionPlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    personalizedPlanService = TestBed.get(PersonalizedPlanService);
    modalService = TestBed.get(BsModalService);
    sharedService = TestBed.get(ArrayUtilityService);
    toastrService = TestBed.get(ToastrService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });

  it("should call getPersonalizedPlan on ngChanges", () => {
    spyOn(component, 'getPersonalizedPlan');
    component.ngOnChanges();
    component.getPersonalizedPlan(mockPlanDetails);
    expect(component.getPersonalizedPlan).toHaveBeenCalled();
  });

  it("should assign values for planSteps and displaySteps if planDetails list is empty in getPersonalizedPlan method", () => {
    spyOn(component, 'getPersonalizedPlan');
    component.getPersonalizedPlan(mockPlanDetails);
    expect(component.planDetails).toEqual(mockplanStep);
    expect(component.displaySteps).toEqual(mockdisplaySteps);
  });

  it("should assign the array json value to the input plan Details in getPersonalizedPlan method", () => {
    spyOn(component, 'getPersonalizedPlan');
    component.planDetails = mockPlanDetailsArray;
    component.getPersonalizedPlan(component.planDetails);
    expect(component.getPersonalizedPlan).toHaveBeenCalled();
  });

  it("should assign values for planSteps and displaySteps in getPersonalizedPlan method", () => {
    spyOn(component, 'getPersonalizedPlan');
    component.getPersonalizedPlan(mockPlanDetailsJson);
    expect(component.planDetails).not.toBeNull();
    expect(component.displaySteps).not.toBe(true);
  });

  it("should call orderBy method in getPersonalizedPlan", () => {
    spyOn(component, 'sortStepsByOrder')
    component.sortStepsByOrder(mockPlanDetails);
    expect(component.sortStepsByOrder).toHaveBeenCalled();
  });

  it('should call checkCompleted', () => {
    spyOn(component, 'checkCompleted');
    expect(component.checkCompleted).toHaveBeenCalled;
  });

  it('should call getPlanDetails method when checkCompleted called with values', () => {
    spyOn(component, 'getPlanDetails');
    component.getPlanDetails(mockTopicId, mockStepId, mockIsChecked)
    component.checkCompleted;
    expect(component.getPlanDetails).toHaveBeenCalled;
  });

  it("should call getActionPlanConditions when getPlanDetails method called ", () => {
    spyOn(personalizedPlanService, 'getActionPlanConditions');
    personalizedPlanService.getActionPlanConditions(mockPlanId);
    expect(personalizedPlanService.getActionPlanConditions).toHaveBeenCalled();
  });

  it("should call updateMarkCompleted when getPlanDetails method called ", () => {
    spyOn(component, 'updateMarkCompleted');
    component.updateMarkCompleted(mockPlanId, mockStepId, mockIsChecked);
    expect(component.updateMarkCompleted).toHaveBeenCalled();
  });

  it("should call updateProfilePlan when getPlanDetails method called ", () => {
    spyOn(component, 'updateProfilePlan');
    component.updateProfilePlan(mockIsChecked);
    expect(component.updateProfilePlan).toHaveBeenCalled();
  });

  it("should return personalizedPlanSteps when updateMarkCompleted method of component", () => {
    spyOn(component, 'updateMarkCompleted');
    component.updateMarkCompleted(mockTopicId, mockStepId, mockIsChecked);
    expect(component.updateMarkCompleted).toHaveBeenCalled();
  });

  it('should return plan steps from  updateStepTagForMatchingTopicId when updateMarkCompleted method of component called ', () => {
    spyOn(component, 'updateStepTagForMatchingTopicId');
    component.updateStepTagForMatchingTopicId(mockItems, mockStepId, mockIsChecked);
    expect(component.updateStepTagForMatchingTopicId).toHaveBeenCalled();
  });

  it('should return plan steps from stepTagForNonMatchingTopicId when updateMarkCompleted method of component called', () => {
    spyOn(component, 'stepTagForNonMatchingTopicId');
    component.stepTagForNonMatchingTopicId(mockItems);
    expect(component.stepTagForNonMatchingTopicId).toHaveBeenCalled();
  });

  it('should call  getResourceIds service method when updateStepTagForMatchingTopicId method of component called', () => {
    spyOn(personalizedPlanService, 'getResourceIds');
    personalizedPlanService.getResourceIds(mockItems);
    expect(personalizedPlanService.getResourceIds).toHaveBeenCalled();
  });

  it('should update profile plan when isChecked value is passed',() => {
    spyOn(component, 'updateProfilePlan');
    component.updateProfilePlan(mockIsChecked);
    expect(component.updateProfilePlan).toHaveBeenCalled();
  });

  it('should call getUpdatedPersonalizedPlan method of component is called', () => {
    spyOn(component, 'getUpdatedPersonalizedPlan');
    component.getUpdatedPersonalizedPlan(mockPlanDetailsJson, mockIsChecked);
    expect(component.getUpdatedPersonalizedPlan).toHaveBeenCalled();
  });

  it('should call loadPersonalizedPlan method when getUpdatedPersonalizedPlan method of component is called', () => {
    spyOn(component, 'loadPersonalizedPlan');
    component.loadPersonalizedPlan();
    expect(component.loadPersonalizedPlan).toHaveBeenCalled();
  });

  it('should call planTagOptions method of component is called when topic id passed', () => {
    spyOn(component, 'planTagOptions');
    component.planTagOptions(mockTopicId);
    expect(component.planTagOptions).toHaveBeenCalled();
  });

  it('should call getRemovePlanDetails method of component is called', () => {
    spyOn(component, 'getRemovePlanDetails');
    component.getRemovePlanDetails();
    expect(component.getRemovePlanDetails).toHaveBeenCalled();
  });
});
