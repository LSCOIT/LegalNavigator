import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Question } from '../guided-assistant/question';

@Injectable()
export class QuestionService {

  constructor(private http: HttpClient) {}

  private questionUrl =
    'http://access2justiceapi.azurewebsites.net/api/curatedexperience?surveyId=19a0c939-9635-40a5-a33c-c441fdb3f26f';

  getQuestion(): Observable<Question> {
    return this.http.get<Question>(this.questionUrl);
  }

  getNextQuestion(params): Observable<Question> {
    return this.http.post<Question>(this.questionUrl, params);
  }
}
