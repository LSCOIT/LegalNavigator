import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

import { AboutComponent } from './about/about.component';
import { CreateAccountComponent } from './account/create-account.component';
import { GuidedAssistantComponent } from './guided-assistant/guided-assistant.component';
import { HelpFaqsComponent } from './help-faqs/help-faqs.component';
import { HomeComponent } from './home/home.component';
import { LogInComponent } from './account/log-in.component';
import { ProfileComponent } from './profile/profile.component';
import { PrivacyPromiseComponent } from './privacy-promise/privacy-promise.component';
import { CuratedExperienceComponent } from './guided-assistant/curated-experience/curated-experience.component';
import { SearchResultsComponent } from './shared/search/search-results/search-results.component';
import { SubtopicDetailComponent } from './topics-resources/subtopic/subtopic-detail.component';
import { SubtopicsComponent } from './topics-resources/subtopic/subtopics.component';
import { TopicsResourcesComponent } from './topics-resources/topics-resources.component';
import { ResourceCardDetailComponent } from
'./shared/resource/resource-card-detail/resource-card-detail.component';
import { PersonalizedPlanComponent } from './guided-assistant/personalized-plan/personalized-plan.component';
import { NotFoundComponent } from './shared/error/not-found/not-found.component';
import { InternalErrorComponent } from './shared/error/internal-error/internal-error.component';
import { ShareButtonRouteComponent } from './shared/resource/user-action/share-button/share-button-route/share-button-route.component';
import { CuratedExperienceResultComponent } from './guided-assistant/curated-experience-result/curated-experience-result.component';

const appRoutes: Routes = [
  { path: 'search', component: SearchResultsComponent },
  { path: 'searchRefresh', component: SearchResultsComponent },
  { path: 'guidedassistant/:id', component: CuratedExperienceComponent },
  { path: 'guidedassistant', component: GuidedAssistantComponent },
  { path: 'guidedassistantSearch', component: CuratedExperienceResultComponent },
  { path: 'resource/:id', component: ResourceCardDetailComponent },
  { path: 'subtopics/:topic', component: SubtopicDetailComponent },
  { path: 'topics/:topic', component: SubtopicsComponent},
  { path: 'topics', component: TopicsResourcesComponent },
  { path: 'about', component: AboutComponent },
  { path: 'createaccount', component: CreateAccountComponent },
  { path: 'plan/:id', component: PersonalizedPlanComponent },
  { path: 'share/:id', component: ShareButtonRouteComponent },
  { path: 'help', component: HelpFaqsComponent },
  { path: 'home', component: HomeComponent },
  //{ path: 'login', component: LogInComponent },
  { path: 'privacy', component: PrivacyPromiseComponent },
  { path: 'profile', component: ProfileComponent },
  { path: '', component: HomeComponent },
  { path: 'error', component: InternalErrorComponent},
  { path: '404', component: NotFoundComponent },
  { path: '**', redirectTo: '/404' } 

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
