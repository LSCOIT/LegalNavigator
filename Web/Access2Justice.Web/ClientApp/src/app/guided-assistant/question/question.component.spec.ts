import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { QuestionComponent } from './question.component';
import { QuestionService } from './question.service';
import { ProgressbarModule, ProgressbarConfig } from 'ngx-bootstrap/progressbar';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { Question } from './question';

const sampleQuestion: Question = {
  title: "sample question",
  questionId: "ABDC",
  questionType: "singleselection",
  choices: [],
  userAnswer: "ASDF"
};

const sampleSingleSelectionQuestion: Question = {
  title: "single selection question",
  questionId: "ABDC",
  questionType: "singleselection",
  choices: [],
  userAnswer: "ASDF"
};

const sampleMultipleSelectionQuestion: Question = {
  title: "multiple selection question",
  questionId: "ABDC",
  questionType: "multipleselection",
  choices: [],
  userAnswer: "ASDF"
};

const sampleYesNoQuestion: Question = {
  title: "yes no type question",
  questionId: "ABDC",
  questionType: "yesnotype",
  choices: [],
  userAnswer: "ASDF"
};

const mockQuestionService = {
  getQuestion: () => { },
  getNextQuestion: () => { return Observable.of(); }

};

describe('QuestionComponent', () => {
  let component: QuestionComponent;
  let fixture: ComponentFixture<QuestionComponent>;
  let questionService: QuestionService;
 
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [QuestionComponent],
      imports: [
        FormsModule,
        HttpClientModule,
        ProgressbarModule
      ],
      providers: [
        { provide: QuestionService, useValue: mockQuestionService },
        ProgressbarConfig]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionComponent);
    component = fixture.componentInstance;
    questionService = fixture.debugElement.injector.get(QuestionService);
  });

  it('should create question component', () => {
    expect(component).toBeTruthy();
  });

  it('should create method get Question', () => {
    spyOn(questionService, "getQuestion").and.callFake((): Observable<Question> => {
      return of(sampleQuestion);
    });
    component.getQuestion();
    expect(mockQuestionService.getQuestion).toBeDefined();
  })

  //it('should create method get Next Question', () => {
  //  spyOn(questionService, "getNextQuestion").and.callFake((): Observable<Question> => {
  //    return of(sampleQuestion);
  //  });
  //  expect(mockQuestionService.getNextQuestion).toBeDefined();
  //})

  //it('should make a call to service get question', () => {
  //  spyOn(questionService, "getQuestion")
  //    .and.returnValue(Observable.of());
  //  fixture.detectChanges();
  //  expect(questionService.getQuestion).toHaveBeenCalled();
  //});

  //it('should increment ques counter for singleselection questionType', async(() => {
  //  let value: any = { listOptions: null };
  //  let empForm: any = { value };
  //  spyOn(questionService, "getQuestion").and.callFake((): Observable<Question> => {
  //    return of(sampleSingleSelectionQuestion);
  //  });
  //  component.question = sampleSingleSelectionQuestion;
  //  component.onSubmit(empForm);
  //  expect(component.ques).toEqual(1);
  //}));

  //it('should increment ques counter for multipleselection questionType', async(() => {
  //  let value: any = { checkOptions: null };
  //  let empForm: any = { value };
  //  spyOn(questionService, "getQuestion").and.callFake((): Observable<Question> => {
  //    return of(sampleMultipleSelectionQuestion);
  //  });
  //  component.question = sampleMultipleSelectionQuestion;
  //  component.onSubmit(empForm);
  //  expect(component.ques).toEqual(1);
  //}));

  //it('should increment ques counter for yesno questionType', async(() => {
  //  let value: any = { checkOptions: null };
  //  let empForm: any = { value };
  //  spyOn(questionService, "getQuestion").and.callFake((): Observable<Question> => {
  //    return of(sampleYesNoQuestion);
  //  });
  //  component.question = sampleYesNoQuestion;
  //  component.onSubmit(empForm);
  //  expect(component.ques).toEqual(1);
  //}));

  //it('should increment progress bar on answering question', async(() => {
  //  let value: any = { checkOptions: null };
  //  let empForm: any = { value };
  //  spyOn(questionService, "getQuestion").and.callFake((): Observable<Question> => {
  //    return of(sampleYesNoQuestion);
  //  });
  //  component.question = sampleYesNoQuestion;
  //  component.onSubmit(empForm);
  //  expect(component.dynamic).toEqual(20);
  //}));

  //it('should call on Validate Element on Checkbox selection', async(() => {
  //  let htmlInputElement: any = { id: 'GUID', name: "checkOptions", checked: true };
  //  spyOn(component, "validateElement").and.callFake((): Observable<Question> => {
  //    return of(htmlInputElement);
  //  });
  //}));

  //it('should call on Validate Element on button click', async(() => {
  //  let htmlInputElement: any = { id: 'GUID', name: "btnOptions", checked: true };
  //  spyOn(component, "validateElement").and.callFake((): Observable<Question> => {
  //    return of(htmlInputElement);
  //  });
  //}));

  //it('should call on Validate Element on Checkbox selection', async(() => {
  //  let htmlInputElement: any = { id: 'GUID', name: "otherOptions", checked: true };
  //  spyOn(component, "validateElement").and.callFake((): Observable<Question> => {
  //    return of(htmlInputElement);
  //  });
  //}));

  //it('should validate Checkbox for checkOptions', async(() => {
  //  let htmlInputElement: any = { id: 'GUID', name: "checkOptions", checked: true };
  //  component.question = sampleMultipleSelectionQuestion;
  //  component.validateElement(htmlInputElement);
  //  expect(htmlInputElement.id).toEqual(component.answer.answers.shift());
  //}));

  //it('should validate button click for btnOptions', async(() => {
  //  let htmlInputElement: any = { id: 'GUID', name: "btnOptions", checked: true };
  //  component.question = sampleYesNoQuestion;
  //  component.validateElement(htmlInputElement);
  //  expect(htmlInputElement.id).toEqual(component.answer.answers.shift());
  //}));

});
