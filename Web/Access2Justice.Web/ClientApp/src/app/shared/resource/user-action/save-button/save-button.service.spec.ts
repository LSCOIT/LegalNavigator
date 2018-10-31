import { TestBed, inject } from '@angular/core/testing';

import { SaveButtonService } from './save-button.service';

describe('SaveButtonService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SaveButtonService]
    });
  });

  it('should be created', inject([SaveButtonService], (service: SaveButtonService) => {
    expect(service).toBeTruthy();
  }));
});
