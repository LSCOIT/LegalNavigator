//need to update this and topic service spec file once database schema is completed
export interface Topic {
    id: string;
    name: string;
    icon: string;
}

export interface Subtopic {  
    id: string,
    name: string,
    subtopics: Array<object>;
}

export interface ITopicInput {
  Id: string;
  Location: any;
}

export interface ISubtopicGuidedInput {
  activeId: string;
  name: string;
}

