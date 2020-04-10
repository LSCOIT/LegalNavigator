import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { api } from "../../../api/api";
import { IResourceFilter } from "../search/search-results/search-results.model";
import { Observable } from "rxjs";

@Injectable()
export class PaginationService {
  constructor(private httpClient: HttpClient) {}

  getPagedResources(resourceInput: IResourceFilter): Observable<any[]> {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" }),
    };
    var result = this.httpClient.post<any[]>(
      api.getResourceUrl,
      resourceInput,
      httpOptions
    );
    return result;
  }

  searchByOffset(searchText: string, offset: number) {
    return this.httpClient.get(
      api.searchOffsetUrl + "/" + searchText + "/" + offset
    );
  }
}
