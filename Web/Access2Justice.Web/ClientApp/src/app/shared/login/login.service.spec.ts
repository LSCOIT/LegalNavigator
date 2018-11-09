import { TestBed, inject, async } from '@angular/core/testing';
import { LoginService } from './login.service';
import { HttpClient, HttpHandler, HttpHeaders } from '@angular/common/http';
import { IUserProfile } from './user-profile.model';
import { Observable } from 'rxjs/Observable';
import { MsalService } from '@azure/msal-angular';

describe('LoginService', () => {

  let service: LoginService;
  const mockHttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  const httpSpy = jasmine.createSpyObj('http', ['get', 'post']);
  const mockMsalService = jasmine.createSpyObj(['getUser', 'loginRedirect']);

  beforeEach((async(() => {
    service = new LoginService(httpSpy, mockMsalService);
    httpSpy.get.calls.reset();

    TestBed.configureTestingModule({
      providers: [
        LoginService,
        HttpClient,
        HttpHandler,
        { provide: MsalService, useValue: mockMsalService }
      ]
    });
  })));

  it('should be created', inject([LoginService], (service: LoginService) => {
    expect(service).toBeTruthy();
  }));

  it('should be created', inject([LoginService], (service: LoginService) => {
    expect(service).toBeDefined();
  }));

  it('should be upsert the user profile details', async(() => {
    service.getUserProfile();
    mockMsalService.getUser.and.returnValue({});
    expect(mockMsalService.loginRedirect).toHaveBeenCalled();
  }));

  it('should set userProfile when userData is defined', async(() => {
    let mockUserData = {
      idToken: {
        name: "Test",
        oid: "123456789",
        preferred_username: "test_id"
      },
    };
    mockMsalService.getUser.and.returnValue(mockUserData);
    service.getUserProfile();
    expect(httpSpy.post).toHaveBeenCalled();
  }));
});
