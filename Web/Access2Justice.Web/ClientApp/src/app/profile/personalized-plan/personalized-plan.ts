export interface PersonalizedPlanCondition {
  title: string;
  description : string;
}

export interface Resources {
  url: string; //bookmarked link - url at which user saves hte profile
  resourceType: string;  //Articles/organizations
  itemId: string; //GUID from Url
}
