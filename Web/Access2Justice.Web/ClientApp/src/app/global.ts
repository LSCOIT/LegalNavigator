import { Injectable } from '@angular/core';
import { api } from '../api/api';
import { Subject } from 'rxjs';

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
  notifyRoleInformation: Subject<any> = new Subject<any>();
  userName: string;
  userId: string;
  sharedUserId: string;
  sharedUserName: string;
  isShared: boolean = false;
  isLoggedIn: boolean = false;
  topicsData: any;
  organizationsData: any;
  isLoginRedirect: boolean = false;
  sessionKey: string = "bookmarkedResource";
  planSessionKey: string = "bookmarkPlan";

  constructor() { }

  setProfileData(oId: string, name: string, eMail: string, roleInformation: any) {
    this.userId = oId;
    this.userName = name ? name : eMail;
    this.roleInformation = roleInformation;
    this.isLoggedIn = true;
    this.notifyRoleInformation.next(this.roleInformation);
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
