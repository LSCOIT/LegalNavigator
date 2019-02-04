import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
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
  planSessionKey: string = "bookmarkedPlan";
  topicsSessionKey: string = "bookmarkedTopics";
  roleInformation: any;
  displayResources: number = 3;
  activeSubtopicParam: string;
  searchResultDetails: any = {
    filterParam: "All",
    sortParam: "date",
    order: "DESC",
    topIntent: ""
  };
  topIntent: string;
  notifyLocationUpate: Subject<any> = new Subject<any>();

  constructor() {}

  setProfileData(
    oId: string,
    name: string,
    eMail: string,
    roleInformation: any
  ) {
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

  notifyLocationUpdate(location: any) {
    this.notifyLocationUpate.next(location);
  }
}

export enum UserStatus {
  Authorized,
  Shared,
  Anonymous,
  UnAuthorized
}
