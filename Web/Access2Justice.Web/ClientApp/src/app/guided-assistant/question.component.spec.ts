import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { QuestionComponent } from './question.component';
import { QuestionService } from './question.service';

describe('QuestionComponent', () => {
  let component: QuestionComponent;
  let fixture: ComponentFixture<QuestionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [QuestionComponent],
      imports: [
        FormsModule,
        HttpClientModule
      ],
      providers: [QuestionService]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  //it('should', async(() => {
  //  spyOn(component, 'onSubmit');

  //  let button = fixture.debugElement.nativeElement.querySelector('button');
  //  button.click();

  //  fixture.whenStable().then(() => {
  //    expect(component.onSubmit).toHaveBeenCalled();
  //  })
  //}));
});
