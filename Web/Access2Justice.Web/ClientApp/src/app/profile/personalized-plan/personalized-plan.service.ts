import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { PersonalizedPlanCondition, Resources } from './personalized-plan';
import { api } from '../../../api/api';

@Injectable()
export class PersonalizedPlanService {

  resource: Resources;
  tempStorage: Array<Resources>;
  sessionKey: string = "bookmarkedResource";

  constructor(private http: HttpClient) { }

  getActionPlanConditions(id): Observable<any> {
    return this.http.get<PersonalizedPlanCondition>(api.planUrl + '/' + id);
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
