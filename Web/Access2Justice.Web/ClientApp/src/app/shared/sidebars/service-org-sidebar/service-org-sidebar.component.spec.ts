import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MapService } from '../../map/map.service'
import { ServiceOrgSidebarComponent } from './service-org-sidebar.component';
import { HttpClientModule } from '@angular/common/http';
import { MapLocation } from '../../map/map';
import { NavigateDataService } from '../../navigate-data.service';
import { PaginationService } from '../../pagination/pagination.service';
import { ShowMoreService } from '../show-more/show-more.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IResourceFilter } from '../../search/search-results/search-results.model';

describe('Component:ServiceOrgSidebar', () => {
  class MockRouter {
    navigate = jasmine.createSpy('navigate');
  }
  let component: ServiceOrgSidebarComponent;
  let fixture: ComponentFixture<ServiceOrgSidebarComponent>;
  const mockRouter = new MockRouter();
  let showMoreService: ShowMoreService;
  let mapService: MapService;
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
  let mockTopicIds: string[] = [];
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
        ShowMoreService,
        MapService,
        NavigateDataService,
        PaginationService
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ServiceOrgSidebarComponent);
    component = fixture.componentInstance;
    mapService = TestBed.get(MapService);
    showMoreService = TestBed.get(ShowMoreService);

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
    expect(component.getOrganizations).toHaveBeenCalled();
  });
});
