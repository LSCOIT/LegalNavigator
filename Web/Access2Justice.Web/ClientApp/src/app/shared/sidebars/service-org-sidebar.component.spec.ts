import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LocationService } from '../location/location.service'
import { ServiceOrgSidebarComponent } from './service-org-sidebar.component';
import { ServiceOrgService } from './service-org.service';
import { HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Organization } from '../sidebars/organization';
import { MapLocation } from '../location/location';

describe('ServiceOrgSidebarComponent',() =>
{
  let component: ServiceOrgSidebarComponent;
  let fixture: ComponentFixture<ServiceOrgSidebarComponent>;
  let locationService: LocationService;
  let serviceorgservice: ServiceOrgService;
  let subscription: any;
  let mockMapLocation: MapLocation = {
    state: 'teststate',
    city: 'testcity',
    county: 'testcounty',
    zipCode: '000007',
    locality: undefined,
    address: undefined
  };

  let mockOrganizations:[
    {
      "id": "19a02209-ca38-4b74-bd67-6ea941d41518",
      "name": "Legal Help Organization",
      "type": "Housing Law Services",
      "description": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.", "resourceType": "Organizations", "externalUrl": "", "url": "websiteurl.com", "topicTags": [{ "id": "afabf032-72a8-4b04-81cb-c101bb1a0730" }, { "id": "3aa3a1be-8291-42b1-85c2-252f756febbc" }], "location": [{ "zipCode": "96741" }, { "state": "Hawaii", "city": "Haiku-Pauwela" }, { "state": "Alaska" }], "icon": "./assets/images/resources/resource.png", "address": "Honolulu, Hawaii 96813, United States", "telephone": "XXX-XXX-XXXX", "overview": "Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter. Lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem consecuter lorem ipsum solor sit amet bibodem consecuter.", "eligibilityInformation": "Copy describing eligibility qualification lorem ipsum dolor sit amet. ", "reviewedByCommunityMember": "Quote from community member consectetur adipiscing elit. Aenean euismod bibendum laoreet. Proin gravida dolor sit amet lacus accumsan et viverra justo commodo. Proin sodales pulvinar tempor. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.", "reviewerFullName": "", "reviewerTitle": "", "reviewerImage": "", "createdBy": "", "createdTimeStamp": "", "modifiedBy": "", "modifiedTimeStamp": "2018-04-01T04:18:00Z", "_rid": "mwoSAJdNlwIGAAAAAAAAAA==", "_self": "dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIGAAAAAAAAAA==/", "_etag": "\"41000b36-0000-0000-0000-5b1e56600000\"", "_attachments": "attachments/", "_ts": 1528714848
    }
  ];

  beforeEach(
    async(() => {
      TestBed.configureTestingModule({
        imports: [HttpClientModule],
        declarations: [ServiceOrgSidebarComponent],
        providers: [
          ServiceOrgService,
          LocationService
        ]
      });
    TestBed.compileComponents();
    fixture = TestBed.createComponent(ServiceOrgSidebarComponent);
    component = fixture.componentInstance;
    locationService = TestBed.get(LocationService);
    serviceorgservice = TestBed.get(ServiceOrgService);

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

  it("should create service organization sidebar componet", () =>{
    expect(component).toBeTruthy();
  });

  it("should define service organization sidebar componet", () =>{
    expect(component).toBeDefined();
  });

  it("should call getOrganizationDetails service method when getOrganizations is called", () =>{
    spyOn(serviceorgservice, 'getOrganizationDetails').and.returnValue(Observable.of());
    component.getOrganizations(mockMapLocation);
    expect(serviceorgservice.getOrganizationDetails).toHaveBeenCalled();
  });

  it("should assign session storage details to map location on ngInit", () =>{
    sessionStorage.setItem("globalMapLocation", JSON.stringify(mockMapLocation));
    component.ngOnInit();
    expect(component.location).toEqual(JSON.parse(sessionStorage.getItem("globalMapLocation")));
  });

  it("should not assign session storage details to map location on ngInit there is no key for global map in session storage", () => {
    sessionStorage.removeItem("mockGlobalMapLocation");
    component.ngOnInit();
    expect(component.location).toEqual(undefined);
  });
});

