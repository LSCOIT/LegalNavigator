export interface Topic {
  id: string;
  name: string;
  icon: string;
}

export interface Subtopic {
  id: string;
  name: string;
  subtopics: Array<object>;
}

export interface ITopicInput {
  Id: string;
  Name: string;
  Location: any;
  IsShared: boolean;
}
export interface ISubtopicGuidedInput {
  activeId: string;
  name: string;
  guidedAssistantId?: string
}
