import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpperNavComponent } from './upper-nav.component';

describe('UpperNavComponent', () => {
  let component: UpperNavComponent;
  let fixture: ComponentFixture<UpperNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpperNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpperNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
