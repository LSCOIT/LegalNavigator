import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { PersonalizedPlanCondition } from './personalized-plan';

@Injectable()
export class PersonalizedPlanService {

  siteHostName = "http://localhost:57203/";
  actionPlanUrl = this.siteHostName + "api/topics/getactionplanresourcedetails";

  constructor(private http: HttpClient) { }

  getActionPlanConditions(id): Observable<any> {
    return this.http.get<PersonalizedPlanCondition>(this.actionPlanUrl + '/' + id);
  }

  saveResourcesToSession(resources) {
    sessionStorage.setItem("bookmarkedResource", JSON.stringify(resources));
  }

}
