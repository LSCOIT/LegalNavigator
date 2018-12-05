import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { LoginService } from '../../shared/login/login.service';
import { Subject } from 'rxjs';
import { Global } from '../../global';
import { of } from 'rxjs/observable/of';

@Injectable()
export class AdminAuthGuard implements CanActivate {
  adminRoles = ['PortalAdmin', 'StateAdmin', 'Developer'];

  constructor(
    private router: Router,
    private loginService: LoginService,
    private global: Global) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot):Observable<any> {
    let url: string = state.url;

    if (this.global.roleInformation) {
      let findAdmin = this.global.roleInformation.find(role => {
        return this.adminRoles.includes(role.roleName);
      });
      if (!findAdmin) {
        this.router.navigate(['/401']);
      } else {
        return of(true);
      }
    } else {
      return this.checkAdminStatus(url, this.adminRoles).map(value => {
        if (!value) {
          this.router.navigate(['/401']);
        }
        return value;
      });
    }
  }

  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<any> {
    let url: string = state.url;
    if (this.global.roleInformation) {
      let findPortalAdmin = this.global.roleInformation.find(role => {
        return role.roleName === "PortalAdmin";
      });
      if (!findPortalAdmin) {
        this.router.navigate(['/401']);
      } else {
        return of(true);
      }
    } else {
      return this.checkAdminStatus(url, "PortalAdmin").map(value => {
        if (!value) {
          this.router.navigate(['/401']);
        }
        return value;
      });
    }
  }

  checkAdminStatus(url: string, admin) {
    let subject = new Subject<boolean>();
    this.loginService.getUserProfile()
      .subscribe(response => {
        this.global.setProfileData(response.oId, response.name, response.eMail, response.roleInformation);
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
