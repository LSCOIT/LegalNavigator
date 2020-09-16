import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InnerQuestionComponent } from './inner-question.component';

describe('InnerQuestionComponent', () => {
  let component: InnerQuestionComponent;
  let fixture: ComponentFixture<InnerQuestionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InnerQuestionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InnerQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
