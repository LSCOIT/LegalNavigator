import { HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, TemplateRef } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { MsalService } from '@azure/msal-angular';
import { BsModalRef, ModalModule } from 'ngx-bootstrap';
import { BsModalService } from 'ngx-bootstrap/modal';
import { from } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Global } from '../../global';
import { EventUtilityService } from '../services/event-utility.service';
import { StaticResourceService } from '../services/static-resource.service';
import { MapResultsService } from '../sidebars/map-results/map-results.service';
import { MapLocation } from './map';
import { MapComponent } from './map.component';
import { MapService } from './map.service';

describe('MapComponent', () => {
  let component: MapComponent;
  let fixture: ComponentFixture<MapComponent>;
  let modalService: BsModalService;
  let template: TemplateRef<any>;
  let mockMapLocation: MapLocation = {
    state: 'Sample State',
    city: 'Sample City',
    county: 'Sample County',
    zipCode: '1009203'
  };
  let mapResultsService: MapResultsService;
  let mockGeolocationPositionLangitude: '77.33817429999999';
  let mockGeolocationPositionLatiitude: '28.5372834';
  let mockBingKey = 'testAqpcQxjuTmheUzCm8b5kUhV9UhjfsK66Ctest';
  let mockMapLocation2 = {
    state: 'UP',
    city: 'Noida',
    zipCode: '201303',
    locality: '201303',
    address: 'UP'
  };
  let mockMapLocationWithFormatAddressGlobal = {
    state: 'UP',
    city: 'Sample City',
    county: 'Sample County',
    zipCode: '1009203'
  };
  let mockMapLocationWithFormatAddressLocal = {
    state: 'Sample State',
    city: 'Sample City',
    county: 'Sample County',
    zipCode: '1009203'
  };
  let mockDisplayLocationDetails = {
    locality: 'UP',
    address: 'UP'
  };
  let mockLocationDetailsWithFormatAddress = {
    location: mockMapLocation,
    displayLocationDetails: mockDisplayLocationDetails,
    country: 'United States',
    formattedAddress: 'AK'
  };
  let mockMapResultsLocation = {
    resourceSets: [
      {
        resources: [
          {
            name: 'UP'
          }
        ]
      }
    ]
  };
  let mockLocationDetailsWithOutFormatAddress = {
    location: mockMapLocation,
    country: 'United States',
    formattedAddress: 'Hjorth St, Indio, California 92201, United States'
  };
  let mapService, msalService;
  let returnLocationValue = {
    value: 'Hawaii'
  };
  let modalRefInstance: jasmine.SpyObj<BsModalRef>;

  beforeEach(() => {
    mapService = jasmine.createSpyObj([
      'getMap',
      'updateLocation',
      'mapLocationDetails',
      'identifyLocation'
    ]);
    msalService = jasmine.createSpyObj(['getUser']);

    TestBed.configureTestingModule({
      imports: [
        HttpClientModule,
        RouterTestingModule,
        ModalModule.forRoot(),
        FormsModule
      ],
      declarations: [MapComponent],
      providers: [
        BsModalService,
        MapResultsService,
        StaticResourceService,
        Global,
        EventUtilityService,
        {
          provide: MsalService,
          useValue: msalService
        },
        {
          provide: MapService,
          useValue: mapService
        }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
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

    spyOn(sessionStorage, 'setItem').and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, 'removeItem').and.callFake(
      mockSessionStorage.removeItem
    );
    spyOn(sessionStorage, 'clear').and.callFake(mockSessionStorage.clear);

    modalRefInstance = jasmine.createSpyObj('BsModalRef', ['hide']);
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('should define component', () => {
    expect(component).toBeDefined();
  });

  it('should call displayLocationDetails on ngInit', () => {
    spyOn(component, 'displayLocationDetails');
    spyOn(component, 'ngOnInit');
    component.displayLocationDetails(mockMapLocation);
    expect(component.displayLocationDetails).toHaveBeenCalled();
  });

  it('should call modalService show when openModal is called', () => {
    const testElement = document.createElement('div');
    document.getElementById = jasmine
      .createSpy('HTML Element')
      .and.returnValue(testElement);
    spyOn(modalService, 'show');
    spyOn(document.getElementById('search-box'), 'focus');
    component.openModal(template);
    expect(modalService.show).toHaveBeenCalled();
    expect(document.getElementById('search-box').focus).toBeDefined();
  });

  it('should call mapService getMap when openModal is called', () => {
    component.openModal(template);
    expect(mapService.getMap).toHaveBeenCalled();
  });

  it('should call update location of location service when location error is false', () => {
    component.modalRef = modalRefInstance;
    component.locationError = false;
    spyOn(component, 'getLocationDetails');
    component.updateLocation();
    expect(component.isError).toBeFalsy();
    expect(component.locationError).toBeUndefined();
  });

  it('should call geocode when showlocation is true in updateLocation method', () => {
    spyOn(component, 'geocode');
    spyOn(sessionStorage, 'getItem');
    component.locationError = true;
    component.updateLocation();
    expect(component.geocode).toHaveBeenCalled();
  });

  it('should call getStateFullName mapresults service method with formataddress for global map', () => {
    spyOn(component, 'updateLocationDetails');
    spyOn(mapResultsService, 'getStateFullName').and.callFake(() => {
      return from([mockMapResultsLocation]);
    });
    spyOn(sessionStorage, 'getItem').and.returnValue(
      JSON.stringify(mockLocationDetailsWithFormatAddress)
    );
    environment.map_type = true;
    component.getLocationDetails();
    expect(mapResultsService.getStateFullName).toHaveBeenCalled();
    expect(component.locationDetails.location).toEqual(
      mockMapLocationWithFormatAddressGlobal
    );
    expect(component.locationDetails.displayLocationDetails).toEqual(
      mockDisplayLocationDetails
    );
    expect(component.updateLocationDetails).toHaveBeenCalled();
  });

  it('should call getStateFullName mapresults service method with formataddress for local map', () => {
    spyOn(component, 'updateLocationDetails');
    spyOn(mapResultsService, 'getStateFullName').and.callFake(() => {
      return from([mockMapResultsLocation]);
    });
    spyOn(sessionStorage, 'getItem').and.returnValue(
      JSON.stringify(mockLocationDetailsWithFormatAddress)
    );
    environment.map_type = false;
    component.getLocationDetails();
    expect(mapResultsService.getStateFullName).toHaveBeenCalled();
    expect(component.locationDetails.location).toEqual(
      mockMapLocationWithFormatAddressLocal
    );
    expect(component.locationDetails.displayLocationDetails).toEqual(
      mockDisplayLocationDetails
    );
    expect(component.updateLocationDetails).toHaveBeenCalled();
  });

  it('should call updateLocationDetails method without formataddress', () => {
    spyOn(component, 'updateLocationDetails');
    spyOn(mapResultsService, 'getStateFullName').and.callFake(() => {
      return from([mockMapResultsLocation]);
    });
    spyOn(sessionStorage, 'getItem').and.returnValue(
      JSON.stringify(mockLocationDetailsWithOutFormatAddress)
    );
    environment.map_type = true;
    component.getLocationDetails();
    expect(component.updateLocationDetails).toHaveBeenCalled();
  });

  it('should call hide of modal ref when updateLocationDetails of component is called', () => {
    component.modalRef = modalRefInstance;
    component.mapType = false;
    component.changeLocationButton = {
      nativeElement: jasmine.createSpyObj('nativeElement', ['focus'])
    };
    mapService.updateLocation.and.returnValue(
      mockLocationDetailsWithFormatAddress
    );
    spyOn(component, 'displayLocationDetails');
    component.updateLocationDetails();
    expect(
      component.changeLocationButton.nativeElement.focus
    ).toHaveBeenCalled();
    expect(modalRefInstance.hide).toHaveBeenCalled();
    expect(component.showLocality).toBeFalsy();
  });

  it('should call displayLocationDetails when updateLocation is called', () => {
    component.modalRef = modalRefInstance;
    let mockLocation = {
      location: null,
      country: 'United States',
      formattedAddress: 'Hjorth St, Indio, California 92201, United States'
    };
    mapService.updateLocation.and.returnValue(mockLocation);
    component.mapType = true;
    spyOn(component, 'displayLocationDetails');
    component.updateLocationDetails();
    expect(component.isError).toBeTruthy();
  });

  it('should set the address,locality and showLocation variables of component when displayLocationDetails is called', () => {
    spyOn(modalService, 'hide');
    spyOn(component, 'setDisplayLocationDetails');
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

  it('should set the flag true on searchChange', () => {
    component.isError = false;
    component.onSearchChange();
    expect(component.isError).toBe(false);
  });

  it('should set the flag false on searchChange', () => {
    component.isError = true;
    component.onSearchChange();
    expect(component.isError).toBe(false);
  });

  it('should load current location', () => {
    spyOn(component, 'loadCurrentLocation');
    expect(component.loadCurrentLocation).toBeTruthy();
  });

  it('should load all components for current location - getAddressBasedOnPoints', () => {
    spyOn(navigator.geolocation, 'getCurrentPosition');
    spyOn(mapResultsService, 'getAddressBasedOnPoints');
    component.openModal(template);
    mapService.updateLocation.and.returnValue(
      mockLocationDetailsWithFormatAddress
    );
    component.displayLocationDetails(mockMapLocation2);
    expect(component.address).toEqual(mockMapLocation2.address);
    expect(component.locality).toEqual(mockMapLocation2.locality);
    expect(mapService.updateLocation).toBeTruthy();
    mapResultsService.getAddressBasedOnPoints(
      mockGeolocationPositionLatiitude,
      mockGeolocationPositionLangitude,
      mockBingKey
    );
    component.loadCurrentLocation();
    expect(mapResultsService.getAddressBasedOnPoints).toBeTruthy();
    expect(navigator.geolocation.getCurrentPosition).toBeTruthy();
  });

  it('should not load current location and throw error', () => {
    spyOn(navigator.geolocation, 'getCurrentPosition');
    spyOn(mapResultsService, 'getAddressBasedOnPoints');
    component.config = {
      ignoreBackdropClick: true,
      keyboard: false
    };
    navigator.geolocation.getCurrentPosition(null);
    mapResultsService.getAddressBasedOnPoints(0, 0, mockBingKey);
    component.locationInputRequired = false;
    component.loadCurrentLocation();
    component.openModal(template);
    expect(component.loadCurrentLocation).toBeTruthy();
  });

  it('should get search text while executing geocode', async(() => {
    document.getElementById = jasmine
      .createSpy('search - box')
      .and.returnValue(returnLocationValue);
    component.geocode();
    expect(component.searchLocation).toEqual(returnLocationValue.value);
  }));

  it('should call loadCurrentLocation and displayLocationDetails on ngOnInit', () => {
    spyOn(component, 'loadCurrentLocation');
    spyOn(component, 'displayLocationDetails');
    spyOn(component, 'ngOnInit');
    component.loadCurrentLocation();
    sessionStorage.setItem(
      'mockGlobalMapLocation',
      JSON.stringify(mockMapLocation)
    );
    let mockSessionMapLocation = JSON.parse(
      sessionStorage.getItem('mockGlobalMapLocation')
    );
    component.displayLocationDetails(mockSessionMapLocation);
    expect(component.loadCurrentLocation).toHaveBeenCalled();
    expect(component.displayLocationDetails).toHaveBeenCalled();
  });

  it('should load current location using geolocation current position', () => {
    spyOn(component, 'loadCurrentLocation');
    component.loadCurrentLocation();
    expect(navigator.geolocation.getCurrentPosition).toBeTruthy();
    expect(component.loadCurrentLocation).toHaveBeenCalled();
  });

  it('should load all components on current location', () => {
    spyOn(navigator.geolocation, 'getCurrentPosition');
    spyOn(mapResultsService, 'getAddressBasedOnPoints');
    spyOn(component, 'displayLocationDetails');
    spyOn(component, 'loadCurrentLocation');
    sessionStorage.setItem(
      'mockGlobalMapLocation',
      JSON.stringify(mockMapLocation)
    );
    let mockSessionMapLocation = JSON.parse(
      sessionStorage.getItem('mockGlobalMapLocation')
    );
    component.mapLocation = JSON.parse(
      sessionStorage.getItem('globalMapLocation')
    );
    component.displayLocationDetails(mockSessionMapLocation);
    expect(component.displayLocationDetails).toHaveBeenCalled();
    component.loadCurrentLocation();
    expect(navigator.geolocation.getCurrentPosition).toBeTruthy();
    expect(component.loadCurrentLocation).toHaveBeenCalled();
  });
});
