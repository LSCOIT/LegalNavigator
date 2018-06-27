export interface PersonalizedPlanCondition {
  title: string;
  description: string;
  markCompleted: boolean;
  stepId: string;
  order: string;
  url: string; //bookmarked link - url at which user saves hte profile
  resourceType: string;  //Articles/organizations
  itemId: string; //GUID from Url
}

export interface Resources {
  url: string; //bookmarked link - url at which user saves hte profile
  resourceType: string;  //Articles/organizations
  itemId: string; //GUID from Url
}

export interface PlanSteps {
  topicId: string;
  steps: Array<Steps>[];
}

export interface Steps {
  id: string;
  title: string;
  type: string;
  description: string;
  order: string;
}



