import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintButtonComponent } from './print-button.component';

describe('PrintButtonComponent', () => {
  let component: PrintButtonComponent;
  let fixture: ComponentFixture<PrintButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrintButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrintButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
