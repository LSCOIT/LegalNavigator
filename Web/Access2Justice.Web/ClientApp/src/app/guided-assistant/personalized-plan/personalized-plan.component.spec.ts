import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { PersonalizedPlanComponent } from './personalized-plan.component';
import { PersonalizedPlanService } from './personalized-plan.service';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';
import { ArrayUtilityService } from '../../shared/array-utility.service';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { Observable } from 'rxjs/Observable';

describe('Component:PersonalizedPlan', () => {
  let component: PersonalizedPlanComponent;
  let fixture: ComponentFixture<PersonalizedPlanComponent>;
  let personalizedPlanService: PersonalizedPlanService;
  let toastrService: ToastrService;
  let mockactiveActionPlan = '3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805';
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
  let mockTopicsList = [
    {
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
    },
    {
      "isSelected": true,
      "topic": {
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
    }];
  let mockFilteredPlanDetails = {
    "id": "3bd5b8cb-69f3-42fc-a74c-a9fa1c94f805",
    "topics": [
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
  let mockFilteredTopicsList = [
    {
      "isSelected": false,
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
    },
    {
      "isSelected": true,
      "topic": {
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
    }];
  let mockFilterTopicName = "Divorce";
  let mockTopicListBlank = [{
    topic: '',
    isSelected: false
  }];

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ToastrModule.forRoot(),
        RouterModule.forRoot([
          { path: 'plan /: id', component: PersonalizedPlanComponent }
        ]),
        HttpClientModule],
      declarations: [PersonalizedPlanComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        PersonalizedPlanService, ArrayUtilityService, ToastrService
      ]
    })
      .compileComponents();

    fixture = TestBed.createComponent(PersonalizedPlanComponent);
    component = fixture.componentInstance;
    personalizedPlanService = TestBed.get(PersonalizedPlanService);
    toastrService = TestBed.get(ToastrService);
    fixture.detectChanges();
  }));

  it('should create personalized plan component', () => {
    expect(component).toBeTruthy();
  });

  it('should define personalized plan component', () => {
    expect(component).toBeDefined();
  });

  it('should call getTopics method on ngOnInit', () => {
    spyOn(component, 'getTopics');
    component.ngOnInit();
    expect(component.getTopics).toHaveBeenCalled();
  });

  it('should call methods of personalized service and assing component values when gettopics is called', () => {
    component.activeActionPlan = mockactiveActionPlan;
    spyOn(personalizedPlanService, 'getActionPlanConditions').and.callFake(() => {
      return Observable.from([mockPlanDetails]);
    });
    spyOn(personalizedPlanService, 'createTopicsList').and.returnValue(mockTopicsList);
    spyOn(personalizedPlanService, 'getPlanDetails').and.returnValue(mockPlanDetails);
    component.getTopics();
    expect(component.topics).toEqual(mockPlanDetails.topics);
    expect(component.planDetailTags).toEqual(mockPlanDetails);
    expect(personalizedPlanService.createTopicsList).toHaveBeenCalledWith(mockPlanDetails.topics);
    expect(personalizedPlanService.getPlanDetails).toHaveBeenCalledWith(mockPlanDetails.topics, mockPlanDetails);
    expect(component.topicsList).toEqual(mockTopicsList);
    expect(component.planDetails).toEqual(mockPlanDetails);
  });

  it('should call filterTopicsList method and  personalizedPlanService displayPlanDetails method when filterPlan is called', () => {
    component.topicsList = mockFilteredTopicsList;
    component.planDetailTags = mockPlanDetails;
    spyOn(component, 'filterTopicsList');
    spyOn(personalizedPlanService, 'displayPlanDetails').and.returnValue(mockFilteredPlanDetails);
    component.filterPlan(mockFilterTopicName);
    expect(component.filterTopicsList).toHaveBeenCalledWith(mockFilterTopicName);
    expect(personalizedPlanService.displayPlanDetails).toHaveBeenCalledWith(mockPlanDetails, mockFilteredTopicsList);
    expect(component.planDetails).toEqual(mockFilteredPlanDetails);
  });

  it('should return filtered topics list based on the input topic and tempTopicsList in filterTopicsList method', () => {
    component.tempTopicsList = mockTopicsList;
    component.filterTopicsList(mockFilterTopicName);
    expect(component.topicsList).toEqual(mockFilteredTopicsList);
  });

  it('should return filtered topics list infilterTopicsList  method when input is blank in topic', () => {
    component.tempTopicsList = mockTopicsList;
    component.filterTopicsList(mockTopicListBlank);
    expect(component.topicsList).toEqual(mockTopicsList);
  });

  it('should return filtered topics list infilterTopicsList  method when input topic is hidden', () => {
    component.tempTopicsList = mockFilteredTopicsList;
    component.filterTopicsList(mockFilterTopicName);
    expect(component.topicsList).toEqual(mockTopicsList);
  });

  it('should assign component values in filterTopics method', () => {
    let mockEvent = { "plan": mockPlanDetails, "topicsList": mockTopicsList };
    spyOn(component,'filterPlan');
    component.filterTopics(mockEvent);
    expect(component.topics).toEqual(mockEvent.plan.topics);
    expect(component.planDetailTags).toEqual(mockEvent.plan);
    expect(component.topicsList).toEqual(mockEvent.topicsList);
    expect(component.filterPlan).toHaveBeenCalledWith("");
  });

});
