import { NgForm } from '@angular/forms';
import { QuestionComponent } from './question.component';
import { of } from 'rxjs/observable/of';
import { Observable } from 'rxjs';

describe('QuestionComponent', () => {
  let component: QuestionComponent;
  let mockQuestionService;
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
    buttons: [{
      id: "111",
      label: "Continue",
      destination: ""
    }],
    fields: []
  }

  beforeEach(() => {
    mockQuestionService = jasmine.createSpyObj(['getQuestion', 'getNextQuestion']);
    component = new QuestionComponent(mockQuestionService, undefined, undefined);

  });

  it('should get initial set of question', () => {
    mockQuestionService.getQuestion.and.returnValue(of(mockQuestion));
    spyOn(component, 'sendTotalQuestions');
    component.getQuestion();
    expect(component.question).toEqual(mockQuestion);
    expect(component.sendTotalQuestions).toHaveBeenCalled();
  });

  it('should map input value to correct parameter', () => {
      let formValue = {
        value: {
          "111": "abc",
          "222": "def",
          "333": "ghi"
        }
      };
      let mockFieldParam = [
        { "fieldId": "111", "value": "abc" },
        { "fieldId": "222", "value": "def" },
        { "fieldId": "333", "value": "ghi" }
      ];
      component.createFieldParam(formValue);
      expect(component.fieldParam).toEqual(mockFieldParam);
  });

  it('should map radio value to crrect parameter', () => {
    let formValue = {
      value: {
        "radioOptions": "111"
      }
    };
    let mockFieldParam = [{ "fieldId": "111" }];
    component.createFieldParam(formValue);
    expect(component.fieldParam).toEqual(mockFieldParam);
  });

  it('should call getNextQuestion if remaining question is greater than 0', () => {
    spyOn(component, 'sendQuestionsRemaining');
    let formValue = <NgForm>{
      value: {
        "111": "abc",
        "222": "def",
        "333": "ghi"
      }
    };
    mockQuestionService.getNextQuestion.and.returnValue(of(mockQuestion));
    component.question = mockQuestion;
    component.onSubmit(formValue);

    expect(mockQuestionService.getNextQuestion).toHaveBeenCalledWith({
      "curatedExperienceId": "123",
      "answersDocId": "456",
      "buttonId": "111",
      "fields": [
        { "fieldId": "111", "value": "abc" },
        { "fieldId": "222", "value": "def" },
        { "fieldId": "333", "value": "ghi" }
      ]
    });

    expect(component.sendQuestionsRemaining).toHaveBeenCalled();
  });

  it('should call getNextQuestion if remaining question is equal 0', () => {
    spyOn(component, 'sendQuestionsRemaining');
    let formValue = <NgForm>{
      value: {
        "111": "abc",
        "222": "def",
        "333": "ghi"
      }
    };
    mockQuestion.questionsRemaining = 0;
    mockQuestionService.getNextQuestion.and.returnValue(of(mockQuestion));
    component.question = mockQuestion;
    component.onSubmit(formValue);

    expect(mockQuestionService.getNextQuestion).toHaveBeenCalledWith({
      "curatedExperienceId": "123",
      "answersDocId": "456",
      "buttonId": "111",
      "fields": [
        { "fieldId": "111", "value": "abc" },
        { "fieldId": "222", "value": "def" },
        { "fieldId": "333", "value": "ghi" }
      ]
    });

    expect(component.sendQuestionsRemaining).toHaveBeenCalled();
    expect(component.question.text).toContain("Thanks for answering the questions");
    expect(component.question.questionsRemaining).toBe(-1);
    expect(component.question.fields).toEqual([]);
  });
});
