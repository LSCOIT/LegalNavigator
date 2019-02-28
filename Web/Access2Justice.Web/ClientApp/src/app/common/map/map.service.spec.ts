import { TestBed } from "@angular/core/testing";

import {ENV} from 'environment';
import { StateCodeService } from "../services/state-code.service";
import { MapService } from "./map.service";

describe("MapService", () => {
  let service: MapService;
  let mockStateCodeService: StateCodeService;
  let mockMapType: boolean = false;
  const mockMapLocation = {
    state: "California",
    city: "Riverside County",
    county: "Indio",
    zipCode: "92201"
  };
  const mockDisplayLocationDetails = {
    locality: "Indio",
    address: "92201"
  };
  const mockLocationDetails = {
    location: mockMapLocation,
    displayLocationDetails: mockDisplayLocationDetails,
    country: "United States",
    formattedAddress: "Hjorth St, Indio, California 92201, United States"
  };
  const mockLocation = {
    address: {
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
      providers: [MapService, StateCodeService]
    });
    service = new MapService(mockStateCodeService);

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

    spyOn(sessionStorage, "setItem").and.callFake(mockSessionStorage.setItem);
    spyOn(sessionStorage, "removeItem").and.callFake(
      mockSessionStorage.removeItem
    );
    spyOn(sessionStorage, "clear").and.callFake(mockSessionStorage.clear);
  });

  it("should create location service", () => {
    expect(service).toBeTruthy();
  });

  it("should define location service", () => {
    expect(service).toBeDefined();
  });

  it("should assign mapType value in environment map type variable", () => {
    spyOn(service, "getMap");
    mockMapType = true;
    service.getMap(mockMapType);
    expect(service.getMap).toHaveBeenCalled();
  });

  it("should call only setGlobalMapLocationDetails when updateLocation is called for global map with no state details", () => {
    mockLocationDetails.location.state = "";
    spyOn(sessionStorage, "getItem").and.returnValue(
      JSON.stringify(mockLocationDetails)
    );
    spyOn(service, "setGlobalMapLocationDetails");
    ENV.map_type = true;
    service.updateLocation();
    expect(service.setGlobalMapLocationDetails).toHaveBeenCalled();
  });

  it("should set location in session in setGlobalMapLocationDetails", () => {
    service.locationDetails = mockLocationDetails;
    service.setGlobalMapLocationDetails();
    expect(sessionStorage.setItem).toHaveBeenCalled();
    expect(sessionStorage.removeItem).toHaveBeenCalled();
  });

  it("should set maplocation in setLocalMapLocationDetails", () => {
    service.locationDetails = mockLocationDetails;
    service.setLocalMapLocationDetails();
    expect(service.mapLocation).toEqual(mockLocationDetails.location);
  });

  it("should set the variables of service when mapLocationDetails is called", () => {
    service.mapLocationDetails(mockLocation);
    expect(service.mapLocation.state).toEqual(
      mockLocation.address.adminDistrict
    );
  });

  it("should set adminDistrict of map location with postal code if postal code, locality and district are undefined when mapLocationDetails is called", () => {
    let mockLocation = {
      address: {
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
    expect(service.locationDetails.displayLocationDetails.locality).toEqual(
      mockLocation.address.adminDistrict
    );
    expect(service.locationDetails.displayLocationDetails.address).toEqual(
      mockLocation.address.adminDistrict
    );
  });

  it("should set district of map location with postal code if postal code and locality are undefined when mapLocationDetails is called", () => {
    let mockLocation = {
      address: {
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
    expect(service.locationDetails.displayLocationDetails.locality).toEqual(
      mockLocation.address.district
    );
    expect(service.locationDetails.displayLocationDetails.address).toEqual(
      mockLocation.address.adminDistrict
    );
  });

  it("should set locality of map location with postal code if postal code is undefined when mapLocationDetails is called", () => {
    let mockLocation = {
      address: {
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
    expect(service.locationDetails.displayLocationDetails.locality).toEqual(
      mockLocation.address.locality
    );
    expect(service.locationDetails.displayLocationDetails.address).toEqual(
      mockLocation.address.adminDistrict
    );
  });

  it("should set locality of map location with postal code if postal code is not undefined when mapLocationDetails is called", () => {
    let mockLocation = {
      address: {
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
    expect(service.locationDetails.displayLocationDetails.locality).toEqual(
      mockLocation.address.postalCode
    );
    expect(service.locationDetails.displayLocationDetails.address).toEqual(
      mockLocation.address.adminDistrict
    );
  });

  it("should store global map location details to session storage when mapLocationDetails is called from global map(map type is true)", () => {
    ENV.map_type = true;
    service.mapLocationDetails(mockLocation);
    expect(service.locationDetails.country).toEqual(
      mockLocation.address.countryRegion
    );
    expect(service.locationDetails.formattedAddress).toEqual(
      "Indio, California 92201, United States"
    );
  });

  it("should store local map location details to session storage when mapLocationDetails is called from global map(map type is flase)", () => {
    ENV.map_type = false;
    service.mapLocationDetails(mockLocation);
    expect(service.locationDetails.country).toEqual(
      mockLocation.address.countryRegion
    );
    expect(service.locationDetails.formattedAddress).toEqual(
      "Indio, California 92201, United States"
    );
  });
});
