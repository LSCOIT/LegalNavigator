export interface PrivacyContent {
  id: string;
  description: string;
  image: Image;
  details: Array<Details>;
}
export interface Image {
  source: string;
  altText: string;
}
export interface Details {
  title: string;
  description: string;
}

