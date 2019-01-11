import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { LoginService } from "../shared/login/login.service";

@Injectable()
export class ProfileResolver implements Resolve<any> {
  constructor(private loginService: LoginService) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {
    if (document.URL.indexOf("/share/") <= -1) {
      return this.loginService.getUserProfile();
    }
  }
}
