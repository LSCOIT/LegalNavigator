import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  AccordionModule,
  BsDropdownModule,
  ButtonsModule,
  CarouselModule,
  CollapseModule,
  ModalModule,
  ProgressbarModule,
  TabsModule
} from 'ngx-bootstrap';
import { NgxSpinnerModule } from 'ngx-spinner';

import { SanitizePipe } from './pipes/sanitize.pipe';
import { SearchFilterPipe } from './pipes/search-filter.pipe';

@NgModule({
  imports: [],
  declarations: [
    SanitizePipe,
    SearchFilterPipe
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    AccordionModule,
    BsDropdownModule,
    CarouselModule,
    CollapseModule,
    ModalModule,
    ProgressbarModule,
    TabsModule,
    ButtonsModule,
    NgxSpinnerModule,

    SanitizePipe,
    SearchFilterPipe
  ]
})
export class SharedModule {
}
