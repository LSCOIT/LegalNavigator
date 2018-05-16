import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AboutComponent } from './about/about.component';
import { CreateAccountComponent } from './account/create-account.component';
import { GuidedAssistantComponent } from './guided-assistant/guided-assistant.component';
import { HelpFaqsComponent } from './help-faqs/help-faqs.component';
import { HomeComponent } from './home/home.component';
import { LogInComponent } from './account/log-in.component';
import { PrivacyPromiseComponent } from './privacy-promise/privacy-promise.component';
import { QuestionComponent } from './guided-assistant/question.component';
import { SearchResultsComponent } from './shared/search/search-results.component';
import { SubtopicDetailComponent } from './topics-resources/subtopic/subtopic-detail.component';
import { SubtopicsComponent } from './topics-resources/subtopic/subtopics.component';
import { TopicsResourcesComponent } from './topics-resources/topics-resources.component';
import { ResourceCardDetailComponent } from
'./shared/resource/resource-card-detail/resource-card-detail.component';


const appRoutes: Routes = [
  { path: 'search', component: SearchResultsComponent},
  { path: 'guidedassistant/123', component: QuestionComponent },
  { path: 'guidedassistant', component: GuidedAssistantComponent },
  { path: 'resource/:id', component: ResourceCardDetailComponent },
  { path: 'topics/:topic/:subtopic', component: SubtopicDetailComponent },
  { path: 'topics/:topic', component: SubtopicsComponent },
  { path: 'topics', component: TopicsResourcesComponent },
  { path: 'about', component: AboutComponent },
  { path: 'createaccount', component: CreateAccountComponent },
  { path: 'help', component: HelpFaqsComponent },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LogInComponent },
  { path: 'privacy', component: PrivacyPromiseComponent },
  { path: '', component: HomeComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(appRoutes)
  ],
  declarations: [],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
