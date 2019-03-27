import { Location } from '../../about/about';
import { MapLocation } from '../../common/map/map';

export interface SavedResource {
  itemId: string;
  resourceType: string;
  resourceDetails: any;
}

export interface ProfileResources {
  oId: string;
  resourceTags: SavedResource[];
  type: string;
}

export interface RemovedElem {
  Oid?: string;
  ItemId?: string;
  ResourceType?: string;
  SharedBy?: string;
  Type?: string;
}

export interface PersonalizedPlan {
  id: string;
  topics: PlanTopic[];
  isShared: boolean;
}

export interface PlanTopic {
  topicId: string;
  name: string;
  additionalReadings: Array<AdditionalReadings>;
  icon: string;
  steps: Array<PlanStep>;
}

export interface AdditionalReadings {
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

export interface IntentInput {
  location: MapLocation;
  intents: Array<string>;
}
