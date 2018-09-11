import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { MsalService } from '@azure/msal-angular';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(public msalService: MsalService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    request = request.clone({
      setHeaders: {
        //Authorization: `Bearer ${this.msalService.getCachedTokenInternal(['user.read'])}`
        Authorization: `Bearer ${sessionStorage.getItem('msal.idtoken')}`
      }
    });
    return next.handle(request);
  }

}
