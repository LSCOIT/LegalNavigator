import { TestBed, inject, ComponentFixture } from '@angular/core/testing';
import { LocationService } from './location.service';
import { MapLocation } from './location';

describe('LocationService', () => {
  let service: LocationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LocationService]
    });
  });

  beforeEach(() => {
    service = new LocationService();
  });

  it('should create location service', inject([LocationService], (service: LocationService) => {
    expect(service).toBeTruthy();
  }));

});
