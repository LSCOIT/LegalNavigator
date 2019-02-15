import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutComponent } from './about/about.component';
import { ProfileResolver } from './app-resolver/profile-resolver.service';
import { CuratedExperienceResultComponent } from './guided-assistant/curated-experience-result/curated-experience-result.component';
import { CuratedExperienceComponent } from './guided-assistant/curated-experience/curated-experience.component';
import { GuidedAssistantComponent } from './guided-assistant/guided-assistant.component';
import { PersonalizedPlanComponent } from './guided-assistant/personalized-plan/personalized-plan.component';
import { HelpFaqsComponent } from './help-faqs/help-faqs.component';
import { HomeComponent } from './home/home.component';
import { PrivacyPromiseComponent } from './privacy-promise/privacy-promise.component';
import { ProfileComponent } from './profile/profile.component';
import { InternalErrorComponent } from './shared/error/internal-error/internal-error.component';
import { NotFoundComponent } from './shared/error/not-found/not-found.component';
import { UnauthorizedComponent } from './shared/error/unauthorized/unauthorized.component';
import { ResourceCardDetailComponent } from './shared/resource/resource-card-detail/resource-card-detail.component';
import { ShareButtonRouteComponent } from './shared/resource/user-action/share-button/share-button-route/share-button-route.component';
import { SearchResultsComponent } from './shared/search/search-results/search-results.component';
import { SubtopicDetailComponent } from './topics-resources/subtopic/subtopic-detail.component';
import { SubtopicsComponent } from './topics-resources/subtopic/subtopics.component';
import { TopicsResourcesComponent } from './topics-resources/topics-resources.component';

const appRoutes: Routes = [
  { path: 'search', component: SearchResultsComponent },
  { path: 'searchRefresh', component: SearchResultsComponent },
  { path: 'guidedassistant/search', component: CuratedExperienceResultComponent },
  { path: 'guidedassistant/:id', component: CuratedExperienceComponent },
  { path: 'guidedassistant', component: GuidedAssistantComponent },
  { path: 'resource/:id', component: ResourceCardDetailComponent },
  { path: 'subtopics/:topic', component: SubtopicDetailComponent },
  { path: 'topics/:topic', component: SubtopicsComponent},
  { path: 'topics', component: TopicsResourcesComponent },
  { path: 'about', component: AboutComponent },
  { path: 'plan/:id', component: PersonalizedPlanComponent },
  { path: 'share/:id', component: ShareButtonRouteComponent },
  { path: 'help', component: HelpFaqsComponent },
  { path: 'home', component: HomeComponent },
  { path: 'privacy', component: PrivacyPromiseComponent },
  { path: 'profile', component: ProfileComponent, resolve: { cres: ProfileResolver } },
  { path: '', component: HomeComponent },
  { path: 'error', component: InternalErrorComponent },
  { path: '401', component: UnauthorizedComponent },
  { path: '404', component: NotFoundComponent },
  // { path: '**', redirectTo: '/404' }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(appRoutes)
  ],
  declarations: [],
  exports: [
    RouterModule
  ],
  providers: [ProfileResolver]
})
export class AppRoutingModule { }
