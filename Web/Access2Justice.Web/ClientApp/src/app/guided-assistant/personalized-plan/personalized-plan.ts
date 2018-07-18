export interface Resources {
  url: string; 
  itemId: string; 
}

export interface ProfileResources {
  oId: string;
  resourceTags: Array<string>;
  type: string;
}

export interface PlanSteps {
  topicId: string;
  topicName: string;
  stepTags: Array<Steps>[];
}

export interface PlanTag {
  topicId: string;
  stepTags: Array<StepTag>;
}

export interface StepTag {
  id: string;
  order: string;
  markCompleted: boolean;
}

export interface Steps {
  id: string;
  title: string;
  type: string;
  description: string;
  order: string;
  markCompleted: boolean;
}

export interface CreatePlan {
  planId: string;
  oId: string;
  type: string;
  planTags: any;
}

export interface UpdatePlan {
  id: string;
  oId: string;
  planTags: Array<PlanTag>;
}

export interface UserUpdatePlan {
  id: string;
  planId: string;
  oId: string;
  planTags: Array<PlanTag>;
  type: string;
}

export interface RemovePlanTag {
  id: string;
  oId: string;
  topicId: string;
}

export interface UserRemovePlanTag {
  id: string;
  planId: string;
  oId: string;
  topicId: string;
  type: string;
}

