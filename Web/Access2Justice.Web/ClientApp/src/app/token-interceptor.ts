import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/operator/mergeMap';
import { Observable } from 'rxjs/Observable';
import { environment } from '../environments/environment';
import { Global } from './global';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth: MsalService, private global: Global) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (!this.global.userId) {
      return next.handle(req);
    }
    var scopes = environment.consentScopes;
    var tokenStored = this.auth.getCachedTokenInternal(scopes);
    if (tokenStored && tokenStored.token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${tokenStored.token}`
        }
      });
      return next.handle(req).do(
        event => {},
        err => {
          if (err instanceof HttpErrorResponse && err.status == 401) {
            var scopes = this.auth.getScopesForEndpoint(req.url);
            var tokenStored = this.auth.getCachedTokenInternal(scopes);
            if (tokenStored && tokenStored.token) {
              this.auth.clearCacheForScope(tokenStored.token);
            }
            console.log(JSON.stringify(err), "", JSON.stringify(scopes));
          }
        }
      );
    } else {
      return Observable.fromPromise(
        this.auth.acquireTokenSilent(scopes).then(token => {
          const JWT = `Bearer ${token}`;
          return req.clone({
            setHeaders: {
              Authorization: JWT
            }
          });
        })
      ).mergeMap(req =>
        next.handle(req).do(
          event => {},
          err => {
            if (err instanceof HttpErrorResponse && err.status == 401) {
              var scopes = this.auth.getScopesForEndpoint(req.url);
              var tokenStored = this.auth.getCachedTokenInternal(scopes);
              if (tokenStored && tokenStored.token) {
                this.auth.clearCacheForScope(tokenStored.token);
              }
              console.log(JSON.stringify(err), "", JSON.stringify(scopes));
            }
          }
        )
      );
    }
  }
}
