import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Resources, PersonalizedPlanTopic, PersonalizedPlan, ProfileResources, SavedResources } from './personalized-plan';
import { api } from '../../../api/api';
import { IResourceFilter } from '../../shared/search/search-results/search-results.model';
import { ArrayUtilityService } from '../../shared/array-utility.service';

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
  planTopic: PersonalizedPlanTopic = { topic: {}, isSelected: true };
  topicsList: Array<PersonalizedPlanTopic> = [];
  planDetailTags: any;
  tempPlanDetailTags: any;
  userId: string;
  planDetails: any = [];
  planSessionKey: string = "bookmarkPlanId";
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  savedResources: SavedResources;
  resourceTags: Array<SavedResources> = [];

  constructor(private http: HttpClient, private arrayUtilityService: ArrayUtilityService) { }

  getActionPlanConditions(id): Observable<any> {
    return this.http.get<PersonalizedPlan>(api.planUrl + '/' + id);
  }

  getUserPlanId(oid): Observable<any> {
    return this.http.get<PersonalizedPlan>(api.getUserProfileUrl + '/' + oid);
  }

  getUserSavedResources(oid): Observable<any> {
    return this.http.get<PersonalizedPlan>(api.getProfileUrl + '/' + oid);
  }

  getMarkCompletedUpdatedPlan(updatePlan) {
    return this.http.post(api.updatePlanUrl, updatePlan, httpOptions);
  }

  saveResources(resource: ProfileResources) {
    return this.http.post(api.userPlanUrl, resource, httpOptions);
  }

  userPlan(plan: PersonalizedPlan) {
    return this.http.post<any>(api.updateUserPlanUrl, plan, httpOptions);
  }

  savePersonalizedPlanToProfile(params): Observable<any> {
    return this.http.post(api.updateUserProfileUrl, params);
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
      this.isObjectExists = this.arrayUtilityService.checkObjectExistInArray(this.tempStorage, resources);
      if (!this.isObjectExists) {
        this.tempStorage.push(resources);
        sessionStorage.setItem(this.sessionKey, JSON.stringify(this.tempStorage));
      }
    } else {
      this.tempStorage = [resources];
      sessionStorage.setItem(this.sessionKey, JSON.stringify(this.tempStorage));
    }
  }

  savePlanToSession(planId) {
    sessionStorage.setItem(this.planSessionKey, JSON.stringify(planId));
  }

  getPlanDetails(topics, planDetailTags): any {
    this.topicsList = this.createTopicsList(this.topics);
    this.planDetails = this.displayPlanDetails(planDetailTags, this.topicsList);
    return this.planDetails;
  }

  createTopicsList(topics): Array<PersonalizedPlanTopic> {
    this.topicsList = [];
    topics.forEach(topic => {
      this.planTopic = { topic: topic, isSelected: true };
      this.topicsList.push(this.planTopic);
    });
    return this.topicsList;
  }

  displayPlanDetails(planDetailTags, topicsList): any {
    this.planDetailTags = planDetailTags;
    this.topicsList = topicsList;
    this.tempPlanDetailTags = JSON.parse(JSON.stringify(this.planDetailTags));
    for (let index = 0, planTagIndex = 0; index < this.topicsList.length; index++ , planTagIndex++) {
      if (!this.topicsList[index].isSelected) {
        this.tempPlanDetailTags.topics.splice(planTagIndex, 1);
        planTagIndex--;
      }
    }
    return this.tempPlanDetailTags;
  }

  getUserId(): string {
    let profileData = sessionStorage.getItem("profileData");
    if (profileData != undefined) {
      profileData = JSON.parse(profileData);
      return profileData["UserId"];
    }
  }

  saveResourcesToProfile() {
    this.userId = this.getUserId();
    this.profileResources.resourceTags = [];
    this.savedResources = { itemId: '', resourceType: '', resourceDetails: {} };
    this.getUserSavedResources(this.userId)
      .subscribe(response => {
        if (response) {
          response.forEach(property => {
            if (property.resources) {
              property.resources.forEach(resource => {
                this.resourceTags.push(resource);
              });
            }
          });
        }
        this.resoureStorage = sessionStorage.getItem(this.sessionKey);
        if (this.resoureStorage && this.resoureStorage.length > 0) {
          this.resoureStorage = JSON.parse(this.resoureStorage);
        }
        this.savedResources = {
          itemId: this.resoureStorage[0].id,
          resourceType: this.resoureStorage[0].type, resourceDetails: this.resoureStorage[0].resourceDetails
        };
        if (!this.arrayUtilityService.checkObjectExistInArray(this.resourceTags, this.savedResources)) {
          this.resourceTags.push(this.savedResources);
          this.saveResourceToProfile(this.resourceTags);
        }
      });
  }

  saveResourceToProfile(resourceTags) {
    this.profileResources = { oId: this.userId, resourceTags: resourceTags, type: 'resources' };
    this.saveResources(this.profileResources)
      .subscribe(() => {
        //this.isSavedPlan = true;
        console.log('Resource saved to profile');
      });
  }


}
