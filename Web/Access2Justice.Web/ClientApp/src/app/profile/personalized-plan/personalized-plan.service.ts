import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { PersonalizedPlanCondition, Resources } from './personalized-plan';
import { api } from '../../../api/api';
import { IResourceFilter } from '../../shared/search/search-results/search-results.model';

@Injectable()
export class PersonalizedPlanService {

  resource: Resources;
  tempStorage: Array<Resources> = [];
  resoureStorage: any = [];
  resourceTemp: any = [];
  sessionKey: string = "bookmarkedResource";
  isObjectExists: boolean = false;
  topics: string[] = [];
  resources: string[] = [];
  planId: string;

  constructor(private http: HttpClient) { }

  getActionPlanConditions(id): Observable<any> {
    //return this.http.get<PersonalizedPlanCondition>(api.actionPlanUrl + '/' + id);
    return this.http.get<PersonalizedPlanCondition>(api.planUrl + '/' + id);
  }

  saveResourcesToSession(resources) {

    this.resoureStorage = sessionStorage.getItem(this.sessionKey);

    if (this.resoureStorage != undefined && this.resoureStorage.length > 0) {
      this.tempStorage = JSON.parse(this.resoureStorage);
      for (var i = 0; i < this.tempStorage.length; i++) {
        if (JSON.stringify(this.tempStorage[i]) == JSON.stringify(resources)) {
          this.isObjectExists = true;
        }
      }
      if (!this.isObjectExists) {
        this.tempStorage.push(resources);
        sessionStorage.setItem(this.sessionKey, JSON.stringify(this.tempStorage));
      }
    }
    else {
      this.tempStorage = [resources];
      sessionStorage.setItem(this.sessionKey, JSON.stringify(this.tempStorage));
    }
  }

  getPersonalizedResources(resourceInput: IResourceFilter) {
    this.getBookmarkedData();
    if (this.topics) {
      resourceInput.TopicIds = this.topics;//["f102bfae-362d-4659-aaef-956c391f79de", "addf41e9-1a27-4aeb-bcbb-7959f95094ba"];
    }
    if (this.resources) {
      resourceInput.ResourceIds = this.resources; //["2225cb29-2442-42ac-a4f5-9e88a7aabc4a", "77d301e7-6df2-612e-4704-c04edf271806"];
    }
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    return this.http.put(api.getPersonalizedResourcesUrl, resourceInput, httpOptions);
  }


  getBookmarkedData() {
    this.topics = [];
    this.resources = [];
    var resourceData = sessionStorage.getItem(this.sessionKey);
    if (resourceData != undefined && resourceData.length > 0) {
      this.tempStorage = JSON.parse(resourceData);
      for (var i = 0; i < this.tempStorage.length; i++) {
        if (this.tempStorage[i].url.startsWith("/subtopics")) {
          this.topics.push(this.tempStorage[i].itemId);
        }
        if (this.tempStorage[i].url.startsWith("/resource")) {
          this.resources.push(this.tempStorage[i].itemId);
        }
        if (this.tempStorage[i].url.startsWith("/personalizedplan")) {
          this.planId = this.tempStorage[i].itemId;
        }
      }
      console.log(this.planId);
    }
  }
}
