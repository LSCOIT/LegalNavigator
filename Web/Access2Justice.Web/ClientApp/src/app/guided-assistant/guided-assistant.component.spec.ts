import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GuidedAssistantComponent } from './guided-assistant.component';

describe('GuidedAssistantComponent', () => {
  let component: GuidedAssistantComponent;
  let fixture: ComponentFixture<GuidedAssistantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GuidedAssistantComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuidedAssistantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
