import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';
import { Global } from "../../global";
import { ArrayUtilityService } from '../../shared/array-utility.service';
import { IResourceFilter } from '../../shared/search/search-results/search-results.model';
import { PersonalizedPlan, PersonalizedPlanTopic, ProfileResources, Resources, SavedResources, UserPlan, IntentInput } from './personalized-plan';
import { NgxSpinnerService } from 'ngx-spinner';
import { LocationDetails } from '../../shared/map/map';

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
  planTopic: PersonalizedPlanTopic;
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
  isIntent: boolean = false;
  intentInput: IntentInput;
  locationDetails: LocationDetails;

  constructor(private http: HttpClient,
    private arrayUtilityService: ArrayUtilityService,
    private toastr: ToastrService,
    private global: Global,
    private spinner: NgxSpinnerService) { }

  getActionPlanConditions(planId): Observable<any> {
    let params = new HttpParams()
      .set("personalizedPlanId", planId);
    return this.http.get<any>(api.planUrl + '?' + params, httpOptions);
  }

  getUserSavedResources(params): Observable<any> {
    return this.http.post<any>(api.getProfileUrl, params);
  }

  saveResources(resource: ProfileResources) {
    return this.http.post(api.userPlanUrl, resource, httpOptions);
  }

  userPlan(plan): Observable<any> {
    return this.http.post<any>(api.updateUserPlanUrl, plan, httpOptions);
  }

  getPersonalizedResources(resourceInput: IResourceFilter) {
    return this.http.put(api.getPersonalizedResourcesUrl, resourceInput, httpOptions);
  }

  getTopicDetails(intentInput): Observable<any> {
    return this.http.post<any>(api.getTopicDetailsUrl, intentInput, httpOptions);
  }

  getPlanDetails(topics, planDetailTags): any {
    this.topicsList = this.createTopicsList(topics);
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
    if (sessionStorage.getItem(this.global.topicsSessionKey)) {
      this.getTopicsFromGuidedAssistant();
    } else {
      this.saveResourceToProfilePostLogin(this.resourceTags);
    }
  }

  getTopicsFromGuidedAssistant() {
    this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    this.intentInput = { location: this.locationDetails.location, intents: JSON.parse(sessionStorage.getItem(this.global.topicsSessionKey)) };
    this.saveTopicsFromGuidedAssistantToProfile(this.intentInput, false);
  }

  saveTopicsFromGuidedAssistantToProfile(intentInput, isIntent) {
    this.isIntent = isIntent;
    if (this.isIntent) {
      this.resourceTags = [];
    }
    this.getTopicDetails(intentInput).subscribe(response => {
      if (response) {
        response.forEach(topic => {
          this.savedResources = { itemId: topic.id, resourceType: topic.resourceType, resourceDetails: {} };
          if (!(this.arrayUtilityService.checkObjectExistInArray(this.resourceTags, this.savedResources))) {
            this.resourceTags.push(this.savedResources);
          }
        });
        this.saveResourceToProfilePostLogin(this.resourceTags);
      }
    });
  }

  saveResourceToProfilePostLogin(savedResources) {
    this.resourceIndex = 0;
    this.resourceTags = [];
    let params = new HttpParams()
      .set("oid", this.global.userId)
      .set("type", "resources");
    this.getUserSavedResources(params)
      .subscribe(response => {
        if (response) {
          this.spinner.show();
          response.forEach(property => {
            if (property.resources) {
              property.resources.forEach(resource => {
                this.resourceTags.push(resource);
              });
            }
          });
        }
        this.checkExistingSavedResources(savedResources);
      });
  }

  checkExistingSavedResources(savedResources) {
    savedResources.forEach(savedResource => {
      if (this.arrayUtilityService.checkObjectExistInArray(this.resourceTags, savedResource)) {
        this.showWarning('Resource already saved to profile');
        this.spinner.hide();
      } else {
        this.resourceIndex++;
        this.resourceTags.push(savedResource);
      }
      if (this.resourceIndex > 0) {
        this.saveResourcesToProfile(this.resourceTags);
      }
    });
    if (sessionStorage.getItem(this.global.topicsSessionKey)) {
      sessionStorage.removeItem(this.global.topicsSessionKey);
    } else if (sessionStorage.getItem(this.global.sessionKey)) {
      sessionStorage.removeItem(this.global.sessionKey);
    }
  }

  saveResourcesToProfile(resourceTags) {
    this.profileResources = { oId: this.global.userId, resourceTags: resourceTags, type: 'resources' };
    this.saveResources(this.profileResources)
      .subscribe(() => {
        if (this.isIntent) {
          this.showSuccess("Topics added to Profile. You can view them later once you've completed the guided assistant.");
        } else {
          this.showSuccess('Resource saved to profile');
        }
        this.spinner.hide();
      });
  }

  saveBookmarkedResource(savedResource) {
    this.tempResourceStorage = [];
    let tempStorage = sessionStorage.getItem(this.global.sessionKey);
    if (tempStorage && tempStorage.length > 0) {
      tempStorage = JSON.parse(tempStorage);
      this.tempResourceStorage = tempStorage;
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
