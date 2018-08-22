import { TestBed, inject } from '@angular/core/testing';

import { StaticContentDataService } from './static-content-data.service';

describe('StaticContentDataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [StaticContentDataService]
    });
  });

  it('should be created', inject([StaticContentDataService], (service: StaticContentDataService) => {
    expect(service).toBeTruthy();
  }));
});
