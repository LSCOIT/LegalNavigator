import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { api } from "../../../api/api";
import { Answer } from "./answers";
import { Question } from "./question";

@Injectable()
export class QuestionService {
  constructor(private http: HttpClient) {}

  getQuestion(params): Observable<Question> {
    return this.http.get<Question>(api.questionUrl + "?" + params);
  }

  getNextQuestion(params: Answer): Observable<Question> {
    const httpOptions = {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    };
    return this.http.post<Question>(api.saveAndGetNextUrl, params, httpOptions);
  }

  getpersonalizedPlan(params): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    };
    return this.http.get<any>(api.personalizedPlan + "?" + params, httpOptions);
  }
}
