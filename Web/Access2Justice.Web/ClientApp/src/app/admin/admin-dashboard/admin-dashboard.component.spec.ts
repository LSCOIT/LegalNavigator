import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";
import { Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { Global } from "../../global";
import { NavigateDataService } from "../../common/services/navigate-data.service";
import { StaticResourceService } from "../../common/services/static-resource.service";
import { AdminDashboardComponent } from "./admin-dashboard.component";

describe("AdminDashboardComponent", () => {
  let component: AdminDashboardComponent;
  let fixture: ComponentFixture<AdminDashboardComponent>;
  let mockGlobal;
  let mockStaticResourceService;
  let mockRouter;
  let mockNavigateDataService;
  let mockNgxSpinnerService;

  beforeEach(async(() => {
    mockNgxSpinnerService = jasmine.createSpyObj(["show", "hide"]);
    mockGlobal = {
      roleInformation: [
        {
          roleName: "StateAdmin",
          organizationalUnit: "Alaska"
        },
        {
          roleName: "Developer",
          organizationalUnit: ""
        }
      ]
    };
    TestBed.configureTestingModule({
      declarations: [AdminDashboardComponent],
      providers: [
        {
          provide: Global,
          useValue: mockGlobal
        },
        {
          provide: StaticResourceService,
          useValue: mockStaticResourceService
        },
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: NavigateDataService,
          useValue: mockNavigateDataService
        },
        {
          provide: NgxSpinnerService,
          useValue: mockNgxSpinnerService
        }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("should check if user is a StateAdmin onInit", () => {
    spyOn(component, "checkIfStateAdmin");
    component.ngOnInit();
    expect(component.checkIfStateAdmin).toHaveBeenCalledWith(
      mockGlobal.roleInformation
    );
    expect(component.isStateAdmin).toBe(true);
    expect(component.stateList).toEqual(["Alaska"]);
  });
});
