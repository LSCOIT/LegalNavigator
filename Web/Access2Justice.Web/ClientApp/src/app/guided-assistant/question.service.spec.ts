import { TestBed, inject } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { guidedAssistantApi } from '../../api/api';
import { QuestionService } from './question.service';
import { Question } from './question';
import { Observable } from 'rxjs/Rx';

describe('QuestionService', () => {
  let service: QuestionService;
  let sampleQuestion: Question = {
    title: 'Question Title',
    questionId: 'Question1',
    questionType: 'Question Type',
    userAnswer: 'Question User Answer',
    choices: ['Question choices']
  };
  let mockResponse = Observable.of(sampleQuestion);

  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);

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

  it('should create question service', inject([QuestionService], (service: QuestionService) => {
    expect(service).toBeTruthy();
  }));

  it('should return list of questions', (done) => {
    httpSpy.get.and.returnValue(mockResponse);
    service.getQuestion().subscribe(question => {
      expect(httpSpy.get).toHaveBeenCalledWith(`${guidedAssistantApi.questionUrl}`);
      expect(question).toEqual(sampleQuestion);
      done();
    });
  });

  it('should return next question', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    service.getNextQuestion("New Param").subscribe(question => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(question).toEqual(sampleQuestion);
      done();
    });
  });

  it('should return previous question', (done) => {
    httpSpy.post.and.returnValue(mockResponse);
    service.getPrevQuestion("New Param").subscribe(question => {
      expect(httpSpy.post).toHaveBeenCalled();
      expect(question).toEqual(sampleQuestion);
      done();
    });
  });
});
