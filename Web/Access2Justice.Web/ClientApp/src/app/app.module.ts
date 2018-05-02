import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import {
  AccordionModule,
  BsDropdownModule,
  CarouselModule,
  CollapseModule,
  ModalModule,
  ProgressbarModule
} from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { AboutComponent } from './about/about.component';
import { ChatbotComponent } from './shared/chatbot.component';
import { CreateAccountComponent } from './account/create-account.component';
import { FooterComponent } from './shared/footer.component';
import { GuidedAssistantComponent } from './guided-assistant/guided-assistant.component';
import { HelpFaqsComponent } from './help-faqs/help-faqs.component';
import { HomeComponent } from './home/home.component';
import { LogInComponent } from './account/log-in.component';
import { LowerNavComponent } from './shared/lower-nav.component';
import { PrivacyPromiseComponent } from './privacy-promise/privacy-promise.component';
import { TopicService } from './topics-resources/topic.service';
import { TopicsResourcesComponent } from './topics-resources/topics-resources.component';
import { TopicsComponent } from './topics-resources/topics.component';
import { UpperNavComponent } from './shared/upper-nav.component';
import { QuestionComponent } from './guided-assistant/question.component';
import { QuestionService } from './guided-assistant/question.service';
import { TopicComponent } from './topics-resources/topic.component';

const appRoutes: Routes = [
  { path: 'about', component: AboutComponent },
  { path: 'createaccount', component: CreateAccountComponent },
  { path: 'guidedassistant', component: GuidedAssistantComponent },
  { path: 'guidedassistant/123', component: QuestionComponent},
  { path: 'help', component: HelpFaqsComponent },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LogInComponent },
  { path: 'privacy', component: PrivacyPromiseComponent },
  { path: 'topics', component: TopicsResourcesComponent },
  { path: 'topics/:title', component: TopicComponent },
  { path: '', component: HomeComponent}
];

@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    ChatbotComponent,
    CreateAccountComponent,
    GuidedAssistantComponent,
    FooterComponent,
    HelpFaqsComponent,
    HomeComponent,
    LogInComponent,
    LowerNavComponent,
    PrivacyPromiseComponent,
    QuestionComponent,
    TopicsComponent,
    TopicsResourcesComponent,
    UpperNavComponent,
    TopicComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    AccordionModule.forRoot(),
    BsDropdownModule.forRoot(),
    CarouselModule.forRoot(),
    CollapseModule.forRoot(),
    ModalModule.forRoot(),
    ProgressbarModule.forRoot()
  ],
  providers: [
    TopicService,
    QuestionService],
  bootstrap: [AppComponent]
})

export class AppModule { }
