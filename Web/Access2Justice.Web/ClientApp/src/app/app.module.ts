import { NgModule } from "@angular/core";
import { ProgressbarConfig, PaginationModule } from "ngx-bootstrap";

import { HttpClientModule, HttpClientJsonpModule } from "@angular/common/http";
import { AboutComponent } from "./about/about.component";
import { AdminModule } from "./admin/admin.module";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { ArticlesResourcesComponent } from "./guided-assistant/articles-resources/articles-resources.component";
import { CuratedExperienceResultComponent } from "./guided-assistant/curated-experience-result/curated-experience-result.component";
import { CuratedExperienceComponent } from "./guided-assistant/curated-experience/curated-experience.component";
import { DidYouKnowComponent } from "./guided-assistant/did-you-know/did-you-know.component";
import { GuidedAssistantComponent } from "./guided-assistant/guided-assistant.component";
import { PersonalizedPlanComponent } from "./guided-assistant/personalized-plan/personalized-plan.component";
import { QuestionComponent } from "./guided-assistant/question/question.component";
import { HelpFaqsComponent } from "./help-faqs/help-faqs.component";
import { HomeComponent } from "./home/home.component";
import { PrivacyPromiseComponent } from "./privacy-promise/privacy-promise.component";
import { ProfileComponent } from "./profile/profile.component";
import { AppCommonModule } from "./common/app-common.module";
import { SubtopicDetailComponent } from "./topics-resources/subtopic/subtopic-detail.component";
import { SubtopicsComponent } from "./topics-resources/subtopic/subtopics.component";
import { TopicsComponent } from "./topics-resources/topic/topics.component";
import { TopicsResourcesComponent } from "./topics-resources/topics-resources.component";
import { CoreModule } from "./core/core.module";
import { SharedModule } from "./shared/shared.module";
import { SavedResourcesComponent } from "./profile/saved-resources/saved-resources.component";
import { IncomingResourcesComponent } from "./profile/incoming-resources/incoming-resources.component";
import { SharedResourcesComponent } from "./profile/shared-resources/shared-resources.component";
import { InnerQuestionComponent } from './guided-assistant/question/inner-question/inner-question.component';

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
    SavedResourcesComponent,
    DidYouKnowComponent,
    ArticlesResourcesComponent,
    CuratedExperienceComponent,
    CuratedExperienceResultComponent,
    IncomingResourcesComponent,
    SharedResourcesComponent,
    InnerQuestionComponent,
  ],
  imports: [
    CoreModule,
    SharedModule,
    AppCommonModule,
    AdminModule,
    AppRoutingModule,
    HttpClientModule,
    HttpClientJsonpModule,
    PaginationModule.forRoot(),
  ],
  providers: [
    ProgressbarConfig,
    // fixme: ???!!!
    ProfileComponent,
    PersonalizedPlanComponent,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
