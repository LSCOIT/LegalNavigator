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

