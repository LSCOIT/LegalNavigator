import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';
import { HttpModule } from '@angular/http';

import {
  AccordionModule,
  CarouselModule,
  CollapseModule,
  ModalModule,
  ProgressbarModule
} from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { AboutComponent } from './about/about.component';
import { CreateAccountComponent } from './account/create-account.component';
import { GuidedAssistantComponent } from './guided-assistant/guided-assistant.component';
import { HelpFaqsComponent } from './help-faqs/help-faqs.component';
import { HomeComponent } from './home/home.component';
import { LogInComponent } from './account/log-in.component';
import { PrivacyPromiseComponent } from './privacy-promise/privacy-promise.component';
import { QuestionComponent } from './guided-assistant/question.component';
import { QuestionService } from './guided-assistant/question.service';
import { TopicService } from './topics-resources/shared/topic.service';
import { SubtopicsComponent } from './topics-resources/subtopic/subtopics.component';
import { SubtopicDetailComponent } from './topics-resources/subtopic/subtopic-detail.component';
import { TopicsResourcesComponent } from './topics-resources/topics-resources.component';
import { TopicsComponent } from './topics-resources/topic/topics.component';


@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    CreateAccountComponent,
    GuidedAssistantComponent,
    HelpFaqsComponent,
    HomeComponent,
    LogInComponent,
    PrivacyPromiseComponent,
    QuestionComponent,
    TopicsComponent,
    TopicsResourcesComponent,
    SubtopicDetailComponent,
    SubtopicsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpModule,
    AppRoutingModule,
    SharedModule,
    AccordionModule.forRoot(),
    CarouselModule.forRoot(),
    CollapseModule.forRoot(),
    ModalModule.forRoot(),
    ProgressbarModule.forRoot()
  ],
  providers: [
    TopicService,
    QuestionService
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
