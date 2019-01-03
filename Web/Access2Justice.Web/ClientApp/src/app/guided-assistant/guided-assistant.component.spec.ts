import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GuidedAssistantComponent } from './guided-assistant.component';
import { HttpClientModule } from '@angular/common/http';
import { MapService } from '../shared/map/map.service';
import { NavigateDataService } from '../shared/navigate-data.service';
import { NgForm } from '@angular/forms';
import { of } from 'rxjs/observable/of';
import { Router } from '@angular/router';
import { SearchService } from '../shared/search/search.service';
import { TopicService } from '../topics-resources/shared/topic.service';
import { StaticResourceService } from '../shared/static-resource.service';
import { Global } from '../global';

describe('GuidedAssistantComponent', () => {
  let component: GuidedAssistantComponent;
  let fixture: ComponentFixture<GuidedAssistantComponent>;
  let mockNavigateDataService;
  let mockSearchService;
  let mockguidedAssistantSearchForm;
  let mockSearchResponse;
  let mockMapLocation;
  let store;
  let mockSessionStorage;
  let mockMapLocationParsed;
  let mockRouter;
  let mockStaticResourceService;
  let mockGlobalService;
  let mockGuidedAssistantDescription;
  let mockGlobalData;
  let mockDescription = "test";

  beforeEach(async(() => {
    mockguidedAssistantSearchForm = <NgForm>{
      value: {
        searchText: "i am getting kicked out"}
    }
    mockGuidedAssistantDescription = {
      name: "GuidedAssistantDescription",
      description: "test",
      location: [
        { state: "AK" }
      ]
    },
      mockGlobalData = [{
      name: "GuidedAssistantDescription",
        location: [
          { state: "AK" }
        ]
      }]
    mockNavigateDataService = jasmine.createSpyObj(['setData']);
    mockSearchService = jasmine.createSpyObj(['search']);
    mockRouter = jasmine.createSpyObj(['navigateByUrl']);
    mockSearchResponse = {
      "topIntent": "Eviction",
      "relevantIntents": [
        "Tenant's rights",
        "Domestic Violence",
        "Mobile home park tenants"
      ],
      "topics": [
        {
          "id": "1370ccb7-3f0a-4b0f-920e-2d12660fafa7",
          "name": "Problems with my landlord, home, or apartment",
          "overview": "Overview of the Problems with My Landlord, Home, or Apartment topic",
          "quickLinks": [
            {
              "text": "Apartment Rules",
              "url": "https://alaskalawhelp.org/resource/apartment-rules?ref=o4LzP"
            },
            {
              "text": "Conversations about Landlord Tenant Law in Alaska: Introduction",
              "url": "https://alaskalawhelp.org/resource/conversations-about-landlord-tenant-law-in-alaska-introduction"
            }
          ],
          "parentTopicId": [
            {
              "id": "62a93f03-8234-46f1-9c35-b3146a96ca8b"
            }
          ],
          "resourceType": "Topics",
          "keywords": "Landlord problem | Eviction",
          "location": [
            {
              "state": "Hawaii",
              "city": "Kalawao",
              "zipCode": "96761"
            }
          ]
        }
      ],
      "resources": [
        {
          "id": "afbb8dc1-f721-485f-ab92-1bab60137e24",
          "name": "Cook Inlet Tribal Council Inc./Family Services Department",
          "type": "Interim Housing Assistance",
          "description": "Funding provided by Cook Inlet Housing Authority, Inc.",
          "resourceType": "Organizations",
          "url": "https://citci.org/child-family/",
          "topicTags": [
            {
              "id": "62a93f03-8234-46f1-9c35-b3146a96ca8b"
            }
          ],
          "location": [
            {
              "state": "Hawaii",
              "city": "Kalawao",
              "zipCode": "96761"
            }
          ]
        }
      ],
      "topicIds": [
        "1370ccb7-3f0a-4b0f-920e-2d12660fafa7"
      ],
      "guidedAssistantId": "12345"
    }
    mockMapLocation = {
      location: {
        "state": "Alaska"
      }
    };
    mockMapLocationParsed = {
      state:"Alaska"
    }
    store = {};
    mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      }
    };

    mockStaticResourceService = jasmine.createSpyObj(['getLocation', 'getStaticContents']);
    mockGlobalService = jasmine.createSpyObj(['getData']);
    mockGlobalService.getData.and.returnValue([mockGuidedAssistantDescription]);
    mockStaticResourceService.getStaticContents.and.returnValue(mockGuidedAssistantDescription);
    mockStaticResourceService.getLocation.and.returnValue('AK');

    TestBed.configureTestingModule({
      declarations: [
        GuidedAssistantComponent,
      ],
      imports: [
        FormsModule,
        HttpClientModule
      ],
      providers: [
        TopicService,
        MapService,
        { provide: SearchService, useValue: mockSearchService },
        { provide: NavigateDataService, useValue: mockNavigateDataService},
        { provide: Router, useValue: mockRouter },
        { provide: StaticResourceService, useValue: mockStaticResourceService },
        { provide: Global, useValue: mockGlobalService }
      ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
      .compileComponents();
    fixture = TestBed.createComponent(GuidedAssistantComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  }));

  it('should create guided assisstant component', () => {
    expect(component).toBeTruthy();
  });

  it('should call onSubmit method when Continue button is clicked', () => {
    spyOn(sessionStorage, 'getItem')
      .and.returnValue(JSON.stringify(mockMapLocation));
    mockSearchService.search.and.returnValue(of(mockSearchResponse));

    component.onSubmit(mockguidedAssistantSearchForm);
    expect(component.luisInput["Sentence"]).toEqual("i am getting kicked out");
    expect(component.luisInput["Location"]).toEqual(mockMapLocationParsed);
    expect(mockNavigateDataService.setData).toHaveBeenCalled();
    expect(component.guidedAssistantResults).toEqual(mockSearchResponse);
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/guidedassistant/search');
  });

  it('should set guided assistant page description to static resource guided assistant page description content if it exists', () => {
    mockStaticResourceService.getLocation.and.returnValue('AK');
    mockStaticResourceService.getStaticContents.and.returnValue(mockGuidedAssistantDescription);
    component.name = mockGuidedAssistantDescription.name;
    component.getGuidedAssistantContent();
    expect(component.description).toEqual(mockDescription);
  });
});
