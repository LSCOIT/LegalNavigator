import { MapLocation } from "../../../map/map";

export interface Share {
  Url?: string;
  ResourceId?: string;
  UserId?: string;
  Location: MapLocation;
  ResourceType: string;
}

export interface ShareView {
  PermaLink?: string;
  UserId?: string;
  UserName?: string;
  IsShared?: boolean;
  ResourceUrl?: string;
}

export interface ShareData {
  Oid?: string;
  Email?: string;
  ItemId?: string;
  ResourceType?: string;
  ResourceDetails?: any;
  Plan?: any;
}
