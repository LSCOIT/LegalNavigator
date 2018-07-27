import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { PersonalizedPlanComponent } from './personalized-plan.component';
import { PersonalizedPlanService } from './personalized-plan.service';
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RouterModule } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';
import { ArrayUtilityService } from '../../shared/array-utility.service';

describe('Component:PersonalizedPlan', () => {
  let component: PersonalizedPlanComponent;
  let fixture: ComponentFixture<PersonalizedPlanComponent>;
  let personalizedPlanService: PersonalizedPlanService;
  let mockactiveActionPlan = 'bd900039-2236-8c2c-8702-d31855c56b0f';
  let mockPlanDetailTags = { id: '773993', oId: 'rere', planTags: [{}], type: '' };
  let mockTopicList = [{
    topic: 'test',
    isSelected: false
  }]
  let mockTopicListBlank = [{
    topic: '',
    isSelected: false
  }]
  let mockTopics = [{ 'id': '3445', planTags: [{}] }];
  let mockTempTopicsList = [{
    topic: {
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
                    },
                    {
                      "id": "afabf032-72a8-4b04-81cb-c101bb1a0730"
                    },
                    {
                      "id": "3aa3a1be-8291-42b1-85c2-252f756febbc"
                    },
                    {
                      "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba"
                    },
                    {
                      "id": "bd900039-2236-8c2c-8702-d31855c56b0f"
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
                    },
                    {
                      "state": "Hawaii",
                      "city": "Honolulu"
                    },
                    {
                      "state": "Alaska"
                    },
                    {
                      "state": "New York",
                      "city": "New York"
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
                  "modifiedTimeStamp": "2018-04-01T04:18:00Z",
                  "_rid": "mwoSAJdNlwIGAAAAAAAAAA==",
                  "_self": "dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIGAAAAAAAAAA==/",
                  "_etag": "\"14002777-0000-0000-0000-5b3b215b0000\"",
                  "_attachments": "attachments/",
                  "_ts": 1530601819
                }
              }
            ]
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
            "resourceTags": []
          },
          "order": 2,
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
    }, isSelected: true
  }, {
    topic: {
      "topicId": "932abb0a-c6bb-46da-a3d8-5f52c2c914a0",
      "stepTags": [
        {
          "id": {
            "id": "2705d544-6af7-bd69-4f19-a1b53e346da2",
            "type": "steps",
            "title": "Take to your partner to see if you can come to an agreement",
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
                    },
                    {
                      "id": "afabf032-72a8-4b04-81cb-c101bb1a0730"
                    },
                    {
                      "id": "3aa3a1be-8291-42b1-85c2-252f756febbc"
                    },
                    {
                      "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba"
                    },
                    {
                      "id": "bd900039-2236-8c2c-8702-d31855c56b0f"
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
        },
        {
          "id": {
            "id": "3d64b676-cc4b-397d-a5bb-f4a0ea6d3040",
            "type": "steps",
            "title": "Submit a complaint for custody form",
            "description": "Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo.",
            "resourceTags": [
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
          "order": 2,
          "markCompleted": false
        }
      ],
      "id": [
        {
          "id": "932abb0a-c6bb-46da-a3d8-5f52c2c914a0",
          "name": "DivorceWithChildren",
          "parentTopicID": "f102bfae-362d-4659-aaef-956c391f79de",
          "resourceType": "Topics",
          "keywords": "CHILD ABUSE|CHILD CUSTODY",
          "location": [
            {
              "state": "Hawaii",
              "city": "Kalawao",
              "zipCode": "96742"
            },
            {
              "zipCode": "96741"
            },
            {
              "state": "Hawaii",
              "city": "Honolulu"
            },
            {
              "state": "Hawaii",
              "city": "Hawaiian Beaches"
            },
            {
              "state": "Hawaii",
              "city": "Haiku-Pauwela"
            },
            {
              "state": "Alaska"
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
    }, isSelected: true }];
  let mockTopic = "Family";

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([
          { path: 'plan /: id', component: PersonalizedPlanComponent }
        ]),
        HttpClientModule],
      declarations: [PersonalizedPlanComponent],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        PersonalizedPlanService, ArrayUtilityService
      ]
    })
      .compileComponents();
  }));
  beforeEach(() => {
    fixture = TestBed.createComponent(PersonalizedPlanComponent);
    component = fixture.componentInstance;
    personalizedPlanService = TestBed.get(PersonalizedPlanService);
    fixture.detectChanges();
  });

  it('should call getTopics method on ngOnInit', () => {
    spyOn(component, 'getTopics');
    component.ngOnInit();
    expect(component.getTopics).toHaveBeenCalled();
  });

  it('should create personalized plan component', () => {
    expect(component).toBeTruthy();
  });

  it('should define personalized plan component', () => {
    expect(component).toBeDefined();
  });

  it('should call getActionPlanConditions service method when  getTopics is called', () => {
    spyOn(component, 'getTopics');
    spyOn(component, 'getPlanDetails');
    spyOn(personalizedPlanService, 'getActionPlanConditions');
    component.activeActionPlan = mockactiveActionPlan;
    component.getTopics();
    component.getPlanDetails();
    personalizedPlanService.getActionPlanConditions(mockactiveActionPlan);
    expect(personalizedPlanService.getActionPlanConditions).toHaveBeenCalled();
    expect(component.getPlanDetails).toHaveBeenCalled();

  });

  it('should call createTopicsList service method when getplandetails method called', () => {
    spyOn(component, 'getPlanDetails');
    spyOn(personalizedPlanService, 'createTopicsList');
    spyOn(personalizedPlanService, 'displayPlanDetails');
    component.topics = mockTopics;
    component.planDetailTags = mockPlanDetailTags;
    component.topicsList = mockTopicList;
    component.getPlanDetails();
    personalizedPlanService.createTopicsList(component.topics);
    personalizedPlanService.displayPlanDetails(mockPlanDetailTags, mockTopicList);
    expect(personalizedPlanService.createTopicsList).toHaveBeenCalled();
    expect(personalizedPlanService.displayPlanDetails).toHaveBeenCalled();
  });

  it('should call displayPlanDetails service method when getplandetails method called', () => {
    spyOn(component, 'getPlanDetails');
    spyOn(personalizedPlanService, 'displayPlanDetails');
    component.planDetailTags = mockPlanDetailTags;
    component.topicsList = mockTopicList;
    component.getPlanDetails();
    personalizedPlanService.displayPlanDetails(mockPlanDetailTags, mockTopicList);
    expect(personalizedPlanService.displayPlanDetails).toHaveBeenCalled();
  });

  it('should call filterPlan  method when input is topic', () => {
    spyOn(component, 'filterTopicsList');
    spyOn(personalizedPlanService, 'displayPlanDetails');
    component.tempTopicsList = mockTopicList;
    component.filterTopicsList(mockTopicList);
    component.filterPlan(mockTopicList);
    personalizedPlanService.displayPlanDetails(mockPlanDetailTags, mockTopicList);
    expect(personalizedPlanService.displayPlanDetails).toHaveBeenCalled();
  });

  it('should call filterTopicsList  method when input is blank in topic', () => {
   spyOn(component, 'filterTopicsList');
    component.filterTopicsList(mockTopicListBlank);
    expect(component.topicsList.values)
  });

  it("should push items into topiclist when filterTopicsList is called", () => {
    component.tempTopicsList = mockTempTopicsList;
    component.filterTopicsList(mockTopic);
    expect(component.topicsList.length).toEqual(2);
  });
 
});
