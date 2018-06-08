import { CuratedResourceService } from './curated-resource.service';
import { TestBed, inject } from '@angular/core/testing';

describe('CuratedResourceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CuratedResourceService]
    });
  });

  it('should be created', inject([CuratedResourceService], (service: CuratedResourceService) => {
    expect(service).toBeTruthy();
  }));
});
