import { TestBed } from '@angular/core/testing';
import { LocationService } from './location.service';
import { environment } from '../../../environments/environment';

describe('LocationService', () => {
  let service: LocationService;
  let mockMapType: boolean = false;
  const mockMapLocation = {
    state: "California",
    city: "Riverside County",
    county: "Indio",
    zipCode: "92201",
    locality: "Indio",
    address: "92201"
  };

  const mockLocation = {
    address:
      {
        addressLine: "Hjorth St",
        adminDistrict: "California",
        countryRegion: "United States",
        countryRegionISO2: "US",
        district: "Riverside County",
        formattedAddress: "Hjorth St, Indio, California 92201, United States",
        locality: "Indio",
        postalCode: "92201"
      }
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LocationService]
    });
    service = new LocationService();

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

  it('should create location service', () => {
    expect(service).toBeTruthy();
  });

  it('should define location service', () => {
    expect(service).toBeDefined();
  });

  it('should assign mapType value in environment map type variable', () => {
    spyOn(service, 'getMap');
    mockMapType = true;
    service.getMap(mockMapType);
    expect(service.getMap).toHaveBeenCalled();
  });

  it('should return searched global location details(map type is true) from session storage when updateLocation is called for global map', () => {
    sessionStorage.setItem("globalSearchMapLocation", JSON.stringify(mockMapLocation));
    environment.map_type = true;
    service.updateLocation();
    expect(service.mapLocation).toEqual(mockMapLocation);
  });

  it('should store searched global location details(map type is true) to session when updateLocation is called for global map', () => {
    sessionStorage.setItem("globalSearchMapLocation", JSON.stringify(mockMapLocation));
    environment.map_type = true;
    service.updateLocation();
    expect(service.mapLocation).toEqual(mockMapLocation);
    sessionStorage.setItem("mockGlobalMapLocation", JSON.stringify(mockMapLocation));
    expect(JSON.parse(sessionStorage.getItem("mockGlobalMapLocation"))).toEqual(mockMapLocation);
  });

  it('should clear searched global location details from session storage when updateLocation is called for global map', () => {
    sessionStorage.setItem("globalSearchMapLocation", JSON.stringify(mockMapLocation));
    environment.map_type = true;
    service.updateLocation();
    expect(JSON.parse(sessionStorage.getItem("globalSearchMapLocation"))).toBeNull();
  });

  it('should return searched local location details(map type is false) from session storage when updateLocation is called for local map', () => {
    sessionStorage.setItem("localSearchMapLocation", JSON.stringify(mockMapLocation));
    environment.map_type = false;
    service.updateLocation();
    expect(service.mapLocation).toEqual(mockMapLocation);
  });

  it('should clear searched local location details from session storage when updateLocation is called for global map', () => {
    sessionStorage.setItem("localSearchMapLocation", JSON.stringify(mockMapLocation));
    environment.map_type = false;
    service.updateLocation();
    expect(JSON.parse(sessionStorage.getItem("localSearchMapLocation"))).toBeNull();
  });

  it("should set the variables of service when mapLocationDetails is called", () => {
    service.mapLocationDetails(mockLocation);
    expect(service.mapLocation.state).toEqual(mockLocation.address.adminDistrict);
    expect(service.mapLocation.county).toEqual(mockLocation.address.district);
    expect(service.mapLocation.city).toEqual(mockLocation.address.locality);
    expect(service.mapLocation.zipCode).toEqual(mockLocation.address.postalCode);
  });

  it("should set adminDistrict of map location with postal code if postal code, locality and district are undefined when mapLocationDetails is called", () => {
    let mockLocation = {
      address:
        {
          addressLine: "Hjorth St",
          adminDistrict: "California",
          countryRegion: "United States",
          countryRegionISO2: "US",
          district: undefined,
          formattedAddress: "Hjorth St, Indio, California 92201, United States",
          locality: undefined,
          postalCode: undefined
        }
    };
    service.mapLocationDetails(mockLocation);
    expect(service.mapLocation.locality).toEqual(mockLocation.address.adminDistrict);
    expect(service.mapLocation.address).toEqual(mockLocation.address.adminDistrict);
  });

  it("should set district of map location with postal code if postal code and locality are undefined when mapLocationDetails is called", () => {
    let mockLocation = {
      address:
        {
          addressLine: "Hjorth St",
          adminDistrict: "California",
          countryRegion: "United States",
          countryRegionISO2: "US",
          district: "Riverside County",
          formattedAddress: "Hjorth St, Indio, California 92201, United States",
          locality: undefined,
          postalCode: undefined
        }
    };
    service.mapLocationDetails(mockLocation);
    expect(service.mapLocation.locality).toEqual(mockLocation.address.district);
    expect(service.mapLocation.address).toEqual(mockLocation.address.adminDistrict);
  });

  it("should set locality of map location with postal code if postal code is undefined when mapLocationDetails is called", () => {
    let mockLocation = {
      address:
        {
          addressLine: "Hjorth St",
          adminDistrict: "California",
          countryRegion: "United States",
          countryRegionISO2: "US",
          district: "Riverside County",
          formattedAddress: "Hjorth St, Indio, California 92201, United States",
          locality: "Indio",
          postalCode: undefined
        }
    };
    service.mapLocationDetails(mockLocation);
    expect(service.mapLocation.locality).toEqual(mockLocation.address.locality);
    expect(service.mapLocation.address).toEqual(mockLocation.address.adminDistrict);
  });

  it("should set locality of map location with postal code if postal code is not undefined when mapLocationDetails is called", () => {
    let mockLocation = {
      address:
        {
          addressLine: "Hjorth St",
          adminDistrict: "California",
          countryRegion: "United States",
          countryRegionISO2: "US",
          district: "Riverside County",
          formattedAddress: "Hjorth St, Indio, California 92201, United States",
          locality: "Indio",
          postalCode: "92201"
        }
    };
    service.mapLocationDetails(mockLocation);
    expect(service.mapLocation.locality).toEqual(mockLocation.address.postalCode);
    expect(service.mapLocation.address).toEqual(mockLocation.address.adminDistrict);
  });

  it("should store global map location details to session storage when mapLocationDetails is called from global map(map type is true)", () => {
    environment.map_type = true;
    service.mapLocationDetails(mockLocation);
    expect(JSON.parse(sessionStorage.getItem("globalSearchMapLocation"))).toEqual(service.mapLocation);
  });

  it("should store local map location details to session storage when mapLocationDetails is called from global map(map type is flase)", () => {
    environment.map_type = false;
    service.mapLocationDetails(mockLocation);
    expect(JSON.parse(sessionStorage.getItem("localSearchMapLocation"))).toEqual(service.mapLocation);
  });

});
