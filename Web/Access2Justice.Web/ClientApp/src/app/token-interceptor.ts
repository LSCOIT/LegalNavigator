import { from, Observable } from 'rxjs';
import { mergeMap, tap } from 'rxjs/operators';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';

import ENV from 'env';
import { Global } from './global';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: MsalService, private global: Global) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.global.userId) {
      return next.handle(req);
    }

    const scopes = ENV().consentScopes;
    const tokenStored = this.auth.getCachedTokenInternal(scopes);
    if (tokenStored && tokenStored.token) {
      req = req.clone({setHeaders: this.getAuthHeaders(tokenStored.token)});

      return next.handle(req).pipe(tap(event => null, error => this.handleError(error, req)));
    } else {
      return from(
        this.auth.acquireTokenSilent(scopes).then(token => {
          return req.clone({
            setHeaders: this.getAuthHeaders(token)
          });
        })
      ).pipe(mergeMap(request => next.handle(request).pipe(
        tap(event => null, error => this.handleError(error, request))
      )));
    }
  }

  private getAuthHeaders(token: string): { [name: string]: string } {
    return {Authorization: `Bearer ${token}`};
  }

  private handleError(error: any, request: HttpRequest<any>) {
    if (error instanceof HttpErrorResponse && error.status === 401) {
      const scopes = this.auth.getScopesForEndpoint(request.url);
      const storedToken = this.auth.getCachedTokenInternal(scopes);

      if (storedToken && storedToken.token) {
        this.auth.clearCacheForScope(storedToken.token);
      }

      console.log(JSON.stringify(error), '', JSON.stringify(scopes));
    }
  }
}
