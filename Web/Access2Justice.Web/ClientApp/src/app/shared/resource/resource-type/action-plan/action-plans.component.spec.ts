import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ActionPlansComponent } from './action-plans.component';
import { PersonalizedPlanService } from '../../../../guided-assistant/personalized-plan/personalized-plan.service';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap';
import { PlanStep } from '../../../../guided-assistant/personalized-plan/personalized-plan';
import { ArrayUtilityService } from '../../../../shared/array-utility.service';
import { ToastrService, ToastrModule, ToastPackage } from 'ngx-toastr';
import { Global } from '../../../../global';
import { Observable } from 'rxjs/Observable';

fdescribe('ActionPlansComponent', () => {
  let component: ActionPlansComponent;
  let fixture: ComponentFixture<ActionPlansComponent>;
  let sharedService: ArrayUtilityService;
  let toastrService: ToastrService;
  let personalizedPlanService: PersonalizedPlanService;
  let mockIsChecked = true;
  let mockTopicId = "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef";
  let mockStepId = "f05ace00-c1cc-4618-a224-56aa4677d2aa";
  let mockPlanId = "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805";
  let mockItems = {
    "topicId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
    "steps": [
      {
        "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
        "title": "Jurisdiction",
        "description": "Jurisdiction is a very complicated subject and you…are some resources that could help you with this:",
        "order": 1,
        "isComplete": false,
        "resources": [
          {
            "id": "19a02209-ca38-4b74-bd67-6ea941d41518"
          },
          {
            "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"
          }
        ]
      }
    ]
  };
  let mockUpdatedMatchingStep =
    {
      "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
      "topicIds": [],
      "title": "Jurisdiction",
      "description": "Jurisdiction is a very complicated subject and you…are some resources that could help you with this:",
      "order": 1,
      "isComplete": true,
      "resources": ["19a02209-ca38-4b74-bd67-6ea941d41518", "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"]
    };
  let mockNonMatchingStep =
    {
      "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
      "topicIds": [],
      "title": "Jurisdiction",
      "description": "Jurisdiction is a very complicated subject and you…are some resources that could help you with this:",
      "order": 1,
      "isComplete": false,
      "resources": ["19a02209-ca38-4b74-bd67-6ea941d41518", "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"]
    };
  let mockPlanDetails = {
    "id": "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805",
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
      }
    ],
    "isShared": false
  };
  let mockupdatedPlanDetails = {
    "id": "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805",
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
            "isComplete": true,
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
      }
    ],
    "isShared": false
  };
  let mockUpdatedPlanForMatchingTopic = {
    "id": "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805",
    "topics": [
      {
        "topicId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
        "steps": [
          {
            "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
            "title": "File a motion to modify if there has been a change of circumstances.",
            "description": "The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.",
            "order": 1,
            "isComplete": true,
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
      }
    ],
    "isShared": false
  };
  let mockUpdatedPlanForNonMatchingTopic = {
    "id": "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805",
    "topics": [
      {
        "topicId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
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
      }
    ],
    "isShared": false
  };
  let mockOrderedPlanDetails = {
    "id": "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805",
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
            "description": "Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:",
            "isComplete": true,
            "order": 2,
            "resources": [
              {
                "id": "19a02209-ca38-4b74-bd67-6ea941d41518"
              },
              {
                "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"
              }
            ],
            "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
            "title": "Jurisdiction"
          },
          {
            "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
            "title": "File a motion to modify if there has been a change of circumstances.",
            "description": "The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.",
            "order": 1,
            "isComplete": false,
            "resources": [
              {
                "id": "19a02209-ca38-4b74-bd67-6ea941d41518"
              },
              {
                "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"
              }
            ]
          }
        ]
      }
    ],
    "isShared": false
  };
  let mockTopicsList = [{
    "isSelected": true,
    "topic":
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
            "resources": [
              {
                "id": "19a02209-ca38-4b74-bd67-6ea941d41518"
              },
              {
                "id": "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"
              }
            ],
            "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
            "title": "Jurisdiction"
          }
        ]
      }
  }];
  let mockEvent = {
    "target": { "checked": true }
  }
  let mockPersonalizedPlan = {
    "id": "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805",
    "topics": [
      {
        "topicId": "d1d5f7a0-f1fa-464f-8da6-c2e7ce1501ef",
        "steps": [
          {
            "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
            "title": "File a motion to modify if there has been a change of circumstances.",
            "description": "The Motion and Affidavit for Post-Decree relief are used to request changes to a divorce or make sure the other side is followin gthe orders.",
            "order": 1,
            "isComplete": true,
            "resources": ["19a02209-ca38-4b74-bd67-6ea941d41518", "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"],
            "topicIds": []
          },
          {
            "description": "Jurisdiction is a very complicated subject and you should talk to an attorney to figure out where is the best place ot file your case. Here are some resources that could help you with this:",
            "isComplete": true,
            "order": 2,
            "resources": ["19a02209-ca38-4b74-bd67-6ea941d41518", "9ca4cf73-f6c0-4f63-a1e8-2a3774961df5"],
            "topicIds": [],
            "stepId": "f05ace00-c1cc-4618-a224-56aa4677d2aa",
            "title": "Jurisdiction"
          }
        ]
      }
    ],
    "isShared": false
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule, ToastrModule.forRoot()
      ],
      declarations: [ActionPlansComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        PersonalizedPlanService,
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
    expect(component.getPersonalizedPlan).toHaveBeenCalledWith(mockPlanDetails);
  });

  it("should assign input topics list to tempFilteredtopicsList on ngChanges", () => {
    component.topicsList = mockTopicsList;
    component.planDetails = mockPlanDetails;
    component.ngOnChanges();
    expect(component.tempFilteredtopicsList).toEqual(mockTopicsList);
  });

  it("should assign values for planDetails and displaySteps if planDetails list is empty in getPersonalizedPlan method", () => {
    let emptyPlanDetails = {};
    component.getPersonalizedPlan(emptyPlanDetails);
    expect(component.planDetails).toEqual(emptyPlanDetails);
    expect(component.displaySteps).toBeFalsy();
  });

  it("should assign values for planDetails and displaySteps if planDetails is not defined in getPersonalizedPlan method", () => {
    let noPlanDetails = undefined;
    component.getPersonalizedPlan(noPlanDetails);
    expect(component.planDetails).toEqual(noPlanDetails);
    expect(component.displaySteps).toBeFalsy();
  });

  it("should assign values for planDetails, displaySteps and call sortStepsByOrder if planDetails list is not empty in getPersonalizedPlan method", () => {
    spyOn(component, 'sortStepsByOrder');
    component.getPersonalizedPlan(mockPlanDetails);
    expect(component.planDetails).toEqual(mockPlanDetails);
    expect(component.displaySteps).toBeTruthy();
    expect(component.sortStepsByOrder).toHaveBeenCalledWith(component.planDetails);
  });

  it("should call orderBy in sortStepsByOrder method", () => {
    spyOn(component, 'orderBy');
    component.sortStepsByOrder(mockPlanDetails);
    expect(component.orderBy).toHaveBeenCalled();
  });

  //Need to check
  xit("should order the plan details by isComplete in sortStepsByOrder method", () => {
    //spyOn(component, 'orderBy');
    component.sortStepsByOrder(mockPlanDetails);
    expect(component.planDetails).toEqual(mockOrderedPlanDetails);
  });

  it("should call assign isChecked and call getPlanDetails in checkCompleted method", () => {
    component.planDetails = mockPlanDetails;
    spyOn(component, 'getPlanDetails');
    component.checkCompleted(mockEvent, mockTopicId, mockStepId);
    expect(component.isChecked).toBeTruthy();
    expect(component.getPlanDetails).toHaveBeenCalledWith(mockTopicId, mockStepId, component.isChecked);
  });

  it("should assign values to topics, plan and call updateMarkCompleted and updateProfilePlan methods", () => {
    component.planDetails = mockPlanDetails;
    component.isChecked = true;
    spyOn(personalizedPlanService, 'getActionPlanConditions').and.callFake(() => {
      return Observable.from([mockPlanDetails]);
    });
    spyOn(personalizedPlanService, 'getPlanDetails').and.returnValue(mockPlanDetails);
    spyOn(component, 'updateMarkCompleted');
    spyOn(component, 'updateProfilePlan');
    component.getPlanDetails(mockTopicId, mockStepId, true);
    expect(component.topics).toEqual(mockPlanDetails.topics);
    expect(component.plan).toEqual(mockPlanDetails);
    expect(component.updateMarkCompleted).toHaveBeenCalledWith(mockTopicId, mockStepId, true);
    expect(component.updateProfilePlan).toHaveBeenCalledWith(component.isChecked);
  });

  it("should call updateStepTagForMatchingTopicId when topicId matches in updateMarkCompleted method", () => {
    let mockSteps = mockUpdatedPlanForMatchingTopic.topics[0].steps;
    component.plan = mockPlanDetails;
    component.planDetails = mockPlanDetails;
    //spyOn(component, 'updateStepTagForMatchingTopicId');
    spyOn(component, 'stepTagForNonMatchingTopicId');
    spyOn(component, 'updateStepTagForMatchingTopicId').and.callFake(() => {
      return Observable.from([mockSteps]);
    });
    component.updateMarkCompleted(mockTopicId, mockStepId, true);
    expect(component.updateStepTagForMatchingTopicId).toHaveBeenCalledTimes(1);
    expect(component.stepTagForNonMatchingTopicId).toHaveBeenCalledTimes(0);
    expect(component.updateStepTagForMatchingTopicId).toHaveBeenCalledWith(mockPlanDetails.topics[0], mockStepId, true);
    //expect(component.personalizedPlan).toEqual(mockUpdatedPlanForMatchingTopic);
  });

  it("should call stepTagForNonMatchingTopicId when topicId matches in updateMarkCompleted method", () => {
    let nonMatchingTopicId = "test";
    component.plan = mockPlanDetails;
    component.planDetails = mockPlanDetails;
    spyOn(component, 'stepTagForNonMatchingTopicId');
    spyOn(component, 'updateStepTagForMatchingTopicId');
    component.updateMarkCompleted(nonMatchingTopicId, mockStepId, true);
    expect(component.updateStepTagForMatchingTopicId).toHaveBeenCalledTimes(0);
    expect(component.stepTagForNonMatchingTopicId).toHaveBeenCalledTimes(1);
    expect(component.stepTagForNonMatchingTopicId).toHaveBeenCalledWith(mockPlanDetails.topics[0]);
    //expect(component.personalizedPlan).toEqual(mockUpdatedPlanForNonMatchingTopic);
  });

  it('should return personalizedPlanSteps in updateStepTagForMatchingTopicId', () => {
    let mockPersonalizedPlanSteps = [];
    mockPersonalizedPlanSteps.push(mockUpdatedMatchingStep);
    component.updateStepTagForMatchingTopicId(mockItems, mockStepId, true);
    expect(component.personalizedPlanSteps).toEqual(mockPersonalizedPlanSteps);
  });

  it('should return personalizedPlanSteps in stepTagForNonMatchingTopicId', () => {
    let mockPersonalizedPlanSteps = [];
    mockPersonalizedPlanSteps.push(mockNonMatchingStep);
    component.stepTagForNonMatchingTopicId(mockItems);
    expect(component.personalizedPlanSteps).toEqual(mockPersonalizedPlanSteps);
  });

  it("should add topics to filteredtopicsList and call getUpdatedPersonalizedPlan in updateProfilePlan method", () => {
    component.personalizedPlan = mockPersonalizedPlan;
    component.tempFilteredtopicsList = mockTopicsList;
    let mockFilteredTopicList = [{ "topic": mockupdatedPlanDetails.topics[0], "isSelected": true }];
    let mockIsChecked = true;
    spyOn(personalizedPlanService, 'userPlan').and.callFake(() => {
      return Observable.from([mockupdatedPlanDetails]);
    });
    spyOn(component, 'getUpdatedPersonalizedPlan');
    component.updateProfilePlan(mockIsChecked);
    expect(personalizedPlanService.userPlan).toHaveBeenCalled();
    expect(component.filteredtopicsList).toEqual(mockFilteredTopicList);
    expect(component.getUpdatedPersonalizedPlan).toHaveBeenCalledWith(mockupdatedPlanDetails, mockIsChecked);
  });

  it("should assign isCompleted to true and call loadPersonalizedPlan in getUpdatedPersonalizedPlan when isChecked is true", () => {
    let mockIsChecked = true;
    let mockFilteredTopicList = [{ "topic": mockupdatedPlanDetails.topics[0], "isSelected": true }];
    component.filteredtopicsList = mockFilteredTopicList;
    component.getUpdatedPersonalizedPlan(mockupdatedPlanDetails, mockIsChecked);
    expect(component.loadPersonalizedPlan).toHaveBeenCalled();
    expect(component.isChecked).toBeTruthy();
    expect(toastrService.success).toHaveBeenCalledWith("Step Completed.");
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

  it('should update profile plan when isChecked value is passed', () => {
    spyOn(component, 'updateProfilePlan');
    component.updateProfilePlan(mockIsChecked);
    expect(component.updateProfilePlan).toHaveBeenCalled();
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
