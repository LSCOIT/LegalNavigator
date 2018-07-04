import { TestBed, inject } from '@angular/core/testing';

import { PersonalizedPlanService } from './personalized-plan.service';

describe('PersonalizedPlanService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PersonalizedPlanService]
    });
  });

  it('should be created', inject([PersonalizedPlanService], (service: PersonalizedPlanService) => {
    expect(service).toBeTruthy();
  }));
});
