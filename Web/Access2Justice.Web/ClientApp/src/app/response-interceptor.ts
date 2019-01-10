import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import "rxjs/add/observable/throw";
import { Observable } from "rxjs/Observable";
import { tap } from "rxjs/operators";
import { catchError } from "rxjs/operators/catchError";

@Injectable()
export class ResponseInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          // do stuff with response if you want
        }
      }),
      catchError((response: HttpErrorResponse) => {
        if (response.status === 404) {
          this.router.navigate(["/404"]);
        } else {
          return Observable.throw(response);
        }
      })
    );
  }
}
