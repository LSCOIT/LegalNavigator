interface IResourceFilter {
  ResourceType: any;
  ContinuationToken: any;
  TopicIds: any;
  PageNumber: any;
  Location: any;
}



interface ILuisInput {
  Sentence: string;
  Location: any;
  TranslateFrom: string;
  TranslateTo: string;
}
