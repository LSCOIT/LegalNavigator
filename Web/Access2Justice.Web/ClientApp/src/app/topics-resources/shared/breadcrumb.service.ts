import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { api } from "../../../api/api";

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {
  constructor(private http: HttpClient) {}

  getBreadcrumbs(breadcrumbId: string): Observable<any> {
    return this.http.get(api.breadcrumbsUrl + "/" + breadcrumbId);
  }
}
