import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '../app-routing.module';
import { FormsModule } from '@angular/forms';

import { ChatbotComponent } from './chatbot/chatbot.component';
import { FooterComponent } from './footer/footer.component';
import { GuidedAssistantSidebarComponent } from './sidebars/guided-assistant-sidebar.component';
import { LowerNavComponent } from './navigation/lower-nav.component';
import { ResourceCardComponent } from './resource/resource-card.component';
import { SearchComponent } from './search/search.component';
import { SearchResultsComponent } from './search/search-results.component';
import { ServiceOrgSidebarComponent } from './sidebars/service-org-sidebar.component';
import { UpperNavComponent } from './navigation/upper-nav.component';

import { BsDropdownModule } from 'ngx-bootstrap';
import { ActionPlanComponent } from './resource/resource-card-detail/resource-types/action-plan.component';
import { ResourceCardDetailComponent } from './resource/resource-card-detail/resource-card-detail.component';


@NgModule({
  imports: [
    CommonModule,
    AppRoutingModule,
    FormsModule,
    BsDropdownModule.forRoot()
  ],
  declarations: [
    ChatbotComponent,
    FooterComponent,
    GuidedAssistantSidebarComponent,
    LowerNavComponent,
    ResourceCardComponent,
    SearchComponent,
    SearchResultsComponent,
    ServiceOrgSidebarComponent,
    UpperNavComponent,
    ActionPlanComponent,
    ResourceCardDetailComponent
  ],
  exports: [
    ChatbotComponent,
    FooterComponent,
    GuidedAssistantSidebarComponent,
    LowerNavComponent,
    ResourceCardComponent,
    SearchComponent,
    SearchResultsComponent,
    ServiceOrgSidebarComponent,
    UpperNavComponent
   ]
})
export class SharedModule { }
