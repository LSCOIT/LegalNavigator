import { TestBed, inject } from '@angular/core/testing';

import { ProfileResolverService } from './profile-resolver.service';
import { Global } from '../global';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { MsalService, MSAL_CONFIG } from '@azure/msal-angular/dist/msal.service';

fdescribe('ProfileResolverService', () => {
  let mockGlobal;
  let httpTestingController: HttpTestingController;
  let profileResolverService: ProfileResolverService;
  //beforeEach(() => {
  //  TestBed.configureTestingModule({
  //    providers: [
  //      ProfileResolverService,
  //      { provide: Global, useValue: mockGlobal }
  //    ],       
  //  });
  //});

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProfileResolverService,
        HttpClientTestingModule,
        MsalService,
        MSAL_CONFIG,
                { provide: Global, useValue: mockGlobal }]
    });
    httpTestingController = TestBed.get(HttpTestingController);
    profileResolverService = TestBed.get(ProfileResolverService);
  });

  it('should be created', inject([ProfileResolverService], (profileResolverService: ProfileResolverService) => {
    expect(profileResolverService).toBeTruthy();
  }));
});
