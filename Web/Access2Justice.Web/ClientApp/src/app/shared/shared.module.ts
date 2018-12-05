import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '../app-routing.module';
import { FormsModule } from '@angular/forms';
import { AccordionModule, BsDropdownModule, ModalModule, CarouselModule } from 'ngx-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { ChatbotComponent } from './chatbot/chatbot.component';
import { DownloadButtonComponent } from './resource/user-action/download-button/download-button.component';
import { FooterComponent } from './footer/footer.component';
import { GuidedAssistantSidebarComponent } from './sidebars/guided-assistant-sidebar/guided-assistant-sidebar.component';
import { LanguageComponent } from './language/language.component';
import { LowerNavComponent } from './navigation/lower-nav.component';
import { NavigateDataService } from './navigate-data.service';
import { PrintButtonComponent } from './resource/user-action/print-button/print-button.component';
import { ResourceCardComponent } from './resource/resource-card/resource-card.component';
import { ResourceCardDetailComponent } from './resource/resource-card-detail/resource-card-detail.component';
import { RemoveButtonComponent } from './resource/user-action/remove-button/remove-button.component';
import { SaveButtonComponent } from './resource/user-action/save-button/save-button.component';
import { SearchComponent } from './search/search.component';
import { SearchFilterComponent } from './search/search-filter/search-filter.component';
import { SearchResultsComponent } from './search/search-results/search-results.component';
import { SearchService } from './search/search.service';
import { ServiceOrgSidebarComponent } from './sidebars/service-org-sidebar/service-org-sidebar.component';
import { ShareButtonComponent } from './resource/user-action/share-button/share-button.component';
import { UpperNavComponent } from './navigation/upper-nav.component';
import { WebResourceComponent } from './search/search-results/web-resource/web-resource.component';
import { SearchFilterPipe } from './search/search-filter.pipe';
import { BreadcrumbComponent } from '../topics-resources/breadcrumb/breadcrumb.component';
import { MapResultsComponent } from './sidebars/map-results/map-results.component';
import { MapResultsService } from './sidebars/map-results/map-results.service';
import { PaginationComponent } from './pagination/pagination.component';
import { PaginationService } from './pagination/pagination.service';
import { ActionPlansComponent } from './resource/resource-type/action-plan/action-plans.component';
import { UserActionSidebarComponent } from './sidebars/user-action-sidebar/user-action-sidebar.component';
import { SettingButtonComponent } from './resource/user-action/setting-button/setting-button.component';
import { ShowMoreService } from './sidebars/show-more/show-more.service';
import { ArrayUtilityService } from './array-utility.service';
import { HelplineComponent } from './helpline/helpline.component';
import { EventUtilityService } from './event-utility.service';
import { NotFoundComponent } from './error/not-found/not-found.component';
import { ArticlesComponent } from './resource/resource-type/articles/articles.component';
import { OrganizationsComponent } from './resource/resource-type/organizations/organizations.component';
import { VideosComponent } from './resource/resource-type/videos/videos.component';
import { ResourceService } from './resource/resource.service';
import { MapComponent } from './map/map.component';
import { MapService } from './map/map.service';
import { InternalErrorComponent } from './error/internal-error/internal-error.component';
import { LoginComponent } from './login/login.component';
import { ShareService } from './resource/user-action/share-button/share.service';
import { ShareButtonRouteComponent } from './resource/user-action/share-button/share-button-route/share-button-route.component';
import { ReadMoreComponent } from './read-more/read-more.component';
import { LoginService } from './login/login.service';
import { BrowserTabCloseComponent } from './browser-tab-close/browser-tab-close.component';
import { SaveButtonService } from './resource/user-action/save-button/save-button.service';
import { UnauthorizedComponent } from './error/unauthorized/unauthorized.component';
import { SanitizePipe } from './pipe/sanitize.pipe';

@NgModule({
  imports: [
    CommonModule,
    AppRoutingModule,
    FormsModule,
    AccordionModule.forRoot(),
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    CarouselModule.forRoot(),
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
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
    SearchFilterPipe,
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
    SanitizePipe
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
    UnauthorizedComponent,
    SanitizePipe
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
    SaveButtonService
  ]
})
export class SharedModule { }
