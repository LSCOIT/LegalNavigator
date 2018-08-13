import { TestBed, inject } from '@angular/core/testing';

import { ArrayUtilityService } from '../shared/array-utility.service';

describe('UtilityService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ArrayUtilityService]
    });
  });

  it('should be created', inject([ArrayUtilityService], (service: ArrayUtilityService) => {
    expect(service).toBeTruthy();
  }));
});
