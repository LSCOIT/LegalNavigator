export interface IResourceFilter {
  ResourceType: string;
  ContinuationToken: any;
  TopicIds: any[];
  ResourceIds: any[];
  PageNumber: number;
  Location: any;
  IsResourceCountRequired: boolean;
}

export interface ILuisInput {
  Sentence: string;
  Location: any;
  TranslateFrom: string;
  TranslateTo: string;
  LuisTopScoringIntent: string;
}

