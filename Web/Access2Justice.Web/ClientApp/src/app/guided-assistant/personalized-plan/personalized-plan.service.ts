import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { PlanSteps, Resources } from './personalized-plan';
import { api } from '../../../api/api';
import { IResourceFilter } from '../../shared/search/search-results/search-results.model';
import { environment } from '../../../environments/environment';

@Injectable()
export class PersonalizedPlanService {
  tempStorage: Array<Resources> = [];
  resoureStorage: any = [];
  sessionKey: string = "bookmarkedResource";
  isObjectExists: boolean = false;
  topics: string[] = [];
  resources: string[] = [];
  planId: string;

  constructor(private http: HttpClient) { }

  getActionPlanConditions(id): Observable<any> {
    return this.http.get<PlanSteps>(api.planUrl + '/' + id);
  }

  saveResourcesToSession(resources) {
    this.resoureStorage = sessionStorage.getItem(this.sessionKey);
    if (this.resoureStorage && this.resoureStorage.length > 0) {
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
    if (!environment.userId) {
      this.getBookmarkedData();
      if (this.topics) {
        resourceInput.TopicIds = this.topics;
      }
      if (this.resources) {
        resourceInput.ResourceIds = this.resources;
      }
    }
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    return this.http.put(api.getPersonalizedResourcesUrl, resourceInput, httpOptions);
  }

  getPersonalizedPlan(): string {
      this.getBookmarkedData();
      return this.planId;
  }

  getUserPlanId(oid): Observable<any> {
    return this.http.get<PlanSteps>(api.getProfileUrl + '/' + oid);
  }
  
  getBookmarkedData() {
    this.topics = [];
    this.resources = [];
    var resourceData = sessionStorage.getItem(this.sessionKey);
    if (resourceData && resourceData.length > 0) {
      this.tempStorage = JSON.parse(resourceData);
      for (var i = 0; i < this.tempStorage.length; i++) {
        if (this.tempStorage[i].url.startsWith("/subtopics")) {
          this.topics.push(this.tempStorage[i].itemId);
        }
        if (this.tempStorage[i].url.startsWith("/resource")) {
          this.resources.push(this.tempStorage[i].itemId);
        }
        if (this.tempStorage[i].url.startsWith("/plan")) {
          this.planId = this.tempStorage[i].itemId;
        }
      }
    }
  }

  getMarkCompletedUpdatedPlan(updatePlan) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post(api.updatePlanUrl, updatePlan, httpOptions);
  }

  userPlan(plan) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post(api.userPlanUrl, plan, httpOptions);
  }

}
