import { TestBed, inject } from '@angular/core/testing';

import { BreadCrumbService } from './breadcrumb.service';

describe('BreadCrumbService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BreadCrumbService]
    });
  });

  it('should be created', inject([BreadCrumbService], (service: BreadCrumbService) => {
    expect(service).toBeTruthy();
  }));
});
