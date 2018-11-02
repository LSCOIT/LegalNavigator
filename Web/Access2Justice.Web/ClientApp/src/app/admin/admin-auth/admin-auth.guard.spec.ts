import { TestBed, async, inject } from '@angular/core/testing';
import { AdminAuthGuard } from './admin-auth.guard';
import { Router, ActivatedRouteSnapshot } from '@angular/router';
import { LoginService } from '../../shared/login/login.service';
import { of } from 'rxjs/observable/of';
import { MsalService } from '@azure/msal-angular';

class MockRouter {
  navigate(path) { }
}

fdescribe('AdminAuthGuard', () => {
  let mockLoginService;
  let adminAuthGuard: AdminAuthGuard;
  let mockRouter;
  let route: ActivatedRouteSnapshot;
  let mockUserProfile = [
    {
      roleInformation: [
        { roleName: "Developer" }
      ]
    }
  ];
  beforeEach(() => {
    //mockLoginService = jasmine.createSpyObj((['getUserProfile']));
    //mockLoginService.getUserProfile.returnValue(of(mockUserProfile));
    

    TestBed.configureTestingModule({
      providers: [
        AdminAuthGuard,
        { provide: Router, useValue: mockRouter },
        ActivatedRouteSnapshot
      ]
    });
  });

  //it('should be truthy', inject([AdminAuthGuard], (guard: AdminAuthGuard) => {
  //  expect(guard).toBeTruthy();
  //}));

  it('should checkAdminStatus and return true', () => {
    adminAuthGuard = new AdminAuthGuard(mockLoginService, mockRouter);
    mockRouter = new MockRouter();
    mockLoginService = { getUserProfile: () => mockUserProfile };
    route = TestBed.get(ActivatedRouteSnapshot);
    spyOnProperty(route, 'data', 'get').and.returnValue({ roles: ['admin'] });
    expect(adminAuthGuard.canActivate(null, route)).toEqual(of(true));
  });
});
