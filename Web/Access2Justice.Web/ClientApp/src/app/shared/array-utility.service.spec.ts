import { TestBed, inject } from '@angular/core/testing';
import { ArrayUtilityService } from './array-utility.service';

describe('ArrayUtilityService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: []
    });
  });

  it('should be created', inject([ArrayUtilityService], (service: ArrayUtilityService) => {
    expect(service).toBeTruthy();
  }));
});
