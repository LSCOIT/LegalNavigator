import { TestBed, async, inject, fakeAsync, tick } from '@angular/core/testing';
import { AdminAuthGuard } from './admin-auth.guard';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { LoginService } from '../../shared/login/login.service';
import { of } from 'rxjs/observable/of';
import { MsalService } from '@azure/msal-angular';
import { pipe } from 'rxjs';

describe('AdminAuthGuard', () => {
  let adminAuthGuard: AdminAuthGuard;
  let mockLoginService;
  let mockRouter;
  let mockUserProfile;

  beforeEach(() => {
    mockLoginService = jasmine.createSpyObj(['getUserProfile']);

    TestBed.configureTestingModule({
      providers: [
        AdminAuthGuard,
        { provide: Router, useValue: mockRouter },
        { provide: LoginService, useValue: mockLoginService }
      ]
    });
    adminAuthGuard = new AdminAuthGuard(mockRouter, mockLoginService);
  });

  it('should be truthy', inject([AdminAuthGuard], (guard: AdminAuthGuard) => {
    expect(guard).toBeTruthy();
  }));

  it('should checkAdminStatus and return true', fakeAsync(() => {
    mockRouter = {
      navigate: () => { },
      url: '/admin'
    }

    mockUserProfile = {
      roleInformation: [
        { roleName: "Admin" }
      ]
    };
    mockLoginService.getUserProfile.and.returnValue(of(mockUserProfile));
    adminAuthGuard.canActivate(mockRouter, mockLoginService);
    expect(adminAuthGuard.canActivate(mockRouter, mockLoginService)).toBeTruthy();
  }));
});
