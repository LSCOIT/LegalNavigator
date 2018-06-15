import { LocationService } from './location.service';
import { MapLocation, DisplayMapLocation } from './location';

describe('LocationService', () => {
  let service: LocationService;
  let mockDisplayMapLocation: DisplayMapLocation;
  let mockMapLocation: MapLocation;
  let microsoftMaps = {
    Microsoft: {
      Maps: {
        loadModule: {}
      }
    }
  };

  beforeEach(() => {
    service = new LocationService();
  });

  it('should create location service', () => {
    expect(service).toBeTruthy();
  });

  it('should define location service', () => {
    expect(service).toBeDefined();
  });

  it('should have value in mapLocation and display map location when updateLocation is called', () => {
    mockDisplayMapLocation = {
      locality: "Sample locality",
      address: "Sample address"
    }
    mockMapLocation = {
      state: "Sample state",
      city: "Sample city",
      county: 'Sample county',
      zipCode: 'Sample zip code'
    };

    sessionStorage.setItem('mockTempGlobalDisplayMapLocation', JSON.stringify(mockDisplayMapLocation));
    sessionStorage.setItem('mockTempGlobalMapLocation', JSON.stringify(mockMapLocation));
    service.updateLocation();
    expect(service.displayMapLocation).toEqual(mockDisplayMapLocation);
    expect(service.mapLocation).toEqual(mockMapLocation);
  });
  
});
