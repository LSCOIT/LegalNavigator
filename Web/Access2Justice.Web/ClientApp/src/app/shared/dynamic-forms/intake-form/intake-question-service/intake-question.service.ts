import { Injectable } from "@angular/core";
import { IntakeFormQuestionBase } from "../intake-form-question-base";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { api } from "../../../../../api/api";

@Injectable()
export class IntakeQuestionService {
  constructor(private http: HttpClient) {}

  getIntakeQuestions(params): Observable<IntakeFormQuestionBase<any>> {
    return this.http.get<IntakeFormQuestionBase<any>>(
      api.onboardingUrl + "?" + params
    );
  }

  sendIntakeResponse(params): Observable<Object> {
    const httpOptions = {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    };
    return this.http.post<any>(
      api.onboardingSubmissionUrl,
      params,
      httpOptions
    );
  }
}
