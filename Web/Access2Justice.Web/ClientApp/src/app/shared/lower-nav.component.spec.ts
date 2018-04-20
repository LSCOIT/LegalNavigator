import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LowerNavComponent } from './lower-nav.component';

describe('LowerNavComponent', () => {
  let component: LowerNavComponent;
  let fixture: ComponentFixture<LowerNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LowerNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LowerNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
