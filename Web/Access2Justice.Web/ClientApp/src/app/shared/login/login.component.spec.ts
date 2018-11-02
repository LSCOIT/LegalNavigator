import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Global } from '../../global';
import { LoginComponent } from './login.component';
import { Router } from '@angular/router';
import { MsalService, BroadcastService } from '@azure/msal-angular';
import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';
import { HttpClientModule } from '@angular/common/http';
import { ArrayUtilityService } from '../array-utility.service';
import { ToastrService } from 'ngx-toastr';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { LoginService } from './login.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockRouter;
  let mockGlobal;
  let msalService;
  let mockToastr;
  let mockLoginService;
  msalService = jasmine.createSpyObj(['getUser']);
  beforeEach(async(() => {
    mockGlobal = jasmine.createSpyObj(['notifyRoleInformation']);
    TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [HttpClientModule],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: Global, useValue: mockGlobal },
        { provide: MsalService, useValue: msalService },
        { provide: ToastrService, useValue: mockToastr },
        { provide: LoginService, useValue: mockLoginService },
        BroadcastService,
        PersonalizedPlanService,
        ArrayUtilityService
      ],
      schemas: [NO_ERRORS_SCHEMA]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    spyOn(component, 'ngOnInit');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should checkAdmin role', () => {
    let mockRoleInformation = [
      {
        roleName: "Authenticated"
      }
    ];
    component.checkIfAdmin(mockRoleInformation);
    expect(component.isAdmin).toBe(false);
  });
});
