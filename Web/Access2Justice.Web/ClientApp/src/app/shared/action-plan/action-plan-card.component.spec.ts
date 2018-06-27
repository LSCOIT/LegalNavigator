import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ActionPlanCardComponent } from './action-plan-card.component';

describe('ActionPlanCardComponent', () => {
  let component: ActionPlanCardComponent;
  let fixture: ComponentFixture<ActionPlanCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActionPlanCardComponent ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActionPlanCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
