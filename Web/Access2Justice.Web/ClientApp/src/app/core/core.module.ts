import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { MsalModule } from '@azure/msal-angular';
import { BrowserModule } from '@angular/platform-browser';

import { ResponseInterceptor } from '../response-interceptor';
import { TokenInterceptor } from '../token-interceptor';
import ENV from 'env';

@NgModule({
  declarations: [],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ResponseInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    MsalModule.forRoot({
      clientID: ENV().clientID,
      authority: ENV().authority,
      consentScopes: ENV().consentScopes,
      redirectUri: ENV().redirectUri,
      navigateToLoginRequestUrl: ENV().navigateToLoginRequestUrl,
      postLogoutRedirectUri: ENV().postLogoutRedirectUri
    }),
  ]
})
export class CoreModule {
}
