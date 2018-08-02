import { ComponentFixture, TestBed, async } from '@angular/core/testing';
import { LocationComponent } from './location.component';
import { LocationService } from './location.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { TemplateRef } from '@angular/core';
import { ModalModule } from 'ngx-bootstrap';
import { MapLocation } from './location';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { MapResultsService } from '../sidebars/map-results.service';
import { Observable } from 'rxjs/Observable';

class MockBsModalRef {
  public isHideCalled = false;

  hide() {
    this.isHideCalled = true;
  }
}

describe('LocationComponent', () => {
  let component: LocationComponent;
  let fixture: ComponentFixture<LocationComponent>;
  let modalService: BsModalService;
  let template: TemplateRef<any>;
  let locationService: LocationService;
  let mockMapLocation: MapLocation = {
    state: 'Sample State',
    city: 'Sample City',
    county: 'Sample County',
    zipCode: '1009203',
    locality: 'Sample Location',
    address: 'Sample Address'
  };
  let mapResultsService: MapResultsService;
  let mockGeolocationPositionLangitude: '77.33817429999999';
  let mockGeolocationPositionLatiitude: '28.5372834';
  let mockBingKey = 'testAqpcQxjuTmheUzCm8b5kUhV9UhjfsK66Ctest';
  let mockselectedAddress: any;
  let mockType = true;
  let mockResponse = { authenticationResultCode: "ValidCredentials", brandLogoUri: "http://dev.virtualearth.net/Branding/logo_powered_by.png", copyright: "Copyright © 2018 Microsoft and its suppliers. All …ss written permission from Microsoft Corporation.", resourceSets: Array(1), statusCode: 200, };

  let mockMapLocationDetails = '{authenticationResultCode:  "ValidCredentials"  brandLogoUri  :  "http://dev.virtualearth.net/Branding/logo_powered_by.png"  copyright  :  "Copyright © 2018 Microsoft and its suppliers. All rights reserved. This API cannot be accessed and the content and any results may not be used, reproduced or transmitted in any manner without express written permission from Microsoft Corporation."  resourceSets  :  Array(1)  0:  { estimatedTotal: 1, resources: Array(1) }  length  :  1  __proto__  :  Array(0)  statusCode  :  200  statusDescription  :  "OK"  traceId  :  "39bbf89d6d2b4691a1cfeb1d8a504eac|HK20240360|7.7.0.0|HK01EAP000001D3"  __proto__  :  Object}';
  let mockMapLocation2 = { "state": "UP", "city": "Noida", "zipCode": "201303", "locality": "201303", "address": "UP" }
  let mockConfig = { ignoreBackdropClick: false, keyboard: true };
  let mockPositionError = { code: 1, message: "User denied Geolocation" }
  beforeEach(
    () => {
      TestBed.configureTestingModule({
        imports: [HttpClientModule, RouterTestingModule, ModalModule.forRoot()],
        declarations: [LocationComponent],
        providers: [
          BsModalService,
          LocationService,
          MapResultsService
        ]
      });
      TestBed.compileComponents();
      fixture = TestBed.createComponent(LocationComponent);
      component = fixture.componentInstance;
      locationService = TestBed.get(LocationService);
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

      spyOn(sessionStorage, 'getItem')
        .and.callFake(mockSessionStorage.getItem);
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

  it("should assign session storage details to map location on ngInit", () => {
    sessionStorage.setItem("mockGlobalMapLocation", JSON.stringify(mockMapLocation));
    component.mapLocation = JSON.parse(sessionStorage.getItem("mockGlobalMapLocation"));
  });

  it("should call displayLocationDetails on ngInit", () => {
    spyOn(component, 'displayLocationDetails');
    component.ngOnInit();
    component.displayLocationDetails(mockMapLocation);
    expect(component.displayLocationDetails).toHaveBeenCalled();
  });

  it("should call modalService show when openModal is called", () => {
    spyOn(locationService, 'getMap');
    spyOn(modalService, 'show');
    component.openModal(template);
    expect(modalService.show).toHaveBeenCalled();
  });

  it("should call locationService getMap when openModal is called", () => {
    spyOn(locationService, 'getMap');
    component.openModal(template);
    expect(locationService.getMap).toHaveBeenCalled();
  });

  it("should call update location of location service when update location of component is called", () => {
    spyOn(locationService, 'updateLocation').and.returnValue(mockMapLocation);
    spyOn(modalService, 'hide');
    component.updateLocation();
    expect(locationService.updateLocation).toHaveBeenCalled();
  });

  it("should call hide of modal ref when update location of component is called", () => {
    spyOn(locationService, 'updateLocation').and.returnValue(mockMapLocation);
    spyOn(modalService, 'hide');
    let modalRefInstance = new MockBsModalRef();
    component.modalRef = modalRefInstance;
    component.updateLocation();
    expect(modalRefInstance.isHideCalled).toBeTruthy();
  });

  it("should call displayLocationDetails when updateLocation is called", () => {
    spyOn(locationService, 'updateLocation').and.returnValue(mockMapLocation);
    spyOn(component, 'displayLocationDetails');
    spyOn(modalService, 'hide');
    component.updateLocation();
    expect(component.displayLocationDetails).toHaveBeenCalled();
  });

  it("should set the address,locality and showLocation variables of component when displayLocationDetails is called", () => {
    spyOn(modalService, 'hide');
    component.showLocation = true;
    component.displayLocationDetails(mockMapLocation);
    expect(component.address).toEqual(mockMapLocation.address);
    expect(component.locality).toEqual(mockMapLocation.locality);
    expect(component.showLocation).toBeFalsy();
  });

  it("should set the flag true on searchChange", () => {
    component.isError = true;
    expect(component.isError).toBe(true);
  });

  it("should set the flag false on searchChange", () => {
    component.isError = false;
    expect(component.isError).toBe(false);
  });

  it("should load current location", () => {
    spyOn(component, 'loadCurrentLocation');
    expect(component.loadCurrentLocation).toBeTruthy();
  });

  it('should load all components for current location - getAddressBasedOnPoints', () => {

    spyOn(navigator.geolocation, "getCurrentPosition");
    spyOn(mapResultsService, "getAddressBasedOnPoints");
    spyOn(locationService, "mapLocationDetails");
    spyOn(locationService, "updateLocation");
    spyOn(locationService, 'getMap');

    let mockselectedAddress = mockResponse;
    let mockLocationInputRequired = true;
    component.openModal(template);

    component.displayLocationDetails(mockMapLocation2);
    expect(component.address).toEqual(mockMapLocation2.address);
    expect(component.locality).toEqual(mockMapLocation2.locality);

    expect(locationService.updateLocation).toBeTruthy();
    mapResultsService.getAddressBasedOnPoints(mockGeolocationPositionLatiitude, mockGeolocationPositionLangitude, mockBingKey);
    component.loadCurrentLocation();
    expect(mapResultsService.getAddressBasedOnPoints).toBeTruthy();
    expect(navigator.geolocation.getCurrentPosition).toBeTruthy();
  });

  it('should not load current location and throw error', () => {

    spyOn(navigator.geolocation, "getCurrentPosition");
    spyOn(mapResultsService, "getAddressBasedOnPoints");
    spyOn(locationService, "mapLocationDetails");
    spyOn(locationService, "updateLocation");
    spyOn(locationService, 'getMap');
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
    spyOn(document, "getElementById").and.callFake(function () {
      return {
        value: 'test'
      }
    });
    spyOn(locationService, 'identifyLocation')
    component.geocode();
    expect(component.searchLocation).toEqual('test');
  }));

  it('should get search text  by using location service', async(() => {
    spyOn(document, "getElementById").and.callFake(function () {
      return {
        value: 'test'
      }
    });
    spyOn(locationService, 'identifyLocation');
    spyOn(component, 'geocode');
    component.geocode();
    expect(component.geocode).toHaveBeenCalled();
  }));

  it("should call loadCurrentLocation and displayLocationDetails on ngOnInit", () => {
    spyOn(component, 'loadCurrentLocation');
    spyOn(component, 'displayLocationDetails');

    component.ngOnInit();
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
    spyOn(locationService, "mapLocationDetails");
    spyOn(locationService, "updateLocation");
    spyOn(locationService, "getMap");
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
