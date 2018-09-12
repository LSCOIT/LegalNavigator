export interface Resources {
  url: string; 
  itemId: string;
  type: string;
}

export interface SavedResources {
  itemId: string;
  resourceType: string;
  resourceDetails: any;
}

export interface ProfileResources {
  oId: string;
  resourceTags: Array<SavedResources>;
  type: string;
}

export interface UserPlan {
  oId: string;
  plan: PersonalizedPlan;
}

export interface PersonalizedPlan {
  id: string;
  topics: Array<PlanTopic>;
  isShared: boolean;
}

export interface PlanTopic {
  topicId: string;
  steps: Array<PlanStep>;
}

export interface PlanStep {
  stepId: string;
  title: string;
  description: string;
  order: number;
  isComplete: boolean;
  resources: Array<string>;
  topicIds: Array<string>;
}

export interface PersonalizedPlanTopic {
  topic: any;
  isSelected: boolean;
}
