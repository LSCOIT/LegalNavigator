import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';
import { Question } from './question';

@Injectable()
export class QuestionService {

  constructor(private http: HttpClient) { }

  getQuestion(): Observable<Question> {
    return this.http.get<Question>(api.questionUrl);
  }

  //getNextQuestion(params): Observable<Question> {
  //  return this.http.post<Question>(api.questionUrl, params);
  //}

  //getPrevQuestion(params): Observable<Question> {
  //  return this.http.post<Question>(api.questionUrl, params);
  //}
}
