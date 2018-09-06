import { Injectable } from '@angular/core';
import { api } from '../api/api';
import { Subject } from 'rxjs';
import { MsalService } from '@azure/msal-angular';

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
  data: any;
  notifyStaticData: Subject<any> = new Subject<any>();  

  constructor(private msalService: MsalService) { }

  externalLogin() {
    this.msalService.loginRedirect(["user.read"]);
  }

  getData() {
    return this.data;
  }
  setData(data: any) {
    this.data = data;
    this.notifyStaticData.next(this.data);
  }
}

export enum UserStatus {
  Authorized,
  Shared,
  Anonymous,
  UnAuthorized
}
