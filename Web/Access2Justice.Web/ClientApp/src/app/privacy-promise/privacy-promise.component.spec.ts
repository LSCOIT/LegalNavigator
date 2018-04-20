import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivacyPromiseComponent } from './privacy-promise.component';

describe('PrivacyPromiseComponent', () => {
  let component: PrivacyPromiseComponent;
  let fixture: ComponentFixture<PrivacyPromiseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrivacyPromiseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivacyPromiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
