import { TestBed, inject } from '@angular/core/testing';

import { AdminAuthService } from './admin-auth.service';

describe('AdminAuthService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdminAuthService]
    });
  });

  it('should be created', inject([AdminAuthService], (service: AdminAuthService) => {
    expect(service).toBeTruthy();
  }));
});
