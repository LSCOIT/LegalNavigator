import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app-routing.module';
import { AdminComponent } from './admin.component';
import { PrivacyPromiseAdminComponent } from './privacy-promise/privacy-promise-admin.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    AppRoutingModule
  ],
  declarations: [
    AdminComponent,
    PrivacyPromiseAdminComponent
  ],
  exports: [
    AdminComponent,
    PrivacyPromiseAdminComponent
  ]
})
export class AdminModule { }
