export interface Navigation {
  id: string;
  language: Language;
  location: Location;
  privacyPromise: ButtonImage;
  helpAndFAQ: ButtonImage;
  login: ButtonImage;
  logo: Logo;
  home: Button;
  guidedAssistant: Button;
  topicAndResources: Button;
  about: Button;
  search: ButtonImage;
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

export interface ButtonImage {
  button: Button;
  image: Image;
}

export interface Logo {
  firstLogo: string;
  secondLogo: string;
  link: string;
}
