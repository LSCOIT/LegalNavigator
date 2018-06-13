import { LocationService } from './location.service';
import { MapLocation } from './location';
import { environment } from '../../../environments/environment';
declare var Microsoft: any;

describe('LocationService', () => {
  let service: LocationService;
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

  it('should have value in mapLocation when updateLocation is called', () => {
    let tempSearchLocation = 'This is tempSearchedLocation';
    let tempSearchedLocationState = 'This is tempSearchedLocationState';

    localStorage.setItem('tempGlobalSearchedLocation', tempSearchLocation);
    localStorage.setItem('tempGlobalSearchedLocationState', tempSearchedLocationState);
    service.updateLocation();
    expect(service.displayMapLocation.address).toEqual(tempSearchedLocationState);
    expect(service.displayMapLocation.locality).toEqual(tempSearchLocation);
  });
});
