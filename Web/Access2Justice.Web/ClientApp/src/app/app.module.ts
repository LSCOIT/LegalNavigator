import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';

import {
  AccordionModule,
  BsDropdownModule,
  CarouselModule,
  CollapseModule,
  ModalModule,
  ProgressbarModule,
  ProgressbarConfig,
  TabsModule
} from 'ngx-bootstrap';
import { MsalModule } from '@azure/msal-angular';
import { NgxSpinnerModule } from 'ngx-spinner';

import { AppComponent } from './app.component';
import { AboutComponent } from './about/about.component';
import { GuidedAssistantComponent } from './guided-assistant/guided-assistant.component';
import { HelpFaqsComponent } from './help-faqs/help-faqs.component';
import { HomeComponent } from './home/home.component';
import { PrivacyPromiseComponent } from './privacy-promise/privacy-promise.component';
import { QuestionComponent } from './guided-assistant/question/question.component';
import { QuestionService } from './guided-assistant/question/question.service';
import { TopicService } from './topics-resources/shared/topic.service';
import { SubtopicsComponent } from './topics-resources/subtopic/subtopics.component';
import { SubtopicDetailComponent } from './topics-resources/subtopic/subtopic-detail.component';
import { TopicsResourcesComponent } from './topics-resources/topics-resources.component';
import { TopicsComponent } from './topics-resources/topic/topics.component';
import { PersonalizedPlanComponent } from './guided-assistant/personalized-plan/personalized-plan.component';
import { ProfileComponent } from './profile/profile.component';
import { BreadcrumbService } from './topics-resources/shared/breadcrumb.service';
import { PersonalizedPlanService } from './guided-assistant/personalized-plan/personalized-plan.service';
import { DidYouKnowComponent } from './guided-assistant/did-you-know/did-you-know.component';
import { ArticlesResourcesComponent } from './guided-assistant/articles-resources/articles-resources.component';
import { CuratedExperienceComponent } from './guided-assistant/curated-experience/curated-experience.component';
import { StaticResourceService } from './shared/static-resource.service';
import { ResponseInterceptor } from './response-interceptor';
import { Global } from './global';
import { CuratedExperienceResultComponent } from './guided-assistant/curated-experience-result/curated-experience-result.component';
import { ProfileResolver } from './app-resolver/profile-resolver.service';
import { MsalInterceptor } from '@azure/msal-angular';
import { environment } from '../environments/environment';
import { api } from '../api/api';
import { AdminComponent } from './admin/admin.component';
import { UploadCuratedExperienceTemplateComponent } from './admin/upload-curated-experience-template/upload-curated-experience-template.component';

export const protectedResourceMap: [string, string[]][] = [[api.checkPermaLink, [environment.apiScope]], [api.shareUrl, [environment.apiScope]]
    , [api.unShareUrl, [environment.apiScope]], [api.userPlanUrl, [environment.apiScope]]]
//[api.uploadCuratedExperienceTemplateUrl, [environment.apiScope]]

@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    GuidedAssistantComponent,
    HelpFaqsComponent,
    HomeComponent,
    PrivacyPromiseComponent,
    QuestionComponent,
    TopicsComponent,
    TopicsResourcesComponent,
    SubtopicDetailComponent,
    SubtopicsComponent,
    PersonalizedPlanComponent,
    ProfileComponent,
    DidYouKnowComponent,
    ArticlesResourcesComponent,
    CuratedExperienceComponent,
    CuratedExperienceResultComponent,
    AdminComponent,
    UploadCuratedExperienceTemplateComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    SharedModule,
    AccordionModule.forRoot(),
    BsDropdownModule.forRoot(),
    CarouselModule.forRoot(),
    CollapseModule.forRoot(),
    ModalModule.forRoot(),
    ProgressbarModule.forRoot(),
    TabsModule.forRoot(),
    NgxSpinnerModule,
    MsalModule.forRoot({
      clientID: environment.clientID,
      authority: environment.authority,
      consentScopes: environment.consentScopes,
      redirectUri: environment.redirectUri,
      navigateToLoginRequestUrl: environment.navigateToLoginRequestUrl,
      postLogoutRedirectUri: environment.postLogoutRedirectUri,
      protectedResourceMap: protectedResourceMap
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ResponseInterceptor,
      multi: true
    }, {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    TopicService,
    QuestionService,
    ProgressbarConfig,
    BreadcrumbService,
    PersonalizedPlanService,
    ProfileComponent,
    PersonalizedPlanComponent,
    StaticResourceService,
    Global,
    ProfileResolver
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
