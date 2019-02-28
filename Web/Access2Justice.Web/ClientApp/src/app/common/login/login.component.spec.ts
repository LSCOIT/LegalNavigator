import { HttpClientModule } from "@angular/common/http";
import { NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { Router } from "@angular/router";
import { BroadcastService, MsalService } from "@azure/msal-angular";
import { ToastrService } from "ngx-toastr";
import { Global } from "../../global";
import { PersonalizedPlanService } from "../../guided-assistant/personalized-plan/personalized-plan.service";
import { ArrayUtilityService } from "../services/array-utility.service";
import { LoginComponent } from "./login.component";
import { LoginService } from "./login.service";

describe("LoginComponent", () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockRouter;
  let mockGlobal;
  let msalService;
  let mockToastr;
  let mockLoginService;

  beforeEach(async(() => {
    msalService = jasmine.createSpyObj(["getUser"]);
    mockGlobal = jasmine.createSpyObj(["notifyRoleInformation"]);

    TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [HttpClientModule],
      providers: [
        BroadcastService,
        PersonalizedPlanService,
        ArrayUtilityService,
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: Global,
          useValue: mockGlobal
        },
        {
          provide: MsalService,
          useValue: msalService
        },
        {
          provide: ToastrService,
          useValue: mockToastr
        },
        {
          provide: LoginService,
          useValue: mockLoginService
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    spyOn(component, "ngOnInit");
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should checkAdmin role", () => {
    let mockRoleInformation = [
      {
        roleName: "Authenticated"
      }
    ];
    component.checkIfAdmin(mockRoleInformation);
    expect(component.isAdmin).toBe(false);
  });
});
