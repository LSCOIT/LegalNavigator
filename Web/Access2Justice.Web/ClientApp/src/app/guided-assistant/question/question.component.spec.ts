import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { QuestionComponent } from './question.component';
import { QuestionService } from './question.service';
import { ProgressbarModule, ProgressbarConfig } from 'ngx-bootstrap/progressbar';
import { Observable } from 'rxjs/Observable';

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
});
