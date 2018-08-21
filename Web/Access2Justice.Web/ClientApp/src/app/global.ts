import { Injectable } from '@angular/core';
import { api } from '../api/api';

@Injectable()
export class Global {
  role: UserStatus = UserStatus.Anonymous;
  showShare: boolean = true;
  showRemove: boolean = true;
  showMarkComplete: boolean = true;
  showDropDown: boolean = true;
  showSetting: boolean = true;
  shareRouteUrl: string = "/share";
  profileRouteUrl: string = "/profile";



  externalLogin() {
    var form = document.createElement('form');
    form.setAttribute('method', 'POST');
    form.setAttribute('action', api.loginUrl);
    document.body.appendChild(form);
    form.submit();
  }
}

export enum UserStatus {
  Authorized,
  Shared,
  Anonymous,
  UnAuthorized
}
