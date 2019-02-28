
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Subject, Observable, of } from "rxjs";
import {map} from 'rxjs/operators';

import { Global } from "../../global";
import { LoginService } from "../../common/login/login.service";

@Injectable()
export class AdminAuthGuard implements CanActivate {
  adminRoles = ["PortalAdmin", "StateAdmin", "Developer"];

  constructor(
    private router: Router,
    private loginService: LoginService,
    private global: Global
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {
    const url: string = state.url;

    if (this.global.roleInformation) {
      const findAdmin = this.global.roleInformation.find(role => {
        return this.adminRoles.includes(role.roleName);
      });
      if (!findAdmin) {
        this.router.navigate(["/401"]);
      } else {
        return of(true);
      }
    } else {
      return this.checkAdminStatus(url, this.adminRoles).pipe(map(value => {
        if (!value) {
          this.router.navigate(["/401"]);
        }
        return value;
      }));
    }
  }

  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {
    let url: string = state.url;
    if (this.global.roleInformation) {
      const findPortalAdmin = this.global.roleInformation.find(role => {
        return role.roleName === "PortalAdmin";
      });
      if (!findPortalAdmin) {
        this.router.navigate(["/401"]);
      } else {
        return of(true);
      }
    } else {
      return this.checkAdminStatus(url, "PortalAdmin").pipe(map(value => {
        if (!value) {
          this.router.navigate(["/401"]);
        }
        return value;
      }));
    }
  }

  checkAdminStatus(url: string, admin) {
    let subject = new Subject<boolean>();
    this.loginService.getUserProfile().subscribe(
      response => {
        this.global.setProfileData(
          response.oId,
          response.name,
          response.eMail,
          response.roleInformation
        );
        let findAdmin = response.roleInformation.find(role => {
          return admin.includes(role.roleName);
        });
        if (findAdmin) {
          subject.next(true);
        } else {
          subject.next(false);
        }
      },
      error => subject.next(false)
    );
    return subject;
  }
}
