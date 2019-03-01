import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { BreadcrumbComponent } from '../topics-resources/breadcrumb/breadcrumb.component';
import { BrowserTabCloseComponent } from './browser-tab-close/browser-tab-close.component';
import { ChatbotComponent } from './chatbot/chatbot.component';
import { IntakeFormComponent } from './dynamic-forms/intake-form/intake-form.component';
import { IntakeQuestionService } from './dynamic-forms/intake-form/intake-question-service/intake-question.service';
import { QuestionControlService } from './dynamic-forms/question-control.service';
import { InternalErrorComponent } from './error/internal-error/internal-error.component';
import { NotFoundComponent } from './error/not-found/not-found.component';
import { UnauthorizedComponent } from './error/unauthorized/unauthorized.component';
import { FooterComponent } from './footer/footer.component';
import { HelplineComponent } from './helpline/helpline.component';
import { LanguageComponent } from './language/language.component';
import { LoginComponent } from './login/login.component';
import { LoginService } from './login/login.service';
import { MapComponent } from './map/map.component';
import { MapService } from './map/map.service';
import { LowerNavComponent } from './navigation/lower-nav.component';
import { UpperNavComponent } from './navigation/upper-nav.component';
import { PaginationComponent } from './pagination/pagination.component';
import { PaginationService } from './pagination/pagination.service';
import { ReadMoreComponent } from './read-more/read-more.component';
import { ResourceCardDetailComponent } from './resource/resource-card-detail/resource-card-detail.component';
import { ResourceCardComponent } from './resource/resource-card/resource-card.component';
import { ActionPlansComponent } from './resource/resource-type/action-plan/action-plans.component';
import { ArticlesComponent } from './resource/resource-type/articles/articles.component';
import { OrganizationsComponent } from './resource/resource-type/organizations/organizations.component';
import { VideosComponent } from './resource/resource-type/videos/videos.component';
import { ResourceService } from './resource/resource.service';
import { DownloadButtonComponent } from './resource/user-action/download-button/download-button.component';
import { PrintButtonComponent } from './resource/user-action/print-button/print-button.component';
import { RemoveButtonComponent } from './resource/user-action/remove-button/remove-button.component';
import { SaveButtonComponent } from './resource/user-action/save-button/save-button.component';
import { SaveButtonService } from './resource/user-action/save-button/save-button.service';
import { SettingButtonComponent } from './resource/user-action/setting-button/setting-button.component';
import { ShareButtonRouteComponent } from './resource/user-action/share-button/share-button-route/share-button-route.component';
import { ShareButtonComponent } from './resource/user-action/share-button/share-button.component';
import { ShareService } from './resource/user-action/share-button/share.service';
import { SearchFilterComponent } from './search/search-filter/search-filter.component';
import { SearchResultsComponent } from './search/search-results/search-results.component';
import { WebResourceComponent } from './search/search-results/web-resource/web-resource.component';
import { SearchComponent } from './search/search.component';
import { SearchService } from './search/search.service';
import { ArrayUtilityService } from './services/array-utility.service';
import { EventUtilityService } from './services/event-utility.service';
import { NavigateDataService } from './services/navigate-data.service';
import { StateCodeService } from './services/state-code.service';
import { GuidedAssistantSidebarComponent } from './sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { MapResultsComponent } from './sidebars/map-results/map-results.component';
import { MapResultsService } from './sidebars/map-results/map-results.service';
import { ServiceOrgSidebarComponent } from './sidebars/service-org-sidebar/service-org-sidebar.component';
import { ShowMoreService } from './sidebars/show-more/show-more.service';
import { UserActionSidebarComponent } from './sidebars/user-action-sidebar/user-action-sidebar.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [RouterModule, SharedModule],
  declarations: [
    ActionPlansComponent,
    ChatbotComponent,
    DownloadButtonComponent,
    FooterComponent,
    GuidedAssistantSidebarComponent,
    LanguageComponent,
    LowerNavComponent,
    MapComponent,
    PrintButtonComponent,
    ResourceCardComponent,
    ResourceCardDetailComponent,
    RemoveButtonComponent,
    SearchComponent,
    SearchFilterComponent,
    SearchResultsComponent,
    ServiceOrgSidebarComponent,
    SaveButtonComponent,
    ShareButtonComponent,
    UpperNavComponent,
    WebResourceComponent,
    BreadcrumbComponent,
    MapResultsComponent,
    PaginationComponent,
    UserActionSidebarComponent,
    SettingButtonComponent,
    HelplineComponent,
    NotFoundComponent,
    ArticlesComponent,
    OrganizationsComponent,
    VideosComponent,
    InternalErrorComponent,
    LoginComponent,
    ShareButtonRouteComponent,
    ReadMoreComponent,
    BrowserTabCloseComponent,
    UnauthorizedComponent,
    IntakeFormComponent
  ],
  exports: [
    ActionPlansComponent,
    ChatbotComponent,
    DownloadButtonComponent,
    FooterComponent,
    GuidedAssistantSidebarComponent,
    LanguageComponent,
    LowerNavComponent,
    MapComponent,
    PrintButtonComponent,
    ResourceCardComponent,
    ResourceCardDetailComponent,
    RemoveButtonComponent,
    SearchComponent,
    SearchFilterComponent,
    SearchResultsComponent,
    ServiceOrgSidebarComponent,
    SaveButtonComponent,
    ShareButtonComponent,
    UpperNavComponent,
    BreadcrumbComponent,
    UserActionSidebarComponent,
    WebResourceComponent,
    HelplineComponent,
    LoginComponent,
    BrowserTabCloseComponent,
    UnauthorizedComponent
  ],
  providers: [
    MapService,
    NavigateDataService,
    SearchService,
    MapResultsService,
    PaginationService,
    ShowMoreService,
    ArrayUtilityService,
    EventUtilityService,
    ResourceService,
    ShareService,
    LoginService,
    SaveButtonService,
    StateCodeService,
    IntakeQuestionService,
    QuestionControlService
  ]
})
export class AppCommonModule {
}
