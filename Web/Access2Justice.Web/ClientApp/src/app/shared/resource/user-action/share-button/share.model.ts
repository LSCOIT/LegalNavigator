export interface Share {
  Url?: string;
  ResourceId?: string;
  UserId?: string;
}

export interface UnShare {
  ResourceId?: string;
  UserId?: string;
}

export interface ShareView {
  PermaLink?: string;
}
