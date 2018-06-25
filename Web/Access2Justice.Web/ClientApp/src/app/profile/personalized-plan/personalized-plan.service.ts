import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { PersonalizedPlanCondition, Resources } from './personalized-plan';

@Injectable()
export class PersonalizedPlanService {

  siteHostName = "http://localhost:57203/";
  actionPlanUrl = this.siteHostName + "api/topics/getactionplanresourcedetails";
  resource: Resources;
  tempStorage: Array<Resources>;
  sessionKey: string = "bookmarkedResource";

  constructor(private http: HttpClient) { }

  getActionPlanConditions(id): Observable<any> {
    return this.http.get<PersonalizedPlanCondition>(this.actionPlanUrl + '/' + id);
  }

  saveResourcesToSession(resources) {
    this.tempStorage = JSON.parse(sessionStorage.getItem(this.sessionKey));
    if (this.tempStorage) {
      let i = 0;
      //for (let i = 0; i < this.tempStorage.length; i++) {
        if (this.tempStorage[i].itemId !== resources.itemId) {
          this.tempStorage[i] = JSON.parse(sessionStorage.getItem(this.sessionKey));
          //this.tempStorage[i] = (this.tempStorage[i] === null) ? [] : this.tempStorage[i];
          this.tempStorage.push(resources);

          sessionStorage.setItem(this.sessionKey, JSON.stringify(this.tempStorage[i]));
        }
      //}
    }
    else {
      sessionStorage.setItem(this.sessionKey, JSON.stringify(resources));
    }
  }

}
