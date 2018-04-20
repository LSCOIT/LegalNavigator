import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProblemsComponent } from './problems.component';

describe('ProblemsComponent', () => {
  let component: ProblemsComponent;
  let fixture: ComponentFixture<ProblemsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProblemsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProblemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
