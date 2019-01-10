import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { MsalService } from "@azure/msal-angular";
import { BsModalService } from "ngx-bootstrap";
import { Global } from "../../global";
import { PersonalizedPlanService } from "../../guided-assistant/personalized-plan/personalized-plan.service";
import { BrowserTabCloseComponent } from "./browser-tab-close.component";

describe("BrowserTabCloseComponent", () => {
  let component: BrowserTabCloseComponent;
  let fixture: ComponentFixture<BrowserTabCloseComponent>;
  let modalService;
  let mockPersonalizedPlanService;
  let mockGlobal;
  let msalService;

  beforeEach(async(() => {
    msalService = jasmine.createSpyObj(["loginRedirect"]);

    TestBed.configureTestingModule({
      declarations: [BrowserTabCloseComponent],
      providers: [
        {
          provide: PersonalizedPlanService,
          useValue: mockPersonalizedPlanService
        },
        {
          provide: Global,
          useValue: {
            mockGlobal,
            userid: "userId",
            isLoginRedirect: true
          }
        },
        {
          provide: MsalService,
          useValue: msalService
        },
        {
          provide: BsModalService,
          useValue: modalService
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowserTabCloseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should assign global value in saveToProfile method if session key exists", () => {
    spyOn(sessionStorage, "getItem").and.returnValue(
      JSON.stringify("test data")
    );
    component.saveToProfile();
    expect(msalService.loginRedirect).toHaveBeenCalled();
  });
});
