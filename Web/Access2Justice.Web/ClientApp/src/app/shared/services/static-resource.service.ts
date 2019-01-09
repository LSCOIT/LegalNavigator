import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';
import { MapLocation, LocationDetails } from './../map/map';
import { About } from '../../about/about';
import { PrivacyContent } from '../../privacy-promise/privacy-promise';
import { HelpAndFaqs } from '../../help-faqs/help-faqs';
import { Navigation } from './../navigation/navigation';
import { Home } from '../../home/home';
import { PersonalizedPlanDescription } from '../../guided-assistant/personalized-plan/personalized-plan';
import { GuidedAssistant } from '../../guided-assistant/guided-assistant';

@Injectable()
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
  location: string='';

  loadStateName(): MapLocation {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.mapLocation = this.locationDetails.location;
      this.state = this.locationDetails.displayLocationDetails.address;
      return this.mapLocation;
    } else {
      return { state: "Default" }
    }
  }

  getLocation() {
    if (this.mapLocation) {
      this.location = this.mapLocation.state;
    }
    if (!((this.location == "AK") || (this.location == "HI"))) {
      this.location = "Default";
    }
    return this.location;
  }

  getStaticContents(location) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.httpClient.post(api.getContentsUrl, location, httpOptions);
  }
}
