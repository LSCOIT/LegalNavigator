export interface Topic {
    id: string;
    type: string;
    icon: string;
    keywords: string;
    description: string;
    lang: string;
    subtopics: Array<object>;
}
