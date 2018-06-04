import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class SearchService {

  endPoint: string = "http://localhost:12176/api/search/";
  siteHostName = "http://localhost:12176/";
  searchUrl = this.siteHostName + "api/search/";
  searchOffsetUrl = this.siteHostName + "api/websearch/";

  constructor(private httpClient: HttpClient) { }

  search(searchText: string) {
    return this.httpClient.get(this.searchUrl + searchText);
  }

  searchByOffset(searchText: string, offset: number) {
    return this.httpClient.get(this.searchOffsetUrl + searchText + "/" + offset);
  }

}
