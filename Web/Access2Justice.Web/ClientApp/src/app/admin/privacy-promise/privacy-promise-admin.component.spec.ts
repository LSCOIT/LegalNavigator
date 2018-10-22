import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivacyPromiseAdminComponent } from './privacy-promise-admin.component';

describe('PrivacyPromiseComponent', () => {
  let component: PrivacyPromiseAdminComponent;
  let fixture: ComponentFixture<PrivacyPromiseAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PrivacyPromiseAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivacyPromiseAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
