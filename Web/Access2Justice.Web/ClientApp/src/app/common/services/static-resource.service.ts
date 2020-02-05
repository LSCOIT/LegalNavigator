import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { api } from "../../../api/api";
import { About } from "../../about/about";
import { GuidedAssistant } from "../../guided-assistant/guided-assistant";
import { PersonalizedPlanDescription } from "../../guided-assistant/personalized-plan/personalized-plan";
import { HelpAndFaqs } from "../../help-faqs/help-faqs";
import { Home } from "../../home/home";
import { PrivacyContent } from "../../privacy-promise/privacy-promise";
import { LocationDetails, MapLocation } from '../map/map';
import { Navigation } from '../navigation/navigation';

@Injectable({
  providedIn: 'root'
})
export class StaticResourceService {
  constructor(private httpClient: HttpClient) { }

  name: any;
  mapLocation: MapLocation;
  state: string;
  aboutContent: About;
  privacyContent: PrivacyContent;
  helpAndFaqsContent: HelpAndFaqs;
  navigation: Navigation;
  homeContent: Home;
  locationDetails: LocationDetails;
  PersonalizedPlanDescription: PersonalizedPlanDescription;
  GuidedAssistantPageContent: GuidedAssistant;
  location: string = "";

  loadStateName(): MapLocation {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.locationDetails = JSON.parse(
        sessionStorage.getItem("globalMapLocation")
      );
      this.mapLocation = this.locationDetails.location;
      this.state = this.locationDetails.displayLocationDetails 
        ? this.locationDetails.displayLocationDetails.address 
        : this.locationDetails.location.state;
      return this.mapLocation;
    } else {
      return { state: "Default" };
    }
  }

  getLocation() {
    if (this.mapLocation) {
      this.location = this.mapLocation.state;
    }
    if (!(this.location == "AK" || this.location == "HI")) {
      this.location = "Default";
    }
    return this.location; 
  }

  getStaticContents(location) {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    };
    return this.httpClient.post(api.getContentsUrl, location, httpOptions);
  }
}
