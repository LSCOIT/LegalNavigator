import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '../app-routing.module';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap';
import { NavigateDataService } from './navigate-data.service';

import { ChatbotComponent } from './chatbot/chatbot.component';
import { DownloadButtonComponent } from './resource/user-action/download-button.component';
import { FooterComponent } from './footer/footer.component';
import { GuidedAssistantSidebarComponent } from './sidebars/guided-assistant-sidebar.component';
import { LowerNavComponent } from './navigation/lower-nav.component';
import { PrintButtonComponent } from './resource/user-action/print-button.component';
import { ResourceCardComponent } from './resource/resource-card/resource-card.component';
import { ResourceCardDetailComponent } from './resource/resource-card-detail/resource-card-detail.component';
import { RemoveButtonComponent } from './resource/user-action/remove-button.component';
import { SaveButtonComponent } from './resource/user-action/save-button.component';
import { SearchComponent } from './search/search.component';
import { SearchFilterComponent } from './search/search-filter.component';
import { SearchResultsComponent } from './search/search-results.component';
import { ServiceOrgSidebarComponent } from './sidebars/service-org-sidebar.component';
import { ShareButtonComponent } from './resource/user-action/share-button.component';
import { UpperNavComponent } from './navigation/upper-nav.component';



@NgModule({
  imports: [
    CommonModule,
    AppRoutingModule,
    FormsModule,
    BsDropdownModule.forRoot()
  ],
  declarations: [
    ChatbotComponent,
    DownloadButtonComponent,
    FooterComponent,
    GuidedAssistantSidebarComponent,
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
    UpperNavComponent
  ],
  exports: [
    ChatbotComponent,
    DownloadButtonComponent,
    FooterComponent,
    GuidedAssistantSidebarComponent,
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
    UpperNavComponent
  ],
  providers: [
    NavigateDataService
  ]
})
export class SharedModule { }
