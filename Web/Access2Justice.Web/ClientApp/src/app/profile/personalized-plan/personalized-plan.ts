export interface PersonalizedPlanCondition {
  url: string; //bookmarked link - url at which user saves the profile
  resourceType: string;  //Articles/organizations
  itemId: string; //GUID from Url
}

export interface Resources {
  url: string; 
  itemId: string; 
}

export interface ActionPlanSteps{
  actionPlanStep: Array<PlanSteps>[];
}

export interface PlanSteps {
  topicId: string;
  topicName: string;
  steps: Array<Steps>[];
}

export interface Steps {
  id: string;
  title: string;
  type: string;
  description: string;
  order: string;
  isCompleted: boolean;
}

export interface UpdateCompletedStatus {
  oId: string;
  topicId: string;
  stepId: string;
  completedStatus: boolean;
}



