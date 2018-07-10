import { TestBed, inject } from '@angular/core/testing';

import { UpperNavService } from './upper-nav.service';

describe('UpperNavService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UpperNavService]
    });
  });

  it('should be created', inject([UpperNavService], (service: UpperNavService) => {
    expect(service).toBeTruthy();
  }));
});
