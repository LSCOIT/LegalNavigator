import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '../app-routing.module';
import { FormsModule } from '@angular/forms';
import { AccordionModule, BsDropdownModule, ModalModule, CarouselModule } from 'ngx-bootstrap';

import { ChatbotComponent } from './chatbot/chatbot.component';
import { CuratedResourceComponent } from './search/search-results/curated-resource/curated-resource.component';
import { DownloadButtonComponent } from './resource/user-action/download-button.component';
import { FooterComponent } from './footer/footer.component';
import { GuidedAssistantSidebarComponent } from './sidebars/guided-assistant-sidebar.component';
import { LanguageComponent } from './language/language.component';
import { LocationComponent } from './location/location.component';
import { LocationService } from './location/location.service';
import { LowerNavComponent } from './navigation/lower-nav.component';
import { NavigateDataService } from './navigate-data.service';
import { PrintButtonComponent } from './resource/user-action/print-button.component';
import { ResourceCardComponent } from './resource/resource-card/resource-card.component';
import { ResourceCardDetailComponent } from './resource/resource-card-detail/resource-card-detail.component';
import { RemoveButtonComponent } from './resource/user-action/remove-button/remove-button.component';
import { SaveButtonComponent } from './resource/user-action/save-button/save-button.component';
import { SearchComponent } from './search/search.component';
import { SearchFilterComponent } from './search/search-filter/search-filter.component';
import { SearchResultsComponent } from './search/search-results/search-results.component';
import { SearchService } from './search/search.service';
import { ServiceOrgSidebarComponent } from './sidebars/service-org-sidebar.component';
import { ShareButtonComponent } from './resource/user-action/share-button/share-button.component';
import { UpperNavComponent } from './navigation/upper-nav.component';
import { WebResourceComponent } from './search/search-results/web-resource/web-resource.component';
import { CuratedResourceService } from './search/search-results/curated-resource/curated-resource.service';
import { SearchFilterPipe } from './search/search-filter.pipe';
import { BreadcrumbComponent } from '../topics-resources/breadcrumb/breadcrumb.component';
import { MapResultsComponent } from './sidebars/map-results.component';
import { MapResultsService } from './sidebars/map-results.service';
import { PaginationComponent } from './pagination/pagination.component';
import { PaginationService } from './search/pagination.service';
import { ActionPlansComponent } from './resource/resource-type/action-plan/action-plans.component';
import { UserActionSidebarComponent } from './sidebars/user-action-sidebar.component';
import { SettingButtonComponent } from './resource/user-action/setting-button.component';
import { ServiceOrgService } from './sidebars/service-org.service';
import { ArrayUtilityService } from './array-utility.service';
import { HelplineComponent } from './helpline/helpline.component';
import { ButtonSelectedDirective } from './styling/button-selected.directive';
import { EventUtilityService } from './event-utility.service';
import { ArticlesComponent } from './resource/resource-type/articles/articles.component';
import { OrganizationsComponent } from './resource/resource-type/organizations/organizations.component';
import { VideosComponent } from './resource/resource-type/videos/videos.component';

@NgModule({
  imports: [
    CommonModule,
    AppRoutingModule,
    FormsModule,
    AccordionModule.forRoot(),
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    CarouselModule.forRoot()
  ],
  declarations: [
    ActionPlansComponent,
    ChatbotComponent,
    CuratedResourceComponent,
    DownloadButtonComponent,
    FooterComponent,
    GuidedAssistantSidebarComponent,
    LanguageComponent,
    LocationComponent,
    LowerNavComponent,
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
    ButtonSelectedDirective,
    ArticlesComponent,
    OrganizationsComponent,
    VideosComponent
  ],
  exports: [
    ActionPlansComponent,
    ChatbotComponent,
    CuratedResourceComponent,
    DownloadButtonComponent,
    FooterComponent,
    GuidedAssistantSidebarComponent,
    LanguageComponent,
    LocationComponent,
    LowerNavComponent,
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
    HelplineComponent
  ],
  providers: [
    CuratedResourceService,
    LocationService,
    NavigateDataService,
    SearchService,
    MapResultsService,
    PaginationService,
    ServiceOrgService,
    ArrayUtilityService,
    EventUtilityService
  ]
})
export class SharedModule { }
