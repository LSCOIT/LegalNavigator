import { TestBed, inject, fakeAsync } from '@angular/core/testing';
import { AdminAuthGuard } from './admin-auth.guard';
import { Router } from '@angular/router';
import { LoginService } from '../../shared/login/login.service';
import { of } from 'rxjs/observable/of';
import { Global } from '../../global';

describe('AdminAuthGuard', () => {
  let adminAuthGuard: AdminAuthGuard;
  let mockLoginService;
  let mockRouter;
  let mockUserProfile;
  let mockGlobal;

  beforeEach(() => {
    mockLoginService = jasmine.createSpyObj(['getUserProfile']);

    mockGlobal = {
      roleInformation: {
        find: () => { return { roleName: "Portal Admin" } }
      }
    }
    mockRouter = {
      navigate: () => {}
    }

    TestBed.configureTestingModule({
      providers: [
        AdminAuthGuard,
        { provide: Router, useValue: mockRouter },
        { provide: LoginService, useValue: mockLoginService },
        { provide: Global, useValue: mockGlobal }
      ]
    });
    adminAuthGuard = new AdminAuthGuard(mockRouter, mockLoginService, mockGlobal);
  });

  it('should be truthy', inject([AdminAuthGuard], (guard: AdminAuthGuard) => {
    expect(guard).toBeTruthy();
  }));

  it('should check global role information and return true if Admin - canActivate', fakeAsync(() => {
    mockRouter = {
      navigate: () => { },
      url: '/admin'
    }
    adminAuthGuard.canActivate(mockRouter, mockLoginService);
    expect(adminAuthGuard.canActivate(mockRouter, mockLoginService)).toBeTruthy();
  }));

  it('should check global role information and return true if Portal Admin - canActivateChild', fakeAsync(() => {
    mockRouter = {
      navigate: () => { },
      url: '/admin/privacy'
    }

    adminAuthGuard.canActivateChild(mockRouter, mockLoginService);
    expect(adminAuthGuard.canActivateChild(mockRouter, mockLoginService)).toBeTruthy();
  }));
});
