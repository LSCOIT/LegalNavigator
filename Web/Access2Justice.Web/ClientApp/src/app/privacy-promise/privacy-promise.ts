import { Location } from "../about/about";

export interface PrivacyContent {
  id: string;
  description: string;
  image: Image;
  details: Array<Details>;
  location: Array<Location>;
}
export interface Image {
  source: string;
  altText: string;
}
export interface Details {
  title: string;
  description: string;
}

