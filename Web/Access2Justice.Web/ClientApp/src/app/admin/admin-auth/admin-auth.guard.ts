import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { LoginService } from '../../shared/login/login.service';
import { Subject } from 'rxjs';

@Injectable()
export class AdminAuthGuard implements CanActivate {

  constructor(
    private router: Router,
    private loginService: LoginService) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot):Observable<any> {
    let url: string = state.url;
    return this.checkAdminStatus(url).map(value => {
      console.log(value);
      return value;
    });
  }

  checkAdminStatus(url: string) {
    let subject = new Subject<boolean>();
    this.loginService.getUserProfile()
      .subscribe(response => {
          let findAdmin = response.roleInformation.find(role => {
            return role.roleName.includes("Admin") || role.roleName === 'Developer';
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
