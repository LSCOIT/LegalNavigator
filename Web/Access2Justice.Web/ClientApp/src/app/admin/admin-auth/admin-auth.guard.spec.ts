import { fakeAsync, inject, TestBed } from "@angular/core/testing";
import { Router } from "@angular/router";
import { Global } from "../../global";
import { LoginService } from "../../common/login/login.service";
import { AdminAuthGuard } from "./admin-auth.guard";
import {CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA} from '@angular/core';
describe("AdminAuthGuard", () => {
  let adminAuthGuard: AdminAuthGuard;
  let mockLoginService;
  let mockRouter;
  let mockGlobal;

  beforeEach(() => {
    mockLoginService = jasmine.createSpyObj(["getUserProfile"]);
    mockGlobal = {
      roleInformation: {
        find: () => {
          return {
            roleName: "Portal Admin"
          };
        }
      }
    };
    mockRouter = {
      navigate: () => {}
    };

    TestBed.configureTestingModule({
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
      providers: [
        AdminAuthGuard,
        {
          provide: Router,
          useValue: mockRouter
        },
        {
          provide: LoginService,
          useValue: mockLoginService
        },
        {
          provide: Global,
          useValue: mockGlobal
        }
      ]
    });
    adminAuthGuard = new AdminAuthGuard(
      mockRouter,
      mockLoginService,
      mockGlobal
    );
  });

  it("should be truthy", inject([AdminAuthGuard], (guard: AdminAuthGuard) => {
    expect(guard).toBeTruthy();
  }));

  it("should check global role information and return true if Admin - canActivate", fakeAsync(() => {
    mockRouter = {
      navigate: () => {},
      url: "/admin"
    };
    adminAuthGuard.canActivate(mockRouter, mockLoginService);
    expect(
      adminAuthGuard.canActivate(mockRouter, mockLoginService)
    ).toBeTruthy();
  }));

  it("should check global role information and return true if Portal Admin - canActivateChild", fakeAsync(() => {
    mockRouter = {
      navigate: () => {},
      url: "/admin/privacy"
    };
    adminAuthGuard.canActivateChild(mockRouter, mockLoginService);
    expect(
      adminAuthGuard.canActivateChild(mockRouter, mockLoginService)
    ).toBeTruthy();
  }));
});
