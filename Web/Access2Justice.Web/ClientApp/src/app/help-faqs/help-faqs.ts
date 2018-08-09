export interface HelpAndFaqs {
  id: string;
  description: string;
  image: ImageUrl;
  imageExpand: ImageUrl;
  imageCollapse: ImageUrl;
  faqs: Array<Faq>;
}
export interface ImageUrl {
  source: string;
  altText: string;
}
export interface Faq {
  question: string;
  answer: Answer;
}
export interface Answer {
  description: string;
}
