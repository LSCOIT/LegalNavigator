import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { api } from '../../../api/api';
import { QuestionService } from './question.service';
import { Question } from './question';
import { Observable } from 'rxjs/Rx';

describe('QuestionService', () => {
  let service: QuestionService;

  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);

  beforeEach(() => {
    service = new QuestionService(httpSpy);
    httpSpy.get.calls.reset();
  });

  it('should create question service', inject([QuestionService], (service: QuestionService) => {
    expect(service).toBeTruthy();
  }));
});
