import { HttpClient, HttpHandler } from "@angular/common/http";
import { async, inject, TestBed } from "@angular/core/testing";
import { MsalService } from "@azure/msal-angular";
import { LoginService } from "./login.service";

describe("LoginService", () => {
  let service: LoginService;
  const httpSpy = jasmine.createSpyObj("http", ["get", "post"]);
  const mockMsalService = jasmine.createSpyObj(["getUser", "loginRedirect"]);

  beforeEach(async(() => {
    service = new LoginService(httpSpy, mockMsalService);
    httpSpy.get.calls.reset();

    TestBed.configureTestingModule({
      providers: [
        LoginService,
        HttpClient,
        HttpHandler,
        {
          provide: MsalService,
          useValue: mockMsalService
        }
      ]
    });
  }));

  it("should be created", inject([LoginService], (service: LoginService) => {
    expect(service).toBeTruthy();
  }));

  it("should be created", inject([LoginService], (service: LoginService) => {
    expect(service).toBeDefined();
  }));

  it("should be upsert the user profile details", async(() => {
    service.getUserProfile();
    mockMsalService.getUser.and.returnValue({});
    expect(mockMsalService.loginRedirect).toHaveBeenCalled();
  }));

  it("should set userProfile when userData is defined", async(() => {
    let mockUserData = {
      idToken: {
        name: "Test",
        oid: "123456789",
        preferred_username: "test_id"
      }
    };
    mockMsalService.getUser.and.returnValue(mockUserData);
    service.getUserProfile();
    expect(httpSpy.post).toHaveBeenCalled();
  }));
});
