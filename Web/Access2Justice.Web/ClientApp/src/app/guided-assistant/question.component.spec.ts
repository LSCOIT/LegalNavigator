import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientModule, HttpParams } from '@angular/common/http';
import { QuestionComponent } from './question.component';
import { QuestionService } from './question.service';
import { ProgressbarModule, ProgressbarConfig } from 'ngx-bootstrap/progressbar';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { Question } from './question';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';

export class MockQuestionService {
  public getQuestion(params: any): Observable<Question> {
    return of(sampleQuestion);
  }
}

const sampleQuestion: Question = {
  title: "test question",
  questionId: "ABDC",
  questionType: "aSDF",
  choices: [],
  userAnswer: "ASDF"
};

describe('QuestionComponent', () => {
  let component: QuestionComponent;
  let fixture: ComponentFixture<QuestionComponent>;
  let mockQuestionService: MockQuestionService;

  let gdForm: NgForm;

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
        QuestionService,
        ProgressbarConfig]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    mockQuestionService = new MockQuestionService();
    fixture = TestBed.createComponent(QuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });


  it('should create get question', () => {
    spyOn(mockQuestionService, "getQuestion").and.callFake((params: any): Observable<Question> => {
      return of(sampleQuestion);
    });
    component.getQuestion();
    expect(mockQuestionService.getQuestion).toBeDefined();

  })

  it('should call on Submit click', async(() => {
    fixture.whenStable().then(() => {
      expect(component.onSubmit).toHaveBeenCalled();
    })
  }));

  it('should call on Validate Checkbox', async(() => {
    fixture.whenStable().then(() => {
      expect(component.validateCheckbox).toHaveBeenCalled();
    })
  }));

});
