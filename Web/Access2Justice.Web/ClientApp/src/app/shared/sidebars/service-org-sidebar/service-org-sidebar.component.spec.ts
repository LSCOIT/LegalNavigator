import { Component, OnInit, Input, Output, EventEmitter, NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MapService } from '../../map/map.service'
import { ServiceOrgSidebarComponent } from './service-org-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { MapLocation } from '../../map/map';
import { NavigateDataService } from '../../navigate-data.service';
import { PaginationService } from '../../pagination/pagination.service';
import { ShowMoreService } from '../show-more/show-more.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { IResourceFilter } from '../../search/search-results/search-results.model';
import { of } from 'rxjs/observable/of';
import { APP_BASE_HREF } from '@angular/common';
import { Subject } from 'rxjs';
import { Observable } from 'rxjs/Observable';

describe('Component:ServiceOrgSidebar', () => {

  let component: ServiceOrgSidebarComponent;
  let fixture: ComponentFixture<ServiceOrgSidebarComponent>;
  let mockMapService;
  let mockPaginationService;
  let mockNavigateDataService;
  let mockactiveTopic = "123";
  let mockMapLocation: MapLocation = {
    state: 'teststate',
    city: 'testcity',
    county: 'testcounty',
    zipCode: '000007',
    locality: undefined,
    address: undefined
  };
  let mockresourcesInput: IResourceFilter = {
    ResourceType: 'Organizations',
    ContinuationToken: '',
    TopicIds: [mockactiveTopic],
    ResourceIds: [],
    PageNumber: 0,
    Location: mockMapLocation,
    IsResourceCountRequired: true
  };
  let mockOrganizations = {
    "resources": [{
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
          "county": "Kalawao County",
          "city": "Kalawao",
          "zipCode": "96742"
        }
      ],
      "icon": "",
      "address": "Girdwood, Anchorage, Hawii 99587",
      "telephone": "907-793-3600",
      "overview": "CITC offers a wide variety of programs and services to support individuals in their journey",
      "eligibilityInformation": "helping Alaska Native and American Indian people residing in the Cook Inlet ",
      "reviewedByCommunityMember": "CITC collaborates with the eight federally recognized tribes within Cook ",
      "reviewerFullName": "John Smith",
      "reviewerTitle": "Tribal Leader",
      "reviewerImage": ""
    },
    {
      "id": "afbb8dc1-f721-485f-ab92-1bab60137e24",
      "name": "Cook Inlet Tribal Council Inc./Family Services Department",
      "type": "Interim Housing Assistance",
      "description": "Funding provided by Cook Inlet Housing Authority, Inc.",
      "resourceType": "Organizations",
      "url": "https://citci.org/child-family/"
    }, {
      "id": "afbb8dc1-f721-485f-ab92-1bab60137e24",
      "name": "Cook Inlet Tribal Council Inc./Family Services Department",
      "type": "Interim Housing Assistance",
      "description": "Funding provided by Cook Inlet Housing Authority, Inc.",
      "resourceType": "Organizations",
      "url": "https://citci.org/child-family/"
    }, {
      "id": "afbb8dc1-f721-485f-ab92-1bab60137e24",
      "name": "Cook Inlet Tribal Council Inc./Family Services Department",
      "type": "Interim Housing Assistance",
      "description": "Funding provided by Cook Inlet Housing Authority, Inc.",
      "resourceType": "Organizations",
      "url": "https://citci.org/child-family/"
    }]
  }
  let mockLessOrganizations = {
    "resources": [
      {
        "id": "afbb8dc1-f721-485f-ab92-1bab60137e24",
        "name": "Cook Inlet Tribal Council Inc./Family Services Department",
        "type": "Interim Housing Assistance",
        "description": "Funding provided by Cook Inlet Housing Authority, Inc.",
        "resourceType": "Organizations",
        "url": "https://citci.org/child-family/"
      }, {
        "id": "afbb8dc1-f721-485f-ab92-1bab60137e24",
        "name": "Cook Inlet Tribal Council Inc./Family Services Department",
        "type": "Interim Housing Assistance",
        "description": "Funding provided by Cook Inlet Housing Authority, Inc.",
        "resourceType": "Organizations",
        "url": "https://citci.org/child-family/"
      }]
  }
  let nofityLocation: Subject<object> = new Subject<object>();
  beforeEach(async(() => {
    mockPaginationService = jasmine.createSpyObj(['getPagedResources']);
    mockNavigateDataService = jasmine.createSpyObj(['getData', 'setData']);
    mockPaginationService.getPagedResources.and.returnValue(of(mockOrganizations));

    TestBed.configureTestingModule({
      declarations: [ServiceOrgSidebarComponent
      ],
      imports: [
        RouterModule.forRoot([
          { path: 'topics/:topic', component: ServiceOrgSidebarComponent }
        ]),
        HttpClientModule
      ],
      providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: { 'topic': 'bd900039-2236-8c2c-8702-d31855c56b0f' }
            },
            url: of([
              { path: 'subtopics', params: {} },
              { path: 'bd900039-2236-8c2c-8702-d31855c56b0f', params: {} }
            ])
          }
        },
        MapService,
        { provide: NavigateDataService, useValue: mockNavigateDataService },
        { provide: PaginationService, useValue: mockPaginationService }

      ],
      schemas: [NO_ERRORS_SCHEMA, CUSTOM_ELEMENTS_SCHEMA]
    })
      .compileComponents();

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
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServiceOrgSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create service organization sidebar componet", () => {
    expect(component).toBeTruthy();
  });

  it("should define service organization sidebar componet", () => {
    expect(component).toBeDefined();
  });

  it("should emit on callOrganizations", () => {
    spyOn(component.showMoreOrganizations, 'emit');
    component.callOrganizations();
    expect(component.showMoreOrganizations.emit).toHaveBeenCalled();
    expect(component.showMoreOrganizations.emit).toHaveBeenCalledWith('Organizations');
  });

  it("should return organization Details when getPagedResource method of pagination service called", () => {
    component.location = mockMapLocation;
    component.activeTopic = mockactiveTopic;
    component.getOrganizations();
    expect(component.organizations.length).toEqual(3);
  });

  it("should return organization Details when getPagedResource method of pagination service called", () => {
    component.location = mockMapLocation;
    component.activeTopic = mockactiveTopic;
    mockPaginationService.getPagedResources.and.returnValue(of(mockLessOrganizations));
    component.getOrganizations();
    expect(component.organizations.length).toEqual(mockLessOrganizations.resources.length);
  });

  it("should assign session storage details to map location on ngInit", () => {
    spyOn(component, 'getOrganizations');
    sessionStorage.setItem("globalMapLocation", JSON.stringify(mockMapLocation));
    component.ngOnInit();
    expect(component.location).toEqual(JSON.parse(sessionStorage.getItem("globalMapLocation")));
    expect(component.getOrganizations).toHaveBeenCalled();
  });

  it("should not assign session storage details to map location on ngInit there is no key for global map in session storage", () => {
    sessionStorage.removeItem("mockGlobalMapLocation");
    component.ngOnInit();
    expect(component.location).toEqual(undefined);
  });
});
