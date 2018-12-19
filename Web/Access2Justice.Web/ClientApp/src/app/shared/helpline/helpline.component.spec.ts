import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HelplineComponent } from './helpline.component';

describe('HelplineComponent', () => {
  let component: HelplineComponent;
  let fixture: ComponentFixture<HelplineComponent>;
  let mockHelpText;

  beforeEach(async(() => {
    mockHelpText = {
      helpText: {
        beginningText: "Are you safe? Call",
        phoneNumber: "X-XXX-XXX-XXXX",
        endingText: "to get help."
      }
    }

    TestBed.configureTestingModule({
      declarations: [ HelplineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HelplineComponent);
    component = fixture.componentInstance;
    component.helpText = mockHelpText;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
