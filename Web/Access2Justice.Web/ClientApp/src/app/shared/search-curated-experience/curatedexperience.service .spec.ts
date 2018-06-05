import { CuratedExperienceService } from './curatedexperience.service';
import { TestBed, inject } from '@angular/core/testing';

describe('CuratedExperienceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CuratedExperienceService]
    });
  });

  it('should be created', inject([CuratedExperienceService], (service: CuratedExperienceService) => {
    expect(service).toBeTruthy();
  }));
});
