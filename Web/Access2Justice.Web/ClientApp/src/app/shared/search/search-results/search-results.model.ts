export interface IResourceFilter {
  ResourceType: string;
  ContinuationToken: any;
  TopicIds: any;
  PageNumber: number;
  Location: any;
}

export interface ILuisInput {
  Sentence: string;
  Location: any;
  TranslateFrom: string;
  TranslateTo: string;
  LuisTopScoringIntent: string;
}
