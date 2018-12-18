import { TestBed, inject } from '@angular/core/testing';

import { IntakeQuestionServiceService } from './intake-question-service.service';

describe('IntakeQuestionServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [IntakeQuestionServiceService]
    });
  });

  it('should be created', inject([IntakeQuestionServiceService], (service: IntakeQuestionServiceService) => {
    expect(service).toBeTruthy();
  }));
});
