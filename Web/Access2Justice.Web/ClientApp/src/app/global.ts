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
  userName: string;
  userId: string;
  sharedUserId: string;
  sharedUserName: string;
  isShared: boolean = false;
  isLoggedIn: boolean = false;
  topicsData: any;
  organizationsData: any;
  notifyScreenSizeChange: Subject<any> = new Subject<any>();

  constructor() {
    window.addEventListener('resize', (event) => {
      this.notifyScreenSizeChange.next(event.target["innerWidth"]);
    });
  }

  setProfileData(oId: string, name: string, eMail: string) {
    this.userId = oId;
    this.userName = name ? name : eMail;
    this.isLoggedIn = true;
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
