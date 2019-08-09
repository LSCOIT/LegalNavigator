import { TestBed } from '@angular/core/testing';

import { GuidUtilityService } from './guid-utility.service';

describe('GuidUtilityService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GuidUtilityService = TestBed.get(GuidUtilityService);
    expect(service).toBeTruthy();
  });

  it('should be get right guid', () => {
    const url = "/resource/60e3c9c1-8dc8-4265-a899-f93860811ed9";
    const expectedResult = "60e3c9c1-8dc8-4265-a899-f93860811ed9";
    const service: GuidUtilityService = TestBed.get(GuidUtilityService);
    expect(service.getGuidFromResourceUrl(url)).toBe(expectedResult);
  });

});
