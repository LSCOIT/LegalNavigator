import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { api } from '../../../api/api';
import { ILuisInput, IResourceFilter } from '../search/search-results/search-results.model';

@Injectable()
export class SearchService {
  
  constructor(private httpClient: HttpClient) { }

  search(luisInput: ILuisInput) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json'})
    };
    return this.httpClient.post(api.searchUrl, luisInput, httpOptions);
  }

  getAnswers(question: string, isLuisCallRequired: boolean)
  {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.httpClient.get(api.getAnswerUrl + question + "/" + isLuisCallRequired, httpOptions);
  }
}
