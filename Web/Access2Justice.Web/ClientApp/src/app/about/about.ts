import { Button, Image } from "../home/home";

export interface About {
  name: string;
  location: Array<Location>;
  aboutImage: Image;
  mission: Mission;
  service: Service;
  privacyPromise: PrivacyPromise;
}

export interface Location {
  state: string;
  city: string;
  county: string;
  zipCode: string;
  locality: string;
  address: string;
}

export interface Mission {
  title: string;
  description: string;
  image: Image;
}

export interface Service {
  title: string;
  description: string;
  image: Image;
  guidedAssistantButton: Button;
  topicsAndResourcesButton: Button;
}

export interface PrivacyPromise {
  title: string;
  description: string;
  image: Image;
  privacyPromiseButton: Button;
}
