import { Injectable } from '@angular/core';

@Injectable()
export class Global {
  role: UserStatus = UserStatus.Anonymous;
  showShare: boolean = true;
  showRemove: boolean = true;
  showMarkComplete: boolean = true;
  showDropDown: boolean = true;
  shareRouteUrl: string = "/share";
}

export enum UserStatus {
  Authorized,
  Shared,
  Anonymous,
  UnAuthorized
}
