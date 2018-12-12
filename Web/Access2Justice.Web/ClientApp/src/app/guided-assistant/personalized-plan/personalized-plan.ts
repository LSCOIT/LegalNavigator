import { Location } from "../../about/about";
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
  name: string;
  essentialReadings: Array<EssentialReadings>;
  icon: string;
  steps: Array<PlanStep>;
}

export interface EssentialReadings {
  text: string;
  url: string;
}

export interface PlanStep {
  stepId: string;
  title: string;
  description: string;
  order: number;
  isComplete: boolean;
  resources: Array<string>;
}

export interface PersonalizedPlanTopic {
  topic: PlanTopic;
  isSelected: boolean;
}

export interface PersonalizedPlanDescription {
  id: string;
  name: string;
  description: string;
  image: Image;
  location: Array<Location>;
  organizationalUnit: string;
}
export interface Image {
  source: string;
  altText: string;
}


