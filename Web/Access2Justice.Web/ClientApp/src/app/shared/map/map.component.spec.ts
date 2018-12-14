import { ComponentFixture, TestBed, async } from '@angular/core/testing';
import { MapComponent } from './map.component';
import { MapService } from './map.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { TemplateRef, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ModalModule } from 'ngx-bootstrap';
import { MapLocation } from './map';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { MapResultsService } from '../sidebars/map-results/map-results.service';
import { Observable } from 'rxjs/Observable';
import { StaticResourceService } from '../static-resource.service';
import { Global } from '../../global';
import { MsalService } from '@azure/msal-angular';
import { of } from 'rxjs/observable/of';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import { FormsModule } from '@angular/forms';
import { environment } from '../../../environments/environment';
import { EventUtilityService } from '../event-utility.service';

class MockBsModalRef {
  public isHideCalled = false;

  hide() {
    this.isHideCalled = true;
  }
}

describe('MapComponent', () => {
  let component: MapComponent;
  let fixture: ComponentFixture<MapComponent>;
  let modalService: BsModalService;
  let template: TemplateRef<any>;  
  let mockMapLocation: MapLocation = { state: 'Sample State', city: 'Sample City', county: 'Sample County', zipCode: '1009203' };//, locality: 'Sample Location',    address: 'Sample Address' };
  let mapResultsService: MapResultsService;
  let mockGeolocationPositionLangitude: '77.33817429999999';
  let mockGeolocationPositionLatiitude: '28.5372834';
  let mockBingKey = 'testAqpcQxjuTmheUzCm8b5kUhV9UhjfsK66Ctest';
  let mockselectedAddress: any;
  let mockType = true;
  let mockResponse = { authenticationResultCode: "ValidCredentials", brandLogoUri: "http://dev.virtualearth.net/Branding/logo_powered_by.png", copyright: "Copyright © 2018 Microsoft and its suppliers. All …ss written permission from Microsoft Corporation.", resourceSets: Array(1), statusCode: 200, };
  let mockMapLocationDetails = '{authenticationResultCode:  "ValidCredentials"  brandLogoUri  :  "http://dev.virtualearth.net/Branding/logo_powered_by.png"  copyright  :  "Copyright © 2018 Microsoft and its suppliers. All rights reserved. This API cannot be accessed and the content and any results may not be used, reproduced or transmitted in any manner without express written permission from Microsoft Corporation."  resourceSets  :  Array(1)  0:  { estimatedTotal: 1, resources: Array(1) }  length  :  1  __proto__  :  Array(0)  statusCode  :  200  statusDescription  :  "OK"  traceId  :  "39bbf89d6d2b4691a1cfeb1d8a504eac|HK20240360|7.7.0.0|HK01EAP000001D3"  __proto__  :  Object}';
  let mockMapLocation2 = { "state": "UP", "city": "Noida", "zipCode": "201303", "locality": "201303", "address": "UP" };
  let mockMapLocationWithFormatAddressGlobal = { "state": "UP", "city": "Sample City", "county": "Sample County", "zipCode": "1009203"};
  let mockMapLocationWithFormatAddressLocal = { "state": "Sample State", "city": "Sample City", "county": "Sample County", "zipCode": "1009203"};
  let mockDisplayLocationDetails = { "locality": "UP", "address": "UP" };
  let mockLocationDetailsWithFormatAddress = {
    location: mockMapLocation,
    displayLocationDetails: mockDisplayLocationDetails,
    country: "United States",
    formattedAddress: "AK"
  };
  let mockMapResultsLocation = { "resourceSets": [{ "resources": [{"name":"UP"}]}]};
  let mockLocationDetailsWithOutFormatAddress = {
    location: mockMapLocation,
    country: "United States",
    formattedAddress: "Hjorth St, Indio, California 92201, United States"
  };
  let mockConfig = { ignoreBackdropClick: false, keyboard: true };
  let mockPositionError = { code: 1, message: "User denied Geolocation" };
  let mapService, msalService;
  let returnLocationValue = { "value": "Hawaii" };
  let modalRefInstance = new MockBsModalRef();
 
  beforeEach(() => {
    mapService = jasmine.createSpyObj(['getMap', 'updateLocation', 'mapLocationDetails', 'identifyLocation']);
    msalService = jasmine.createSpyObj(['getUser']);
    
    TestBed.configureTestingModule({
      imports: [HttpClientModule, RouterTestingModule, ModalModule.forRoot(), FormsModule],
      declarations: [MapComponent],
      providers: [
        BsModalService,
        MapResultsService,
        StaticResourceService,
        Global,
        { provide: MsalService, useValue: msalService },
        { provide: MapService, useValue: mapService },
        EventUtilityService
      ],
      schemas: [
        CUSTOM_ELEMENTS_SCHEMA
      ]
    });

    TestBed.compileComponents();
    fixture = TestBed.createComponent(MapComponent);
    component = fixture.componentInstance;
    mapService = TestBed.get(MapService);
    modalService = TestBed.get(BsModalService);
    mapResultsService = TestBed.get(MapResultsService);

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
    
    spyOn(sessionStorage, 'setItem')
      .and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, 'removeItem')
      .and.callFake(mockSessionStorage.removeItem);
    spyOn(sessionStorage, 'clear')
      .and.callFake(mockSessionStorage.clear);
  });

  it("should create component", () => {
    expect(component).toBeTruthy();
  });

  it("should define component", () => {
    expect(component).toBeDefined();
  });

  it("should call displayLocationDetails on ngInit", () => {
    spyOn(component, 'displayLocationDetails');
    spyOn(component, 'ngOnInit');
    component.displayLocationDetails(mockMapLocation);
    expect(component.displayLocationDetails).toHaveBeenCalled();    
  });

  it("should call modalService show when openModal is called", () => {
    var testElement = document.createElement('div');
    document.getElementById = jasmine.createSpy('HTML Element').and.returnValue(testElement);
    spyOn(modalService, 'show');
    spyOn(document.getElementById("search-box"), 'focus');
    component.openModal(template);
    expect(modalService.show).toHaveBeenCalled();
    expect(document.getElementById("search-box").focus).toBeDefined(); 
  });

  it("should call mapService getMap when openModal is called", () => {
    component.openModal(template);
    expect(mapService.getMap).toHaveBeenCalled();
  });

  it("should call update location of location service when location error is false", () => {    
    component.modalRef = modalRefInstance;
    component.locationError = false;
    spyOn(component,'getLocationDetails');
    component.updateLocation();
    expect(component.isError).toBeFalsy();
    expect(component.locationError).toBeUndefined;
  });
  
  it("should call geocode when showlocation is true in updateLocation method", () => {
    spyOn(component, 'geocode');
    spyOn(sessionStorage, 'getItem');
    component.locationError = true;
    component.updateLocation();
    expect(component.geocode).toHaveBeenCalled();
  });

  it("should call getStateFullName mapresults service method with formataddress for global map", () => {
    spyOn(component, 'updateLocationDetails');
    spyOn(mapResultsService, 'getStateFullName').and.callFake(() => {
      return Observable.from([mockMapResultsLocation]);
    });
    spyOn(sessionStorage, 'getItem')
      .and.returnValue(JSON.stringify(mockLocationDetailsWithFormatAddress));
    environment.map_type = true;
    component.getLocationDetails();
    expect(mapResultsService.getStateFullName).toHaveBeenCalled();
    expect(component.locationDetails.location).toEqual(mockMapLocationWithFormatAddressGlobal);
    expect(component.locationDetails.displayLocationDetails).toEqual(mockDisplayLocationDetails);
    expect(component.updateLocationDetails).toHaveBeenCalled();
  });

  it("should call getStateFullName mapresults service method with formataddress for local map", () => {
    spyOn(component, 'updateLocationDetails');
    spyOn(mapResultsService, 'getStateFullName').and.callFake(() => {
      return Observable.from([mockMapResultsLocation]);
    });
    spyOn(sessionStorage, 'getItem')
      .and.returnValue(JSON.stringify(mockLocationDetailsWithFormatAddress));
    environment.map_type = false;
    component.getLocationDetails();
    expect(mapResultsService.getStateFullName).toHaveBeenCalled();
    expect(component.locationDetails.location).toEqual(mockMapLocationWithFormatAddressLocal);
    expect(component.locationDetails.displayLocationDetails).toEqual(mockDisplayLocationDetails);
    expect(component.updateLocationDetails).toHaveBeenCalled();
  });

  it("should call updateLocationDetails method without formataddress", () => {
    spyOn(component, 'updateLocationDetails');
    spyOn(mapResultsService, 'getStateFullName').and.callFake(() => {
      return Observable.from([mockMapResultsLocation]);
    });
    spyOn(sessionStorage, 'getItem')
      .and.returnValue(JSON.stringify(mockLocationDetailsWithOutFormatAddress));
    environment.map_type = true;
    component.getLocationDetails();
    expect(component.updateLocationDetails).toHaveBeenCalled();
  });

  it("should call hide of modal ref when updateLocationDetails of component is called", () => {    
    component.modalRef = modalRefInstance;
    component.mapType = false;
    component.changeLocationButton = {
      nativeElement: jasmine.createSpyObj('nativeElement', ['focus'])
    }
    mapService.updateLocation.and.returnValue(mockLocationDetailsWithFormatAddress);
    spyOn(component,'displayLocationDetails');
    component.updateLocationDetails();
    expect(component.changeLocationButton.nativeElement.focus).toHaveBeenCalled();
    expect(modalRefInstance.isHideCalled).toBeTruthy();
    expect(component.showLocality).toBeFalsy();
  });

  it("should call displayLocationDetails when updateLocation is called", () => {    
    component.modalRef = modalRefInstance;
    let mockLocation = {
      location: null,
      country: "United States",
      formattedAddress: "Hjorth St, Indio, California 92201, United States"}
    mapService.updateLocation.and.returnValue(mockLocation);
    component.mapType = true;
    spyOn(component, 'displayLocationDetails');
    component.updateLocationDetails();
    expect(component.isError).toBeTruthy();
  });

  it("should set the address,locality and showLocation variables of component when displayLocationDetails is called", () => {
    spyOn(modalService, 'hide');
    spyOn(component,'setDisplayLocationDetails');
    component.displayLocationDetails(mockDisplayLocationDetails);
    expect(component.displayLocation).toEqual(mockDisplayLocationDetails);
    expect(component.showLocation).toBeFalsy();
  });

  it('should set display location details', () => {
    component.displayLocation = mockDisplayLocationDetails;
    component.setDisplayLocationDetails();
    expect(component.address).toEqual(mockDisplayLocationDetails.address);
    expect(component.locality).toEqual(mockDisplayLocationDetails.locality);
  });

  it("should set the flag true on searchChange", () => {
    component.isError = false;
    component.onSearchChange();
    expect(component.isError).toBe(false);
  });

  it("should set the flag false on searchChange", () => {
    component.isError = true;
    component.onSearchChange();
    expect(component.isError).toBe(false);
  });

  it("should load current location", () => {
    spyOn(component, 'loadCurrentLocation');
    expect(component.loadCurrentLocation).toBeTruthy();
  });

  it('should load all components for current location - getAddressBasedOnPoints', () => {
    spyOn(navigator.geolocation, "getCurrentPosition");
    spyOn(mapResultsService, "getAddressBasedOnPoints");
    let mockselectedAddress = mockResponse;
    let mockLocationInputRequired = true;
    component.openModal(template);
    mapService.updateLocation.and.returnValue(mockLocationDetailsWithFormatAddress);
    component.displayLocationDetails(mockMapLocation2);
    expect(component.address).toEqual(mockMapLocation2.address);
    expect(component.locality).toEqual(mockMapLocation2.locality);

    expect(mapService.updateLocation).toBeTruthy();
    mapResultsService.getAddressBasedOnPoints(mockGeolocationPositionLatiitude, mockGeolocationPositionLangitude, mockBingKey);
    component.loadCurrentLocation();
    expect(mapResultsService.getAddressBasedOnPoints).toBeTruthy();
    expect(navigator.geolocation.getCurrentPosition).toBeTruthy();
  });

  it('should not load current location and throw error', () => {
    spyOn(navigator.geolocation, "getCurrentPosition");
    spyOn(mapResultsService, "getAddressBasedOnPoints");
    component.config = {
      ignoreBackdropClick: true,
      keyboard: false
    };
    navigator.geolocation.getCurrentPosition(null, )
    mapResultsService.getAddressBasedOnPoints(0, 0, mockBingKey);
    component.locationInputRequired = false;
    component.loadCurrentLocation();
    component.openModal(template);
    expect(component.loadCurrentLocation).toBeTruthy()
  });

  it('should get search text while executing geocode', async(() => {
    document.getElementById = jasmine.createSpy('search - box').and.returnValue(returnLocationValue);
    component.geocode();
    expect(component.searchLocation).toEqual(returnLocationValue.value);
  }));

  it("should call loadCurrentLocation and displayLocationDetails on ngOnInit", () => {
    spyOn(component, 'loadCurrentLocation');
    spyOn(component, 'displayLocationDetails');
    spyOn(component, 'ngOnInit');
    component.loadCurrentLocation();
    sessionStorage.setItem("mockGlobalMapLocation", JSON.stringify(mockMapLocation));
    let mockSessionMapLocation = JSON.parse(sessionStorage.getItem("mockGlobalMapLocation"));
    component.displayLocationDetails(mockSessionMapLocation);
    expect(component.loadCurrentLocation).toHaveBeenCalled();
    expect(component.displayLocationDetails).toHaveBeenCalled();
  });

  it('should load current location using geolocation current position', () => {
    spyOn(component, "loadCurrentLocation");
    component.loadCurrentLocation();
    expect(navigator.geolocation.getCurrentPosition).toBeTruthy();
    expect(component.loadCurrentLocation).toHaveBeenCalled();
  });

  it('should load all components on current location', () => {
    spyOn(navigator.geolocation, "getCurrentPosition");
    spyOn(mapResultsService, "getAddressBasedOnPoints");
    spyOn(component, "displayLocationDetails");
    spyOn(component, "loadCurrentLocation");
    sessionStorage.setItem("mockGlobalMapLocation", JSON.stringify(mockMapLocation));
    let mockSessionMapLocation = JSON.parse(sessionStorage.getItem("mockGlobalMapLocation"));
    component.mapLocation = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    component.displayLocationDetails(mockSessionMapLocation);
    expect(component.displayLocationDetails).toHaveBeenCalled()
    component.loadCurrentLocation();
    expect(navigator.geolocation.getCurrentPosition).toBeTruthy();
    expect(component.loadCurrentLocation).toHaveBeenCalled();
  });
});
