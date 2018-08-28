import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivacyPromiseComponent } from './privacy-promise.component';
import { StaticResourceService } from '../shared/static-resource.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';

describe('PrivacyPromiseComponent', () => {
  let component: PrivacyPromiseComponent;
  let fixture: ComponentFixture<PrivacyPromiseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PrivacyPromiseComponent],
      imports: [HttpClientModule],
      providers: [StaticResourceService, HttpClient]
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
