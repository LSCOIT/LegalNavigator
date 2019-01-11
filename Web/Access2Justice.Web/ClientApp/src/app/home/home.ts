import { Location } from "../about/about";

export interface Home {
  carousel: Carousel;
  guidedAssistantOverview: GuidedAssistantOverview;
  helpText: HelpText;
  hero: Hero;
  id: string;
  location: Array<Location>;
  privacy: Privacy;
  sponsorOverview: SponsorOverview;
  topicAndResources: TopicAndResources;
}

export interface Hero {
  heading?: string;
  description?: Description;
  image?: Image;
}

export interface Description {
  text?: string;
  textWithLink?: TextWithLink;
}

export interface TextWithLink {
  urlText?: string;
  url?: string;
}

export interface Image {
  source?: string;
  altText?: string;
}

export interface GuidedAssistantOverview {
  heading?: string;
  description?: DescriptionGuidedAssistant;
  button?: Button;
  image?: Image;
}

export interface DescriptionGuidedAssistant {
  steps?: Array<Steps>;
  text?: string;
  textWithLink?: TextWithLink;
}

export interface Steps {
  order?: number;
  descritpion?: string;
}
export interface Button {
  buttonText?: string;
  buttonAltText?: string;
  buttonLink?: string;
}

export interface TopicAndResources {
  heading?: string;
  button?: Button;
}

export interface Carousel {
  slides?: Array<Slides>;
}

export interface Slides {
  quote?: string;
  author?: string;
  location?: string;
  image?: Image;
}

export interface SponsorOverview {
  heading?: string;
  description?: string;
  sponsors?: Array<Sponsors>;
  button?: Button;
}

export interface Sponsors {
  source?: string;
  altText?: string;
}

export interface Privacy {
  heading?: string;
  description?: string;
  button?: Button;
  image?: Image;
}

export interface HelpText {
  beginningText: string;
  phoneNumber: string;
  endingText: string;
}
