import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from '../app-routing.module';
import { BsDropdownModule } from 'ngx-bootstrap';
import { AdminComponent } from './admin.component';
import { PrivacyPromiseAdminComponent } from './privacy-promise/privacy-promise-admin.component';
import { AdminService } from './admin.service';
import { AboutAdminComponent } from './about/about-admin.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot()
  ],
  declarations: [
    AdminComponent,
    PrivacyPromiseAdminComponent,
    AboutAdminComponent
  ],
  exports: [
    AdminComponent,
    PrivacyPromiseAdminComponent
  ],
  providers: [
    AdminService
  ]
})
export class AdminModule { }
