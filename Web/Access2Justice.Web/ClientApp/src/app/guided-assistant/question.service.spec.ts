import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { QuestionService } from './question.service';
import { Question } from './question';
import { Observable } from 'rxjs/Rx';

describe('QuestionService', () => {
  let service: QuestionService;
  const httpSpy = jasmine.createSpyObj('http', ['get']);

  beforeEach(() => {
    service = new QuestionService(httpSpy);
    httpSpy.get.calls.reset();
  });
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [QuestionService],
      imports: [HttpClientModule]
    });
  });

  it('should be created', inject([QuestionService], (service: QuestionService) => {
    expect(service).toBeTruthy();
  }));
  
});
