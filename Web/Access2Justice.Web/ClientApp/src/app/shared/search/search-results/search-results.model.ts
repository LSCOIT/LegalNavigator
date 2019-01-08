export interface IResourceFilter {
  ResourceType: string;
  ContinuationToken: any;
  TopicIds: any[];
  ResourceIds: any[];
  PageNumber: number;
  Location: any;
  IsResourceCountRequired: boolean;
  IsOrder: boolean;
  OrderByField: string;
  OrderBy: string;
}

export interface ILuisInput {
  Sentence: string;
  Location: any;
  TranslateFrom?: string;
  TranslateTo?: string;
  LuisTopScoringIntent?: string;
  OrderByField?: string;
  OrderBy?: string;
}

