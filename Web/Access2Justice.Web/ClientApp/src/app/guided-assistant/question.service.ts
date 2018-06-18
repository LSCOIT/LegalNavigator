import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { guidedAssistantApi } from '../../api/api';
import { Question } from '../guided-assistant/question';

@Injectable()
export class QuestionService {

  constructor(private http: HttpClient) { }

  getQuestion(): Observable<Question> {
    return this.http.get<Question>(guidedAssistantApi.questionUrl);
  }

  getNextQuestion(params): Observable<Question> {
    return this.http.post<Question>(guidedAssistantApi.questionUrl, params);
  }

  getPrevQuestion(params): Observable<Question> {
    return this.http.post<Question>(guidedAssistantApi.questionUrl, params);
  }
}
