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
    return this.httpClient.put(api.searchUrl, luisInput, httpOptions);
  }

  getPagedResources(resourceInput: IResourceFilter) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.httpClient.put(api.getResourceUrl, resourceInput, httpOptions);
  }

  searchByOffset(searchText: string, offset: number) {
    return this.httpClient.get(api.searchOffsetUrl + "/" + searchText + "/" + offset);
  }

}
