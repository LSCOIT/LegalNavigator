import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Resources, PersonalizedPlanTopic, PersonalizedPlan, ProfileResources, SavedResources, UserPlan } from './personalized-plan';
import { api } from '../../../api/api';
import { IResourceFilter } from '../../shared/search/search-results/search-results.model';
import { ArrayUtilityService } from '../../shared/array-utility.service';
import { ToastrService } from 'ngx-toastr';
import { Global } from "../../global";
import { resource } from 'selenium-webdriver/http';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable()
export class PersonalizedPlanService {
  tempStorage: Array<Resources> = [];
  resoureStorage: any = [];
  isObjectExists: boolean = false;
  topics: string[] = [];
  resources: string[] = [];
  planId: string;
  planTopic: PersonalizedPlanTopic = { topic: {}, isSelected: true };
  topicsList: Array<PersonalizedPlanTopic> = [];
  planDetailTags: any;
  tempPlanDetailTags: any;  
  planDetails: any = [];
  profileResources: ProfileResources = { oId: '', resourceTags: [], type: '' };
  savedResources: SavedResources;
  resourceTags: Array<SavedResources> = [];
  resourceIds: Array<string>;
  personalizedPlan: PersonalizedPlan;
  userPersonalizedPlan: UserPlan = { oId: '', plan: this.personalizedPlan };
  resourceIndex: number;
  tempResourceStorage: any = [];

  constructor(private http: HttpClient,
              private arrayUtilityService: ArrayUtilityService,
              private toastr: ToastrService,
              private global: Global) { }
  
  getActionPlanConditions(planId): Observable<any> {
    return this.http.get<PersonalizedPlan>(api.planUrl, planId);
  }

  getUserSavedResources(params): Observable<any> {
    return this.http.post<any>(api.getProfileUrl, params);
  }

  saveResources(resource: ProfileResources) {
    return this.http.post(api.userPlanUrl, resource, httpOptions);
  }

  userPlan(plan: PersonalizedPlan) {
    this.userPersonalizedPlan = { oId: this.global.userId, plan: plan };
    return this.http.post<any>(api.updateUserPlanUrl, this.userPersonalizedPlan, httpOptions);
  }

  getPersonalizedResources(resourceInput: IResourceFilter) {
    return this.http.put(api.getPersonalizedResourcesUrl, resourceInput, httpOptions);
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

  saveResourcesToUserProfile() {
    this.resoureStorage = [];
    this.savedResources = { itemId: '', resourceType: '', resourceDetails: {} };
    this.resoureStorage = sessionStorage.getItem(this.global.sessionKey);
    if (this.resoureStorage && this.resoureStorage.length > 0) {
      this.resoureStorage = JSON.parse(this.resoureStorage);
    }
    this.resourceTags = [];
    this.resoureStorage.forEach(resource => {
      this.resourceTags.push(resource);
    });
    this.saveResourcesToProfile(this.resourceTags);
  }

  saveResourcesToProfile(savedResources) {
    this.resourceIndex = 0;
    this.resourceTags = [];
    let params = new HttpParams()
      .set("oid", this.global.userId)
      .set("type", "resources");
    this.getUserSavedResources(params)
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
        savedResources.forEach(savedResource => {
          if (this.arrayUtilityService.checkObjectExistInArray(this.resourceTags, savedResource)) {
            this.showWarning('Resource already saved to profile');
          } else {
            this.resourceIndex++;
            this.resourceTags.push(savedResource);
          }
          if (this.resourceIndex > 0) {
            this.saveResourceToProfile(this.resourceTags);
          }
        });
        sessionStorage.removeItem(this.global.sessionKey);
      });
  }

  saveResourceToProfile(resourceTags) {
    this.profileResources = { oId: this.global.userId, resourceTags: resourceTags, type: 'resources' };
    this.saveResources(this.profileResources)
      .subscribe(() => {
        this.showSuccess('Resource saved to profile');
      });
  }

  saveBookmarkedResource(savedResource) {
    this.tempResourceStorage = [];
    let tempStorage = sessionStorage.getItem(this.global.sessionKey);
    if (tempStorage && tempStorage.length > 0) {
      tempStorage = JSON.parse(tempStorage);
      this.tempResourceStorage =  tempStorage;
    }
    if (savedResource) {
      if (!this.arrayUtilityService.checkObjectExistInArray(this.tempResourceStorage, savedResource)) {
        this.tempResourceStorage.push(savedResource);
        this.showSuccess('Resource saved to session');
      } else {
        this.showWarning('Resource already saved to session');
      }
    }
    if (this.tempResourceStorage.length > 0) {
      sessionStorage.setItem(this.global.sessionKey, JSON.stringify(this.tempResourceStorage));
    }
  }

  getResourceIds(resources): Array<string> {
    this.resourceIds = [];
    resources.forEach(resource => {
      this.resourceIds.push(resource.id);
    });
    return this.resourceIds;
  }

  showSuccess(message) {
    this.toastr.success(message);
  }

  showWarning(message) {
    this.toastr.warning(message);
  }

}
