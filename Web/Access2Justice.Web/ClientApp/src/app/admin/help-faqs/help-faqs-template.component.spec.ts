import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpFaqsTemplateComponent } from './help-faqs-template.component';

describe('HelpFaqsTemplateComponent', () => {
  let component: HelpFaqsTemplateComponent;
  let fixture: ComponentFixture<HelpFaqsTemplateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HelpFaqsTemplateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HelpFaqsTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
