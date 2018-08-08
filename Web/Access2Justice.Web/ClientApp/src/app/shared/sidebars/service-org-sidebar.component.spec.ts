import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MapLocation } from '../location/location';
import { ServiceOrgSidebarComponent } from './service-org-sidebar.component';
import { NavigateDataService } from '../navigate-data.service';
import { PaginationService } from '../search/pagination.service';
import { LocationService } from '../location/location.service';
import { ServiceOrgService } from './service-org.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IResourceFilter } from '../search/search-results/search-results.model';

describe('Component:ServiceOrgSidebar', () => {
  class MockRouter {
    navigate = jasmine.createSpy('navigate');
  }
  let component: ServiceOrgSidebarComponent;
  let fixture: ComponentFixture<ServiceOrgSidebarComponent>;
  const mockRouter = new MockRouter();
  let router: Router;
  let activeRoute: ActivatedRoute;
  let serviceorgservice: ServiceOrgService;
  let locationService: LocationService;
  let navigateDataService: NavigateDataService;
  let paginationService: PaginationService;
  const fakeActivatedRoute = {
    snapshot: { data: {} }
  } as ActivatedRoute;

  let mockMapLocation: MapLocation = {
    state: 'teststate',
    city: 'testcity',
    county: 'testcounty',
    zipCode: '000007',
    locality: undefined,
    address: undefined
  };
  let mockOrganizations =
    {
      "id": "19a02209-ca38-4b74-bd67-6ea941d41518",
      "name": "Legal Help Organization",
      "type": "Housing Law Services",
      "description": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.", "resourceType": "Organizations", "externalUrl": "", "url": "websiteurl.com", "topicTags": [{ "id": "afabf032-72a8-4b04-81cb-c101bb1a0730" }, { "id": "3aa3a1be-8291-42b1-85c2-252f756febbc" }], "location": [{ "zipCode": "96741" }, { "state": "Hawaii", "city": "Haiku-Pauwela" }, { "state": "Alaska" }], "icon": "./assets/images/resources/resource.png", "address": "Honolulu, Hawaii 96813, United States", "telephone": "XXX-XXX-XXXX", "overview": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.", "eligibilityInformation": "Copy describing eligibility qualification lorem ipsum dolor sit amet. ", "reviewedByCommunityMember": "Quote from community member consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar tempor. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.", "reviewerFullName": "", "reviewerTitle": "", "reviewerImage": "", "createdBy": "", "createdTimeStamp": "", "modifiedBy": "", "modifiedTimeStamp": "2018-04-01T04:18:00Z", "_rid": "mwoSAJdNlwIGAAAAAAAAAA==", "_self": "dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIGAAAAAAAAAA==/", "_etag": "\"41000b36-0000-0000-0000-5b1e56600000\"", "_attachments": "attachments/", "_ts": 1528714848
    };
  let mockTopicIds: string[] = [];
  let topIntent = 'test';
  let mockResourceType = 'Organizations';
  let mockContinuationToken = 'test';
  let mockResourceIds = ['test'];
  let mockLocation = 'test';
  let mockActiveTopic = "addf41e9-1a27-4aeb-bcbb-7959f95094ba";
  mockTopicIds.push(mockActiveTopic);

  let mockResourceInput: IResourceFilter = {
    ResourceType: mockResourceType,
    ContinuationToken: mockContinuationToken,
    TopicIds: mockTopicIds,
    PageNumber: 1,
    Location: mockLocation,
    IsResourceCountRequired: false,
    ResourceIds: mockResourceIds,
  };
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      declarations: [ServiceOrgSidebarComponent],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: fakeActivatedRoute },
        ServiceOrgService,
        LocationService,
        NavigateDataService,
        PaginationService
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ServiceOrgSidebarComponent);
    component = fixture.componentInstance;
    serviceorgservice = TestBed.get(ServiceOrgService);
    locationService = TestBed.get(LocationService);
    navigateDataService = TestBed.get(NavigateDataService);
    paginationService = TestBed.get(PaginationService);

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

  it("should create service organization sidebar componet", () => {
    expect(component).toBeTruthy();
  });

  it("should define service organization sidebar componet", () => {
    expect(component).toBeDefined();
  });

  it('should test the emitter with a Jasmine spy', () => {
    spyOn(component.showMoreOrganizations, 'emit');
    const button = fixture.nativeElement.querySelector('button');
    button.click();
    expect(component.showMoreOrganizations.emit).toHaveBeenCalledWith('Organizations');
  });

  it('should fire the event emitter when triggering an event', async(() => {
    component.showMoreOrganizations.subscribe(d => {
      expect(d).toBe('Organizations');
    });
    fixture.debugElement.triggerEventHandler('showMoreOrganizations', <Event>{});
  }));

  it("should assign session storage details to map location on ngInit", () => {
    spyOn(component, 'getOrganizations');
    component.ngOnInit();
    sessionStorage.setItem("globalMapLocation", JSON.stringify(mockMapLocation));
    let mockSessionMapLocation = JSON.parse(sessionStorage.getItem("mockGlobalMapLocation"));
    expect(component.getOrganizations).toHaveBeenCalled();
  });
});

