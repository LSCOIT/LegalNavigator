import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GuidedAssistantSidebarComponent } from './guided-assistant-sidebar.component';

describe('GuidedAssistantSidebarComponent', () => {
  let component: GuidedAssistantSidebarComponent;
  let fixture: ComponentFixture<GuidedAssistantSidebarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GuidedAssistantSidebarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuidedAssistantSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
