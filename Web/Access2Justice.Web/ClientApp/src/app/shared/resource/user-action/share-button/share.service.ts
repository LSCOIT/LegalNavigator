import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { api } from '../../../../../api/api';
import { Share, UnShare, ShareView } from '../share-button/share.model';

@Injectable()
export class ShareService {

  constructor(private http: HttpClient) { }

  getResourceLink(params): Observable<any> {
    return this.http.get(api.getResourceLink + "?" + params);
  }

  generateLink(shareInput: Share) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post<ShareView>(api.shareUrl, shareInput, httpOptions);
  }

  checkLink(shareInput: Share) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post<ShareView>(api.checkPermaLink, shareInput, httpOptions);
  }

  removeLink(unShareInput: UnShare) {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post(api.unShareUrl, unShareInput, httpOptions);
  }

}
