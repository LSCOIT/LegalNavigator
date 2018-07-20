import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { PlanSteps, Resources } from './personalized-plan';
import { api } from '../../../api/api';
import { IResourceFilter } from '../../shared/search/search-results/search-results.model';
import { CommonService } from '../../shared/common.service';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable()
export class PersonalizedPlanService {
  tempStorage: Array<Resources> = [];
  resoureStorage: any = [];
  sessionKey: string = "bookmarkedResource";
  isObjectExists: boolean = false;
  topics: string[] = [];
  resources: string[] = [];
  planId: string;
  userId: string;

  constructor(private http: HttpClient, private commonService: CommonService) { }

  getActionPlanConditions(id): Observable<any> {
    return this.http.get<PlanSteps>(api.planUrl + '/' + id);
  }

  getUserPlanId(oid): Observable<any> {
    return this.http.get<PlanSteps>(api.getProfileUrl + '/' + oid);
  }

  getMarkCompletedUpdatedPlan(updatePlan) {
    return this.http.post(api.updatePlanUrl, updatePlan, httpOptions);
  }

  userPlan(plan) {
    return this.http.post(api.userPlanUrl, plan, httpOptions);
  }

  getBookmarkedData() {
    this.topics = [];
    this.resources = [];
    let resourceData = sessionStorage.getItem(this.sessionKey);
    if (resourceData && resourceData.length > 0) {
      this.tempStorage = JSON.parse(resourceData);
      for (let index = 0; index < this.tempStorage.length; index++) {
        if (this.tempStorage[index].type === "Topics") {
          this.topics.push(this.tempStorage[index].itemId);
        } else if (this.tempStorage[index].type === "Plan") {
          this.planId = this.tempStorage[index].itemId;
        } else {
          this.resources.push(this.tempStorage[index].itemId);
        }
      }
    }
  }

  getPersonalizedPlan(): string {
    this.getBookmarkedData();
    return this.planId;
  }

  getPersonalizedResources(resourceInput: IResourceFilter) {
    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      this.userId = profileData["UserId"];
    }
    if (this.userId === undefined) {
      this.getBookmarkedData();
      if (this.topics) {
        resourceInput.TopicIds = this.topics;
      }
      if (this.resources) {
        resourceInput.ResourceIds = this.resources;
      }
    }
    return this.http.put(api.getPersonalizedResourcesUrl, resourceInput, httpOptions);
  }

  //Below method need to be modified to store data in session if user is not logged in
  saveResourcesToSession(resources) {
    this.resoureStorage = sessionStorage.getItem(this.sessionKey);
    if (this.resoureStorage && this.resoureStorage.length > 0) {
      this.tempStorage = JSON.parse(this.resoureStorage);
      this.isObjectExists = this.commonService.checkObjectExistInArray(this.tempStorage, resources);
      if (!this.isObjectExists) {
        this.tempStorage.push(resources);
        sessionStorage.setItem(this.sessionKey, JSON.stringify(this.tempStorage));
      }
    } else {
      this.tempStorage = [resources];
      sessionStorage.setItem(this.sessionKey, JSON.stringify(this.tempStorage));
    }
  }

}
