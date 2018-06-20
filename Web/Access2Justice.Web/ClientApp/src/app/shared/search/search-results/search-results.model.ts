export interface IResourceFilter {
  ResourceType: any;
  ContinuationToken: any;
  TopicIds: any;
  PageNumber: any;
  Location: any;
}



export interface ILuisInput {
  Sentence: string;
  Location: any;
  TranslateFrom: string;
  TranslateTo: string;
}
