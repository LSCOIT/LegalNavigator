import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AccordionModule, BsDropdownModule,  CarouselModule, CollapseModule, ModalModule, ProgressbarModule } from 'ngx-bootstrap';

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
import { ProblemsComponent } from './guided-assistant/problems.component';

const appRoutes: Routes = [
  { path: 'about', component: AboutComponent },
  { path: 'createaccount', component: CreateAccountComponent },
  { path: 'guidedassistant', component: GuidedAssistantComponent },
  { path: 'guidedassistant/123', component: ProblemsComponent},
  { path: 'help', component: HelpFaqsComponent },
  { path: 'login', component: LogInComponent },
  { path: 'privacy', component: PrivacyPromiseComponent },
  { path: 'topics', component: TopicsResourcesComponent },
  { path: '', component: HomeComponent}
]

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
    TopicsComponent,
    TopicsResourcesComponent,
    UpperNavComponent,
    ProblemsComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes),
    AccordionModule.forRoot(),
    BsDropdownModule.forRoot(),
    CarouselModule.forRoot(),
    CollapseModule.forRoot(),
    ModalModule.forRoot(),
    ProgressbarModule.forRoot()
  ],
  providers: [TopicService],
  bootstrap: [AppComponent]
})

export class AppModule { }
