import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../api/api';
import { Question } from './question';
import { Answer } from './answers';

@Injectable()
export class QuestionService {

  constructor(private http: HttpClient) { }

  getQuestion(params): Observable<Question> {
    return this.http.get<Question>(api.questionUrl + '?' + params);
    //return this.http.get<Question>('./assets/SampleJsons/mockRadioQuestion.json');
  }

  getNextQuestion(params: Answer): Observable<Question> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.post<Question>(api.saveAndGetNextUrl, params, httpOptions);
  }
}
