import { Location } from "../about/about";

export interface HelpAndFaqs {
  id: string;
  description: string;
  image: ImageUrl;
  faqs: Array<Faq>;
  location: Array<Location>;
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
