import { TestBed, inject } from '@angular/core/testing';
import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { HttpClient, HttpHeaders, HttpParams, HttpClientModule } from '@angular/common/http';
import { PersonalizedPlanService } from '../guided-assistant/personalized-plan/personalized-plan.service';
import { IUserProfile } from '../shared/login/user-profile.model';
import { Global } from '../global';
import { MsalService } from '@azure/msal-angular';
import { Observable } from 'rxjs/Observable';
import { api } from '../../api/api';
import { ProfileResolverService } from './profile-resolver.service';
import { MSAL_CONFIG } from '@azure/msal-angular/dist/msal.service';
import { RouterTestingModule } from '@angular/router/testing';
import { BroadcastService } from '@azure/msal-angular/dist/broadcast.service';
import { ArrayUtilityService } from '../shared/array-utility.service';
import { ToastrService } from 'ngx-toastr';
import { LoginService } from '../shared/login/login.service';
import { resolve } from 'q';

describe('ProfileResolverService', () => {
  let profileResolverService: ProfileResolverService;
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post', 'put']);
  let global: Global;
  let msalService: MsalService;
  let loginService: LoginService;
  let personalizedPlanService: PersonalizedPlanService;
  var originalTimeout;
  let arrayUtilityService: ArrayUtilityService;
  let mockToastr;
  let mockRoute: ActivatedRouteSnapshot;
  let mockState: RouterStateSnapshot;
  
  beforeEach(() => {
    mockToastr = jasmine.createSpyObj(['success']);
    msalService = jasmine.createSpyObj(['getUser']);
    loginService = jasmine.createSpyObj(['upsertUserProfile']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule],
      providers: [ProfileResolverService,
        Global,
        MsalService,
        BroadcastService,
        PersonalizedPlanService,
        ArrayUtilityService,
        ToastrService,
        { provide: MSAL_CONFIG, useValue: {} },
        { provide: ToastrService, useValue: mockToastr },
        LoginService
      ]
    });
    profileResolverService = new ProfileResolverService(global, msalService, loginService);
    arrayUtilityService = new ArrayUtilityService();
    httpSpy.get.calls.reset();
  });

  it('should be created', inject([ProfileResolverService], (profileResolverService: ProfileResolverService) => {
    expect(profileResolverService).toBeDefined();
  }));

  it('should be created', inject([ProfileResolverService], (profileResolverService: ProfileResolverService) => {
    expect(profileResolverService).toBeTruthy();
  }));

  it('should resolve be called', (done) => {
    spyOn(profileResolverService, 'resolve');    
    profileResolverService.resolve(mockRoute, mockState);
    expect(profileResolverService.resolve).toHaveBeenCalled();
    done();
  });
 
});
