import { Location } from "../about/about";

export interface GuidedAssistant {
  id: string;
  name: string;
  description: string;
  location: Array<Location>;
  organizationalUnit: string;
}


