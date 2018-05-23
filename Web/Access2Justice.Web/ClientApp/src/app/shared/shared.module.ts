import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '../app-routing.module';

import { ChatbotComponent } from './chatbot.component';
import { FooterComponent } from './footer.component';
import { LowerNavComponent } from './lower-nav.component';
import { UpperNavComponent } from './upper-nav.component';

import { BsDropdownModule } from 'ngx-bootstrap';
import { LocationComponent } from './location/location.component';


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
    LocationComponent
  ],
  exports: [
    ChatbotComponent,
    FooterComponent,
    LowerNavComponent,
    UpperNavComponent
   ]
})
export class SharedModule { }
