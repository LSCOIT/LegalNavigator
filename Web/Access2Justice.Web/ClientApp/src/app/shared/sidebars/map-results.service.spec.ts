import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MapResultsService } from './map-results.service';
import { environment } from '../../../environments/environment';

describe('MapResults Service', () => {
  let service: MapResultsService;
  const httpSpy = jasmine.createSpyObj('http', ['get']);
  
  beforeEach(() => {
    service = new MapResultsService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('should create map Results service', () => {
    expect(service).toBeTruthy();
  });

  it('should define map Results service', () => {
    expect(service).toBeDefined();
  });

  //it('should call getLocationDetails', (done) => {
  //  let address = "Alaska, United States";
  //  let credentials = environment.bingmap_key;

  // //let mockRequest = 'https://dev.virtualearth.net/REST/v1/Locations/' + encodeURI(address) + '?output=json&key=' + credentials;
  //  //const mockResponse = Observable.of(mockTopics);

  //  //httpSpy.get.and.returnValue(mockResponse);
  //  spyOn(service.getLocationDetails(address, credentials));
  //  expect(service.getLocationDetails(address, credentials)).toHaveBeenCalled();
  //  //service.getLocationDetails(address, credentials).subscribe(result => {
  //  //  expect(result).toEqual(expect(httpSpy.get).toHaveBeenCalledWith(`${mockRequest}`));
  //  //  done();
  //  //});
  //});

});
