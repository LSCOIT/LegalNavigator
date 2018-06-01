import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '../app-routing.module';

import { ChatbotComponent } from './chatbot.component';
import { FooterComponent } from './footer.component';
import { LowerNavComponent } from './lower-nav.component';
import { UpperNavComponent } from './upper-nav.component';

import { BsDropdownModule } from 'ngx-bootstrap';
import { SearchCuratedExperienceComponent } from './search-curated-experience/search-curated-experience.component';
import { CuratedExperienceService } from './search-curated-experience/curatedexperience.service';


@NgModule({
  imports: [
    CommonModule,
    AppRoutingModule,
    BsDropdownModule.forRoot()
  ],
  declarations: [
    ChatbotComponent,
    FooterComponent,
    LowerNavComponent,
    UpperNavComponent,
    SearchCuratedExperienceComponent
  ],
  exports: [
    ChatbotComponent,
    FooterComponent,
    LowerNavComponent,
    UpperNavComponent
  ],
  providers: [CuratedExperienceService]
})
export class SharedModule { }
