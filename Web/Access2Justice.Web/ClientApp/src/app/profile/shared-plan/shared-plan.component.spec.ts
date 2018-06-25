import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedPlanComponent } from './shared-plan.component';

describe('SharedPlanComponent', () => {
  let component: SharedPlanComponent;
  let fixture: ComponentFixture<SharedPlanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SharedPlanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SharedPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
