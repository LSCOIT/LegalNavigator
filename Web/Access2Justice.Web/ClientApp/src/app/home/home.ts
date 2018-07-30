export interface HomeRouter {
  overview: string;
  routerText: string;
  routerLink: string;
}
export interface HomeButton {
  overview: string;
  buttonText: string;
  buttonLink: string;
}
export interface Hero {
  home: Array<HomeRouter>
  description: string;
  image: Array<Image>;
}
export interface GuidedAssistant {
  homebutton: Array<HomeButton>
  description: Array<Description>;
  image: Array<Image>;
}
export interface TopicAndResources {
  homebutton: Array<HomeButton>
}
export interface Carousel {
  overview: Array<OverView>;
}
export interface Information {
  description: string;
  homebutton: Array<HomeButton>
  image: Array<Image>;
}
export interface Privacy {
  description: string;
  homebutton: Array<HomeButton>;
  image: Array<Image>
}
export interface Image {
  id: number;
  path: string
}
export interface Steps {
  order: number;
  descritpion: string
}
export interface Paragraph {
  text: string;
  routerText: string;
  routerLink: string;
}
export interface Description {
  steps: Array<Steps>;
  paragraphs: Array<Paragraph>;
}
export interface OverView {
  description: string;
  byName: string;
  location: string;
  image: Array<Image>;
}

