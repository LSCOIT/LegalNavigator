//need to update this and topic service spec file once database schema is completed
export interface Topic {
    id: string;
    title: string;
    icon: string;
}

export interface Subtopic {  
    id: string,
    title: string,
    subtopics: Array<object>;
}

