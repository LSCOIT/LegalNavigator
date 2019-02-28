import { HttpClientModule } from "@angular/common/http";
import { fakeAsync, inject, TestBed, tick } from "@angular/core/testing";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { RouterTestingModule } from "@angular/router/testing";
import { of } from "rxjs";

import { LoginService } from "../common/login/login.service";
import { ProfileResolver } from "./profile-resolver.service";

describe("ProfileResolverService", () => {
  let profileResolverService: ProfileResolver;
  let mockLoginService;
  let mockRoute: ActivatedRouteSnapshot;
  let mockState: RouterStateSnapshot;

  beforeEach(() => {
    mockLoginService = jasmine.createSpyObj(["getUserProfile"]);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule],
      providers: [
        ProfileResolver,
        {
          provide: LoginService,
          useValue: mockLoginService
        }
      ]
    });
    profileResolverService = new ProfileResolver(mockLoginService);
  });

  it("should be defined", inject(
    [ProfileResolver],
    (profileResolverService: ProfileResolver) => {
      expect(profileResolverService).toBeDefined();
    }
  ));

  it("should be created", inject(
    [ProfileResolver],
    (profileResolverService: ProfileResolver) => {
      expect(profileResolverService).toBeTruthy();
    }
  ));

  it("should call LoginService getUser profile when resolve is called", fakeAsync(() => {
    let mockLoginResponse = {
      oId: "1234567890ABC",
      name: "mockUser",
      eMail: "mockUser@microsoft.com"
    };
    mockLoginService.getUserProfile.and.returnValue(of(mockLoginResponse));
    profileResolverService.resolve(mockRoute, mockState);
    tick(250);
    expect(mockLoginService.getUserProfile).toHaveBeenCalledWith();
  }));
});
