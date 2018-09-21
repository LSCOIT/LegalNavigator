import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Global } from '../../global';
import { LoginComponent } from './login.component';
import { Router } from '@angular/router';
import { MsalService, BroadcastService } from '@azure/msal-angular';
import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { HttpClientModule } from '@angular/common/http';
import { ArrayUtilityService } from '../array-utility.service';
import { ToastrService } from 'ngx-toastr';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockRouter;
  let mockGlobal;
  let msalService;
  let mockToastr;
  msalService = jasmine.createSpyObj(['getUser']);
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [HttpClientModule],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: Global, useValue: mockGlobal },
        { provide: MsalService, useValue: msalService },
        { provide: ToastrService, useValue: mockToastr },
        BroadcastService,
        PersonalizedPlanService,
        ArrayUtilityService
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
