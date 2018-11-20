import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../api/api';
import { MapLocation, LocationDetails } from './map/map';
import { About } from '../about/about';
import { PrivacyContent } from '../privacy-promise/privacy-promise';
import { HelpAndFaqs } from '../help-faqs/help-faqs';
import { Navigation } from './navigation/navigation';
import { Home } from '../home/home';

@Injectable()
export class StaticResourceService {

  name: any;
  constructor(private httpClient: HttpClient) { }

  mapLocation: MapLocation;
  state: string;
  aboutContent: About;
  privacyContent: PrivacyContent;
  helpAndFaqsContent: HelpAndFaqs;
  navigation: Navigation;
  homeContent: Home;
  locationDetails: LocationDetails;

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
    let location = this.loadStateName().state;
    if (!((location == "Alaska") || (location == "Hawaii"))) {
      location = "Default";
    }
    return location;
  }

  getStaticContents(location) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.httpClient.post(api.getContentsUrl, location, httpOptions);
  }
}
