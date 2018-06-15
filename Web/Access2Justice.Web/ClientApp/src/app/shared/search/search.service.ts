import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class SearchService {

  siteHostName = "http://localhost:12176/";
  searchUrl = this.siteHostName + "api/search";
  searchOffsetUrl = this.siteHostName + "api/websearch/";
  getResourceUrl = this.siteHostName + "api/resources";
  

  constructor(private httpClient: HttpClient) { }

  search(luisInput: ILuisInput) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json'})
    };
    return this.httpClient.put(this.searchUrl, luisInput, httpOptions);
  }

  getPagedResources(resourceInput: IResourceFilter) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    //let httpHeaders = new Headers();
    //httpHeaders.append('resourceInput', resourceFilter);

    return this.httpClient.put(this.getResourceUrl, resourceInput, httpOptions);
    //return this.httpClient.get(this.getResourceUrl, resourceFilter, httpOptions);
  }

  searchByOffset(searchText: string, offset: number) {
    return this.httpClient.get(this.searchOffsetUrl + searchText + "/" + offset);
  }

}
