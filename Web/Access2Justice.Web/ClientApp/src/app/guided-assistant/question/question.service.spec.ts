import { TestBed } from '@angular/core/testing';
import { QuestionService } from './question.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { api } from '../../../api/api';

describe('QuestionService', () => {
  let httpTestingController: HttpTestingController;
  let service: QuestionService;
  let mockQuestion = {
    curatedExperienceId: '123',
    answersDocId: '456',
    questionsRemaining: 9,
    componentId: '789',
    name: '',
    text: '',
    learn: '',
    help: '',
    tags: [],
    buttons: [],
    fields: [],
  }

  let mockAnswer = {
    curatedExperienceId: "123",
    answersDocId: "",
    buttonId: "",
    fields: []
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        QuestionService
      ]
    });
    httpTestingController = TestBed.get(HttpTestingController);
    service = TestBed.get(QuestionService);
  });

  describe('getQuestion', () => {
    it('should call get with the correct URL', () => {
      service.getQuestion('123').subscribe();
      const req = httpTestingController.expectOne(api.questionUrl + '?123');
      req.flush(mockQuestion);
      httpTestingController.verify();
    });
  });

  describe('getNextQuestion', () => {
    it('should call post with the correct URL', () => {
      service.getNextQuestion(mockAnswer).subscribe();
      const req = httpTestingController.expectOne(api.saveAndGetNextUrl);
      req.flush(mockQuestion);
      httpTestingController.verify();
    });
  });
});
