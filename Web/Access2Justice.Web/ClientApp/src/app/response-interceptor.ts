import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { tap } from "rxjs/operators";
import { catchError } from "rxjs/operators/catchError";
import 'rxjs/add/observable/throw';
import { Router } from '@angular/router';
import { ErrorHandler } from "@angular/router/src/router";

@Injectable()
export class ResponseInterceptor implements HttpInterceptor {
  constructor(private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).pipe(
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          // do stuff with response if you want
        }
      }),
      catchError((response: HttpErrorResponse) => {
          this.router.navigate(["/404"]);
          return Observable.throw(response);
        }
      )
    );
  }
}
