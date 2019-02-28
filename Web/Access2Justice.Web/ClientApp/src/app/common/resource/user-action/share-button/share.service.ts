import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { api } from "../../../../../api/api";
import { Share, ShareView } from "./share.model";

@Injectable()
export class ShareService {
  constructor(private http: HttpClient) {}

  getResourceLink(params): Observable<any> {
    return this.http.get<ShareView>(api.getResourceLink + "?" + params);
  }

  generateLink(shareInput: Share) {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    };
    return this.http.post<ShareView>(api.shareUrl, shareInput, httpOptions);
  }

  checkLink(shareInput: Share) {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    };
    return this.http.post<ShareView>(
      api.checkPermaLink,
      shareInput,
      httpOptions
    );
  }

  removeLink(unShareInput: Share) {
    const httpOptions = {
      headers: new HttpHeaders({ "Content-Type": "application/json" })
    };
    return this.http.post(api.unShareUrl, unShareInput, httpOptions);
  }
}
