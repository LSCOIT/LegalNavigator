import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { IntakeQuestionService } from './intake-question.service';

describe('IntakeQuestionService', () => {
  let service: IntakeQuestionService;
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);

  beforeEach(() => {
    service = new IntakeQuestionService(httpSpy);
    httpSpy.get.calls.reset();
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [IntakeQuestionService]
    });
  });

  it('should be created', inject([IntakeQuestionService], (service: IntakeQuestionService) => {
    expect(service).toBeTruthy();
  }));
});
