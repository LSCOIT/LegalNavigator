import { TestBed, inject } from '@angular/core/testing';

import { StaticResourceService } from './static-resource.service';

describe('StaticResource', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [StaticResourceService]
    });
  });

  it('should be created', inject([StaticResourceService], (service: StaticResourceService) => {
    expect(service).toBeTruthy();
  }));
});
