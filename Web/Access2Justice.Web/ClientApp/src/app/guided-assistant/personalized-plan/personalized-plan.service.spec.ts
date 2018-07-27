import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule, HttpHeaders } from '@angular/common/http';
import { PersonalizedPlanService } from './personalized-plan.service';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { api } from '../../../api/api';
import { Observable } from 'rxjs/Rx';
import { ServiceOrgService } from '../../shared/sidebars/service-org.service';
import { ArrayUtilityService } from '../../shared/array-utility.service';
import { catchError } from 'rxjs/operators/catchError';

describe(' Service:PersonalizedPlan', () => {
  const mockHttpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  let mockupdateplan = {
      "planId": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
      "oId": "User Id",
      "type": "plans",
      "planTags": [
        {
          "topicId": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
          "stepTags": [
            {
              "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
              "order": 1,
              "markCompleted": true
            },
            {
              "id": "d46aecee-8c79-df1b-4081-1ea02b5022df",
              "order": 2,
              "markCompleted": false
            }
          ]
        },
        {
          "topicId": "932abb0a-c6bb-46da-a3d8-5f52c2c914a0",
          "stepTags": [
            {
              "id": "2705d544-6af7-bd69-4f19-a1b53e346da2",
              "order": 1,
              "markCompleted": false
            },
            {
              "id": "3d64b676-cc4b-397d-a5bb-f4a0ea6d3040",
              "order": 2,
              "markCompleted": false
            }
          ]
        }
      ]
    };
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
      },
      {
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
      }
    ]
  }; 
  let mockPlanDetails = {
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
      }
    ]
  };
  let mockResourceInput = { 
      "ResourceType": "testtype",
      "ContinuationToken": "",
      "TopicIds": "",
      "ResourceIds": "",
      "PageNumber":1,
      "Location":"testlocation"
  };
  let mockProfileData ={
      "planId": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
      "oId": "User Id",
      "type": "plans",
      "planTags": [
        {
          "topicId": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
          "stepTags": [
            {
              "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
              "order": 1,
              "markCompleted": true
            },
            {
              "id": "d46aecee-8c79-df1b-4081-1ea02b5022df",
              "order": 2,
              "markCompleted": false
            }
          ]
        },
        {
          "topicId": "932abb0a-c6bb-46da-a3d8-5f52c2c914a0",
          "stepTags": [
            {
              "id": "2705d544-6af7-bd69-4f19-a1b53e346da2",
              "order": 1,
              "markCompleted": false
            },
            {
              "id": "3d64b676-cc4b-397d-a5bb-f4a0ea6d3040",
              "order": 2,
              "markCompleted": false
            }
          ]
        }
      ]
    };
  let mockPlanDetailTags = [{
    "id": "393994-133-33",
    "oId": "93004kdf033keke;",
    "planTags": [{}],
    "type": "testtype"
  }];

  let mockActiveRoute: ActivatedRoute;
  let resoureStorage = sessionStorage.getItem(this.mockSessionKey);
  let mockResponse = Observable.of(mockPlanDetailsJson);
  let profileStorage = sessionStorage.getItem(this.mockProfileData);
  
  let mockTopics = ["topic1", "topic2"];
  let mockTopicsList = [{ topic: "topic1", isSelected: true }, { topic: "topic2", isSelected: true }];
  let oid = "123456789";
  let mockPlanId = "993jfk3003j"
  let mockSessionKey = "883833";
  let mockObjects = false;
  let mockObject = false;
  let service: PersonalizedPlanService;
  let arrayUtilityService: ArrayUtilityService;
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  var originalTimeout;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientModule],
      providers: [PersonalizedPlanService, ArrayUtilityService]
    });
    service = new PersonalizedPlanService(httpSpy, arrayUtilityService);
    arrayUtilityService = new ArrayUtilityService();
    httpSpy.get.calls.reset();
  });
  beforeEach(function () {
    originalTimeout = jasmine.DEFAULT_TIMEOUT_INTERVAL;
    jasmine.DEFAULT_TIMEOUT_INTERVAL = 10000;
    it('should call getBookmarkedData when getPersonalizedPlan called', (done) => {
      spyOn(service, 'getBookmarkedData');
      service.topics = mockTopics;
      service.getBookmarkedData();
      expect(service.getBookmarkedData).toBeDefined();
    });
  });
  afterEach(function () {
    jasmine.DEFAULT_TIMEOUT_INTERVAL = originalTimeout;
  });
  it('personalized plan should be created', () => {
    expect(service).toBeTruthy();
  });
  it('personalized plan should be defined ', () => {
    expect(service).toBeDefined();
  });
  it('should retrieve plan steps when the id  value is id', (done) => {
    let mockid = "bf8d7e7e-2574-7b39-efc7-83cb94adae07";
    let mockplan = {
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
                "resourceTags": []
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
                },
                {
                  "state": "Hawaii",
                  "county": "Honolulu County",
                  "city": "Honolulu"
                }
              ],
              "jsonContent": "",
              "icon": "./assets/images/topics/topic14.png",
              "createdBy": "",
              "createdTimeStamp": "",
              "modifiedBy": "",
              "modifiedTimeStamp": "",
            }
          ]
        },
        {}
      ]
    }
    const mockResponse = Observable.of(mockplan);
    httpSpy.get.and.returnValue(mockResponse);
    service.getActionPlanConditions(mockid).subscribe(plans => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.planUrl}` + '/' + mockid);
      expect(service.getActionPlanConditions(mockid)).toEqual(mockResponse);
      done();
    });
  });
  it('should return user profile plan details', (done) => {
    let mockUserPlan = [
      {
        "id": "bf8d7e7e-2574-7b39-efc7-83cb94adae07",
        "oId": "User Id",
        "type": "plans",
        "planTags": [
          {
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
            ]
          },
          {
            "topicId": "932abb0a-c6bb-46da-a3d8-5f52c2c914a0",
            "stepTags": [
              {
                "id": "2705d544-6af7-bd69-4f19-a1b53e346da2",
                "order": 1,
                "markCompleted": false
              },
              {
                "id": "3d64b676-cc4b-397d-a5bb-f4a0ea6d3040",
                "order": 2,
                "markCompleted": false
              }
            ]
          }
        ]
      }
    ]
    const mockResponse = Observable.of(mockUserPlan);
    httpSpy.get.and.returnValue(mockResponse);
    service.getUserPlanId("userId").subscribe(userPlan => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${api.getProfileUrl}/userId`);
      expect(userPlan).toEqual(mockUserPlan);
      done();
    });
  });
  it('should return mark completed updated plan when passed value is update plan', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    service.getMarkCompletedUpdatedPlan(mockupdateplan).subscribe(updateplan => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(updateplan).toEqual(mockPlanDetailsJson);
      done();
    });
  });
  it('should return userplan when value passed is plan', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    service.userPlan(mockupdateplan).subscribe(userplan => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(userplan).toEqual(mockPlanDetailsJson);
      done();
    });
  });
  it('should call getPersonalizedResources when value is  resource input', () => {
    spyOn(service, 'getBookmarkedData');
    spyOn(service, 'getPersonalizedResources');    
   
    expect(service.getPersonalizedResources).toBeDefined();
 
  });
  it('should return resources when saveResourcesToSession method called', () => {
    spyOn(service, 'saveResourcesToSession');
    spyOn(arrayUtilityService, 'checkObjectExistInArray');
    service.saveResourcesToSession(resoureStorage);
    arrayUtilityService.checkObjectExistInArray(mockObjects, mockObject);  
    expect(service.saveResourcesToSession).toHaveBeenCalled();
    expect(arrayUtilityService.checkObjectExistInArray).toHaveBeenCalled();
  });
  it('should return topics list when createTopicsList method called', () => {
    const mockResponse = Observable.of(mockTopicsList);
    spyOn(service, 'createTopicsList').and.returnValue(mockResponse);   
    service.createTopicsList(mockTopicsList);
    expect(service.createTopicsList).toBeDefined();   
  });

});
