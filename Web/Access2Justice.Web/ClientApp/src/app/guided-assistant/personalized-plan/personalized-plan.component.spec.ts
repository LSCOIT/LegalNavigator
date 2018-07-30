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
  }];
  let mockTopics = [{ 'id': '3445', planTags: [{}] }];
  let mockTempTopicsList = [{
    topic: {
      "topicId": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
      "stepTags": [{
          "id":{
            "id": "6b230be1-302b-7090-6cb3-fc6aa084274c",
            "type": "steps",
            "title": "Make sure your summons is real.",
            "description": "Why you should do this dolor sit amet, consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo.",
            "resourceTags": [
              {
                "id": "9fc75d90-7ffa-4c26-9cb7-ba271f2007ad"
              },
              {
                "id": "19a02209-ca38-4b74-bd67-6ea941d41518"
              }
            ]
          },
          "order": 1,
          "markCompleted": false
        }],
      "id": [{
          "id": "addf41e9-1a27-4aeb-bcbb-7959f95094ba",
          "name": "Family",
          "parentTopicID": "",
          "resourceType": "Topics",
          "keywords": "EVICTION",
          "location":
            {
              "state": "Hawaii",
              "county": "Kalawao County",
              "city": "Kalawao",
              "zipCode": "96742"
            }
        }]
    }, isSelected: true
  }];
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
