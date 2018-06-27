import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { PersonalizedPlanCondition, Resources } from './personalized-plan';
import { api } from '../../../api/api';

@Injectable()
export class PersonalizedPlanService {

  resource: Resources;
  tempStorage: Array<Resources> = [];
  resoureStorage: any = [];
  resourceTemp: any = [];
  sessionKey: string = "bookmarkedResource";
  isObjectExists: boolean = false;

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

}
