import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { MsalModule } from "@azure/msal-angular";
import { AccordionModule, BsDropdownModule, CarouselModule, CollapseModule, ModalModule, ProgressbarConfig, ProgressbarModule, TabsModule } from "ngx-bootstrap";
import { NgxSpinnerModule } from "ngx-spinner";
import { environment } from "../environments/environment";
import { AboutComponent } from "./about/about.component";
import { AdminModule } from "./admin/admin.module";
import { ProfileResolver } from "./app-resolver/profile-resolver.service";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { Global } from "./global";
import { ArticlesResourcesComponent } from "./guided-assistant/articles-resources/articles-resources.component";
import { CuratedExperienceResultComponent } from "./guided-assistant/curated-experience-result/curated-experience-result.component";
import { CuratedExperienceComponent } from "./guided-assistant/curated-experience/curated-experience.component";
import { DidYouKnowComponent } from "./guided-assistant/did-you-know/did-you-know.component";
import { GuidedAssistantComponent } from "./guided-assistant/guided-assistant.component";
import { PersonalizedPlanComponent } from "./guided-assistant/personalized-plan/personalized-plan.component";
import { PersonalizedPlanService } from "./guided-assistant/personalized-plan/personalized-plan.service";
import { QuestionComponent } from "./guided-assistant/question/question.component";
import { QuestionService } from "./guided-assistant/question/question.service";
import { HelpFaqsComponent } from "./help-faqs/help-faqs.component";
import { HomeComponent } from "./home/home.component";
import { PrivacyPromiseComponent } from "./privacy-promise/privacy-promise.component";
import { ProfileComponent } from "./profile/profile.component";
import { ResponseInterceptor } from "./response-interceptor";
import { PipeModule } from "./shared/pipe/pipe.module";
import { StaticResourceService } from "./shared/services/static-resource.service";
import { SharedModule } from "./shared/shared.module";
import { TokenInterceptor } from "./token-interceptor";
import { BreadcrumbService } from "./topics-resources/shared/breadcrumb.service";
import { TopicService } from "./topics-resources/shared/topic.service";
import { SubtopicDetailComponent } from "./topics-resources/subtopic/subtopic-detail.component";
import { SubtopicsComponent } from "./topics-resources/subtopic/subtopics.component";
import { TopicsComponent } from "./topics-resources/topic/topics.component";
import { TopicsResourcesComponent } from "./topics-resources/topics-resources.component";

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
    CuratedExperienceResultComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AdminModule,
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
      postLogoutRedirectUri: environment.postLogoutRedirectUri
    }),
    PipeModule.forRoot()
  ],
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
export class AppModule {}
