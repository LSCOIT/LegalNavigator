import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActionPlanCardComponent } from './action-plan-card.component';

describe('ActionPlanCardComponent', () => {
  let component: ActionPlanCardComponent;
  let fixture: ComponentFixture<ActionPlanCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActionPlanCardComponent ]
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
