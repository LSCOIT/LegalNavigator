import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MsalModule } from '@azure/msal-angular';
import {
  AccordionModule,
  BsDropdownModule,
  ButtonsModule,
  CarouselModule,
  CollapseModule,
  ModalModule,
  ProgressbarModule,
  TabsModule
} from 'ngx-bootstrap';
import { ToastrModule } from 'ngx-toastr';

import { ENV } from 'environment';
import { ResponseInterceptor } from '../response-interceptor';
import { TokenInterceptor } from '../token-interceptor';

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
    BrowserAnimationsModule,

    MsalModule.forRoot({
      clientID: ENV.clientID,
      authority: ENV.authority,
      consentScopes: ENV.consentScopes,
      redirectUri: ENV.redirectUri,
      navigateToLoginRequestUrl: ENV.navigateToLoginRequestUrl,
      postLogoutRedirectUri: ENV.postLogoutRedirectUri
    }),
    AccordionModule.forRoot(),
    BsDropdownModule.forRoot(),
    CarouselModule.forRoot(),
    CollapseModule.forRoot(),
    ModalModule.forRoot(),
    ProgressbarModule.forRoot(),
    TabsModule.forRoot(),
    ButtonsModule.forRoot(),
    ToastrModule.forRoot(),
  ]
})
export class CoreModule {
}
