import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

import { api } from '../../../api/api';
import { Global } from '../../global';
import { LocationDetails } from '../../common/map/map';
import { IResourceFilter } from '../../common/search/search-results/search-results.model';
import { ArrayUtilityService } from '../../common/services/array-utility.service';
import { IntentInput, PersonalizedPlan, PersonalizedPlanTopic, ProfileResources, SavedResource, RemovedElem} from './personalized-plan';

const httpOptions = {
  headers: new HttpHeaders({'Content-Type': 'application/json'})
};

@Injectable({
  providedIn: 'root'
})
export class PersonalizedPlanService {
  resourceStorage: any = [];
  tempResourceStorage: any = [];
  savedResources: SavedResource;
  resourceTags: SavedResource[] = [];
  topics: string[] = [];
  resources: string[] = [];
  planTopic: PersonalizedPlanTopic;
  topicsList: PersonalizedPlanTopic[] = [];
  planDetailTags: any;
  tempPlanDetailTags: any;
  planDetails: any = [];
  profileResources: ProfileResources = {oId: '', resourceTags: [], type: ''};
  resourceIds: string[];
  resourceIndex: number;
  isIntent = false;
  intentInput: IntentInput;
  locationDetails: LocationDetails;

  constructor(
    private http: HttpClient,
    private arrayUtilityService: ArrayUtilityService,
    private toastr: ToastrService,
    private global: Global,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {
  }

  getActionPlanConditions(planId): Observable<any> {
    return this.http.get<any>(api.planUrl, {
      ...httpOptions,
      params: {personalizedPlanId: planId}
    });
  }

  getPersonalizedPlan(): Observable<PersonalizedPlan> {
    return this.http.post<any>(api.getProfileUrl, null, {params: {oid: this.getUserId(), type: 'plan'}}).pipe(
      map(response => {
        if (Array.isArray(response)) {
          return response[0];
        } else {
          return null;
        }
      })
    );
  }

  getUserSavedResources(type = 'resources'): Observable<SavedResource[]> {
    return this.http.post<any>(api.getProfileUrl, null, {params: {oid: this.getUserId(), type: type}}).pipe(
      map(response => {
        if (Array.isArray(response)) {
          return response[0] ? response[0].resources || [] : [];
        } else {
          return [];
        }
      })
    );
  }

  getPersonalizedResources(type = 'resources'): Observable<any> {
    return this.getUserSavedResources(type).pipe(
      switchMap(resources => {
        const webResources = [];

        const resourcesFilter: IResourceFilter = {
          TopicIds: [],
          ResourceIds: [],
          ResourceType: 'ALL',
          PageNumber: 0,
          ContinuationToken: null,
          Location: null,
          IsResourceCountRequired: false,
          IsOrder: false,
          OrderByField: '',
          OrderBy: ''
        };

        resources.forEach(resource => {
          if (resource.resourceType === 'Topics') {
            resourcesFilter.TopicIds.push(resource.itemId);
          } else if (resource.resourceType === 'WebResources') {
            webResources.push(resource);
          } else {
            resourcesFilter.ResourceIds.push(resource.itemId);
          }
        });

        const personalizedResources = {
          resources: [],
          topics: [],
          webResources
        };

        return this.http.put<any>(api.getPersonalizedResourcesUrl, resourcesFilter, httpOptions).pipe(
          map(response => {
            if (response) {
              personalizedResources.resources = response['resources'];
              personalizedResources.topics = response['topics'];
            }
            return personalizedResources;
          })
        );
      })
    );
  }

  saveResources(resource: ProfileResources) {
    return this.http.post(api.userPlanUrl, resource, httpOptions);
  }

  removeSharedResources(resource: RemovedElem){
    return this.http.post(api.removeSharedResources, resource, httpOptions);
  }

  userPlan(plan): Observable<any> {
    return this.http.post<any>(api.updateUserPlanUrl, plan, httpOptions);
  }

  getTopicDetails(intentInput): Observable<any> {
    return this.http.post<any>(
      api.getTopicDetailsUrl,
      intentInput,
      httpOptions
    );
  }

  getPlanDetails(topics, planDetailTags): any {
    this.topicsList = this.createTopicsList(topics);
    this.planDetails = this.displayPlanDetails(planDetailTags, this.topicsList);
    return this.planDetails;
  }

  createTopicsList(topics): Array<PersonalizedPlanTopic> {
    this.topicsList = [];
    topics.forEach(topic => {
      this.planTopic = {topic: topic, isSelected: true};
      this.topicsList.push(this.planTopic);
    });
    return this.topicsList;
  }

  displayPlanDetails(planDetailTags, topicsList): any {
    this.planDetailTags = planDetailTags;
    this.topicsList = topicsList;
    this.tempPlanDetailTags = JSON.parse(JSON.stringify(this.planDetailTags));
    for (
      let index = 0, planTagIndex = 0;
      index < this.topicsList.length;
      index++, planTagIndex++
    ) {
      if (!this.topicsList[index].isSelected) {
        this.tempPlanDetailTags.topics.splice(planTagIndex, 1);
        planTagIndex--;
      }
    }
    return this.tempPlanDetailTags;
  }

  saveResourcesToUserProfile() {
    this.resourceStorage = [];
    this.savedResources = {itemId: '', resourceType: '', resourceDetails: {}};
    if (sessionStorage.getItem(this.global.sessionKey)) {
      this.resourceStorage = sessionStorage.getItem(this.global.sessionKey);
      if (this.resourceStorage && this.resourceStorage.length > 0) {
        this.resourceStorage = JSON.parse(this.resourceStorage);
      }
      this.resourceTags = [];
      this.resourceStorage.forEach(resource => {
        this.resourceTags.push(resource);
      });
    }
    if (sessionStorage.getItem(this.global.topicsSessionKey)) {
      this.getTopicsFromGuidedAssistant();
    } else {
      this.saveResourceToProfilePostLogin(this.resourceTags);
    }
  }

  getTopicsFromGuidedAssistant() {
    this.locationDetails = JSON.parse(
      sessionStorage.getItem('globalMapLocation')
    );
    this.intentInput = {
      location: this.locationDetails.location,
      intents: JSON.parse(sessionStorage.getItem(this.global.topicsSessionKey))
    };
    this.saveTopicsFromGuidedAssistantToProfile(this.intentInput, false);
  }

  saveTopicsFromGuidedAssistantToProfile(intentInput, isIntent) {
    this.isIntent = isIntent;
    if (this.isIntent) {
      this.resourceTags = [];
    }
    this.getTopicDetails(intentInput).subscribe(response => {
      if (response && response.length > 0) {
        response.forEach(topic => {
          this.savedResources = {
            itemId: topic.id,
            resourceType: topic.resourceType,
            resourceDetails: {}
          };
          if (
            !this.arrayUtilityService.checkObjectExistInArray(
              this.resourceTags,
              this.savedResources
            )
          ) {
            this.resourceTags.push(this.savedResources);
          }
        });
        this.saveResourceToProfilePostLogin(this.resourceTags);
      } else {
        this.showWarning('Resource doesn\'t exists');
        this.router.navigateByUrl('/guidedassistant');
        this.spinner.hide();
      }
    });
  }

  saveResourceToProfilePostLogin(savedResources) {
    this.resourceIndex = 0;
    this.resourceTags = [];

    this.getUserSavedResources().subscribe(resources => {
      // todo for what this was need i don't know
      // if (!resources.length) {
      //   return;
      // }

      this.spinner.show();

      resources.forEach(resource => {
        this.resourceTags.push(resource);
      });

      this.checkExistingSavedResources(savedResources);
    });
  }

  checkExistingSavedResources(savedResources) {
    savedResources.forEach(savedResource => {
      if (
        this.arrayUtilityService.checkObjectExistInArray(
          this.resourceTags,
          savedResource
        )
      ) {
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
    this.profileResources = {
      oId: this.global.userId,
      resourceTags: resourceTags,
      type: 'resources'
    };
    this.saveResources(this.profileResources).subscribe(() => {
      this.spinner.hide();
      if (this.isIntent) {
        this.showSuccess(
          'Topics added to Profile. You can view them later once you\'ve completed the guided assistant.'
        );
      } else {
        this.showSuccess('Resource saved to profile');
      }
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
      if (
        !this.arrayUtilityService.checkObjectExistInArray(
          this.tempResourceStorage,
          savedResource
        )
      ) {
        this.tempResourceStorage.push(savedResource);
        this.showSuccess('Resource saved to session');
      } else {
        this.showWarning('Resource already saved to session');
      }
    }
    if (this.tempResourceStorage.length > 0) {
      sessionStorage.setItem(
        this.global.sessionKey,
        JSON.stringify(this.tempResourceStorage)
      );
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

  private getUserId(): string {
    if (this.global.isLoggedIn && !this.global.isShared) {
      return this.global.userId;
    } else if (this.global.isShared) {
      return this.global.sharedUserId;
    }
  }
}
