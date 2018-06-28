import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { api } from '../../../api/api';
import { IResourceFilter } from '../search/search-results/search-results.model';

@Injectable()
export class PaginationService {

  constructor(private httpClient: HttpClient) { }

  getPagedResources(resourceInput: IResourceFilter) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.httpClient.post(api.getResourceUrl, resourceInput, httpOptions);
  }

  searchByOffset(searchText: string, offset: number) {
    return this.httpClient.get(api.searchOffsetUrl + "/" + searchText + "/" + offset);
  }
}
