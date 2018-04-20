import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpFaqsComponent } from './help-faqs.component';

describe('HelpFaqsComponent', () => {
  let component: HelpFaqsComponent;
  let fixture: ComponentFixture<HelpFaqsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HelpFaqsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HelpFaqsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
