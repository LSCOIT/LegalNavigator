import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { api } from "../../../api/api";
import { Global } from "../../global";
import { LocationDetails, MapLocation } from "../../common/map/map";
import { ITopicInput, Topic } from "./topic";

const httpOptions = {
  headers: new HttpHeaders({ "Content-Type": "application/json" })
};

@Injectable({
  providedIn: 'root'
})

export class TopicService {
  topicInput: ITopicInput = {
    Id: "",
    Location: "",
    IsShared: false };
  mapLocation: MapLocation = {
    state: "",
    city: "",
    county: "",
    zipCode: "" };
  locationDetails: LocationDetails;

  constructor(
    private http: HttpClient,
    private global: Global
  ) {}

  loadStateName(): MapLocation {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.locationDetails = JSON.parse(
        sessionStorage.getItem("globalMapLocation")
      );
      this.mapLocation = this.locationDetails.location;
      return this.mapLocation;
    }
  }

  getTopics(): Observable<any> {
    this.mapLocation = this.loadStateName();
    if (this.mapLocation) {
      this.mapLocation.city = "";
      this.mapLocation.county = "";
      this.mapLocation.zipCode = "";
    }
    return this.http.post<Topic>(
      api.topicUrl,
      JSON.stringify(this.mapLocation),
      httpOptions
    );
  }

  getSubtopics(id): Observable<any> {
    this.buildParams(id);
    return this.http.post<Topic>(
      api.subtopicUrl,
      JSON.stringify(this.topicInput),
      httpOptions
    );
  }

  getSubtopicDetail(id): Observable<any> {
    this.buildParams(id);
    return this.http.post<Topic>(
      api.subtopicDetailUrl,
      JSON.stringify(this.topicInput),
      httpOptions
    );
  }

  getDocumentData(id): Observable<any> {
    this.buildParams(id);
    return this.http.post<Topic>(
      api.getDocumentUrl,
      JSON.stringify(this.topicInput),
      httpOptions
    );
  }

  buildParams(id) {
    this.topicInput.Id = id;
    this.topicInput.Location = this.loadStateName();
    if (location.pathname.indexOf(this.global.shareRouteUrl) >= 0) {
      this.topicInput.IsShared = true;
    }
  }

  printTopic(params): Observable<any> {
    return this.http.get<any>(api.printTopicUrl + "?" + params, {responseType: 'arraybuffer' as 'json'});
  }

  printResourceTopic(params): Observable<any> {
    return this.http.get<any>(api.printSubtopicUrl + "?" + params, {responseType: 'arraybuffer' as 'json'});
  }
}
