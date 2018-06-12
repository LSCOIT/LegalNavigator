import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class SearchService {

  endPoint: string = "http://localhost:12176/api/search/";
  siteHostName = "http://localhost:12176/";
  searchUrl = this.siteHostName + "api/search/";
  searchOffsetUrl = this.siteHostName + "api/websearch/";
  getResourceUrl = this.siteHostName + "api/resources";
  

  constructor(private httpClient: HttpClient) { }

  search(searchText: string) {
    return this.httpClient.get(this.searchUrl + searchText);
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
