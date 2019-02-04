import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { FormsModule } from "@angular/forms";
import { NgxSpinnerModule } from 'ngx-spinner';
import {
  AccordionModule,
  BsDropdownModule,
  CarouselModule,
  CollapseModule,
  ModalModule, ProgressbarConfig,
  ProgressbarModule,
  TabsModule
} from 'ngx-bootstrap';

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
import { PipeModule } from './shared/pipe/pipe.module';
import { SharedModule } from "./shared/shared.module";
import { SubtopicDetailComponent } from "./topics-resources/subtopic/subtopic-detail.component";
import { SubtopicsComponent } from "./topics-resources/subtopic/subtopics.component";
import { TopicsComponent } from "./topics-resources/topic/topics.component";
import { TopicsResourcesComponent } from "./topics-resources/topics-resources.component";
import { CoreModule } from './core/core.module';

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
    CommonModule,
    FormsModule,
    AccordionModule.forRoot(),
    BsDropdownModule.forRoot(),
    CarouselModule.forRoot(),
    CollapseModule.forRoot(),
    ModalModule.forRoot(),
    ProgressbarModule.forRoot(),
    TabsModule.forRoot(),
    NgxSpinnerModule,
    CoreModule,
    SharedModule,
    PipeModule,
    AdminModule,
    AppRoutingModule,
  ],
  providers: [
    ProgressbarConfig,
    // fixme: ???!!!
    ProfileComponent,
    PersonalizedPlanComponent,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
