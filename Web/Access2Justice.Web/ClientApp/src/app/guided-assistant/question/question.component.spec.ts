import { FormsModule, NgForm } from '@angular/forms';
import { QuestionComponent } from './question.component';
import { of } from 'rxjs/observable/of';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionService } from './question.service';
import { HttpParams } from '@angular/common/http';
import { NavigateDataService } from '../../shared/navigate-data.service';

describe('QuestionComponent', () => {
  let component: QuestionComponent;
  let fixture: ComponentFixture<QuestionComponent>;
  let mockQuestionService;
  let mockRouter;
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
    buttons: [
      {
        id: "111",
        label: "Continue",
        destination: ""
      }
    ],
    fields: []
  }
  let mockNavigateDataService;

  beforeEach(async(() => { 
    mockQuestionService = jasmine.createSpyObj(['getQuestion', 'getNextQuestion']);
    mockNavigateDataService = jasmine.createSpyObj((['setData']));

    TestBed.configureTestingModule({
      imports: [ FormsModule ],
      declarations: [ QuestionComponent ],
      schemas: [ NO_ERRORS_SCHEMA ],
      providers: [ 
        { provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                'id': '123'
              }
            }
          }
        },
        { provide: QuestionService, useValue: mockQuestionService },
        { provide: Router, useValue: mockRouter },
        { provide: NavigateDataService, useValue: mockNavigateDataService }
      ]
    })
    .compileComponents();
  }));
  
  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionComponent);
    component = fixture.componentInstance;
  });
  
  it('should get initial set of question and send id as param', () => {
    mockQuestionService.getQuestion.and.returnValue(of(mockQuestion));
    component.ngOnInit();
    component.getQuestion();
    expect(component.curatedExperienceId).toBe("123");
    spyOn(component, 'sendTotalQuestions');
    expect(component.question).toEqual(mockQuestion);
    let params = new HttpParams()
      .set("curatedExperienceId", component.curatedExperienceId);
    expect(mockQuestionService.getQuestion).toHaveBeenCalledWith(params);
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

  it('should call getNextQuestion if remaining question is greater than 1', () => {
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
});
