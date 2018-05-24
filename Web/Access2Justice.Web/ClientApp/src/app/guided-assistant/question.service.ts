import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Question } from '../guided-assistant/question';

@Injectable()
export class QuestionService {

  constructor(private http: HttpClient) { }

  private questionUrl =
    'http://access2justiceapi.azurewebsites.net/api/curatedexperience?surveyId=0b7dfe9b-cec9-4490-b768-c40916d52382';

  getQuestion(): Observable<Question> {
    return this.http.get<Question>(this.questionUrl);
  }

  getNextQuestion(params): Observable<Question> {
    return this.http.post<Question>(this.questionUrl, params);
  }

  getPrevQuestion(params): Observable<Question> {
    return this.http.post<Question>(this.questionUrl, params);
  }
}
