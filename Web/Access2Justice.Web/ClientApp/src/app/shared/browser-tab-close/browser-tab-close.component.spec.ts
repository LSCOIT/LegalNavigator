import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BrowserTabCloseComponent } from './browser-tab-close.component';
import { BsModalService } from 'ngx-bootstrap';
import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { Global } from '../../global';
import { MsalService } from '@azure/msal-angular';
import { TemplateRef } from '@angular/core';

describe('BrowserTabCloseComponent', () => {
  class MockBsModalRef {
    public isHideCalled = false;

    hide() {
      this.isHideCalled = true;
    }
  }
  let component: BrowserTabCloseComponent;
  let fixture: ComponentFixture<BrowserTabCloseComponent>;
  let modalService;
  let mockPersonalizedPlanService;
  let mockGlobal;
  let msalService;
  let template: TemplateRef<any>;
  let modalRefInstance = new MockBsModalRef();

  beforeEach(async(() => {
    msalService = jasmine.createSpyObj(['loginRedirect']);

    TestBed.configureTestingModule({
      declarations: [BrowserTabCloseComponent],
      providers: [
        { provide: PersonalizedPlanService, useValue: mockPersonalizedPlanService },
        { provide: Global, useValue: { mockGlobal, userid: "userId", isLoginRedirect: true } },
        { provide: MsalService, useValue: msalService },
        { provide: BsModalService, useValue: modalService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowserTabCloseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    let store = {};
    const mockSessionStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      setItem: (key: string, value: string) => {
        store[key] = `${value}`;
      },
      removeItem: (key: string) => {
        delete store[key];
      },
      clear: () => {
        store = {};
      }
    };
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should assign global value in saveToProfile method if session key exists', () => {
    spyOn(sessionStorage, 'getItem')
      .and.returnValue(JSON.stringify("test data"));
    component.saveToProfile();
    expect(msalService.loginRedirect).toHaveBeenCalled();
  });
});
