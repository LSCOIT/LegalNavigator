export interface Share {
  Url?: string;
  ResourceId?: string;
  UserId?: string;
}

export interface ShareView {
  PermaLink?: string;
  UserId?: string;
  UserName?: string;
  IsShared?: boolean;
  ResourceUrl?: string;
}
