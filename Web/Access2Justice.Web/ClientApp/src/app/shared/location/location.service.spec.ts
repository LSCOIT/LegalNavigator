import { LocationService } from './location.service';
import { MapLocation } from './location';
import { environment } from '../../../environments/environment';
declare var Microsoft: any;

describe('LocationService', () => {
  let service: LocationService = new LocationService();
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

    localStorage.setItem('tempSearchedLocation', tempSearchLocation);
    localStorage.setItem('tempSearchedLocationState', tempSearchedLocationState);
    service.updateLocation();
    expect(service.displayMapLocation.address).toEqual(tempSearchedLocationState);
    expect(service.displayMapLocation.locality).toEqual(tempSearchLocation);
  });

  //it('should call Microsoft Maps load module when getMap is called', () => {
  //  // Unit test for getMap is not possible as New of Microsoft is used.
  //  let map = new Microsoft.Maps.Map('#my-map',
  //    {
  //      credentials: environment.bingmap_key
  //    });
  //  spyOn(Microsoft.Maps, 'loadModule');
  //  service.getMap();
  //  expect(Microsoft.Maps.loadModule).toHaveBeenCalled();
  //});

  //it('should have new instance for searchManager when loadSearchManager is called', () => {
  //  // Unit test for getMap is not possible as New of Microsoft is used.
  //  expect(service.searchManager).toBeUndefined();
  //  service.loadSearchManager()
  //  expect(service.searchManager).not.toBeNull();
  //});
});
