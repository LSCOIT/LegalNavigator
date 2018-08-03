export interface Navigation {
  id: string;
  language: Language;
  location: Location;
  privacyPromise: PrivacyPromise;
  helpAndFAQ: HelpAndFAQ;
  login: Login;
  logo: Logo;
  home: Home;
  guidedAssistant: GuidedAssistant;
  topicAndResources: TopicAndResources;
  about: About;
  search: Search;
}

export interface Button {
    buttonText: string;
    buttonAltText: string;
    buttonLink: string;
}

export interface Image {
  source: string;
  altText: string;
}

export interface Language {
  button: Button;
  navigationImage: Image;
  dropDownImage: Image
}

export interface Location {
  text: string;
  altText: string;
  button: Button;
  image: Image;
}

export interface Logo {
  firstLogo: string;
  secondLogo: string;
  link: string;
}

export interface PrivacyPromise {
  button: Button;
  image: Image;
}

export interface HelpAndFAQ {
  button: Button;
  image: Image;
}

export interface Login {
  button: Button;
  image: Image;
}

export interface Home {
  button: Button;
}

export interface GuidedAssistant {
  button: Button;
}

export interface TopicAndResources {
  button: Button;
}

export interface About {
  button: Button;
}

export interface Search {
  button: Button;
  image: Image;
}
