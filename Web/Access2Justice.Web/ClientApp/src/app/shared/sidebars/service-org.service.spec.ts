import { TestBed, inject } from '@angular/core/testing';

import { ServiceOrgService } from './service-org.service';

describe('ServiceOrgService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ServiceOrgService]
    });
  });

  it('should be created', inject([ServiceOrgService], (service: ServiceOrgService) => {
    expect(service).toBeTruthy();
  }));
});
