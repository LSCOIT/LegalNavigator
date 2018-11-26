import { TestBed, async, inject, fakeAsync, tick } from '@angular/core/testing';
import { CuratedExperienceAuthGuard } from './curated-experience-auth.guard';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { LoginService } from '../../shared/login/login.service';
import { of } from 'rxjs/observable/of';
import { MsalService } from '@azure/msal-angular';
import { pipe } from 'rxjs';

describe('CuratedExperienceAuthGuard', () => {
  let curatedExperienceAuthGuard: CuratedExperienceAuthGuard;
  let mockLoginService;
  let mockRouter;
  let mockUserProfile;

  beforeEach(() => {
    mockLoginService = jasmine.createSpyObj(['getUserProfile']);

    TestBed.configureTestingModule({
      providers: [
        CuratedExperienceAuthGuard,
        { provide: Router, useValue: mockRouter },
        { provide: LoginService, useValue: mockLoginService }
      ]
    });
    curatedExperienceAuthGuard = new CuratedExperienceAuthGuard(mockRouter, mockLoginService);
  });

  it('should be truthy', inject([CuratedExperienceAuthGuard], (guard: CuratedExperienceAuthGuard) => {
    expect(guard).toBeTruthy();
  }));

  it('should checkAdminStatus and return true', fakeAsync(() => {
    mockRouter = {
      navigate: () => { },
      url: '/admin/curated-experience'
    }

    mockUserProfile = {
      roleInformation: [
        { roleName: "PortalAdmin" }
      ]
    };
    mockLoginService.getUserProfile.and.returnValue(of(mockUserProfile));
    curatedExperienceAuthGuard.canActivate(mockRouter, mockLoginService);
    expect(curatedExperienceAuthGuard.canActivate(mockRouter, mockLoginService)).toBeTruthy();
  }));
});
