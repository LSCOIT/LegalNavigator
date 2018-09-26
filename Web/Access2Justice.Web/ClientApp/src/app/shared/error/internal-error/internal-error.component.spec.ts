import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { InternalErrorComponent } from './internal-error.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('InternalErrorComponent', () => {
  let component: InternalErrorComponent;
  let fixture: ComponentFixture<InternalErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [InternalErrorComponent ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InternalErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
