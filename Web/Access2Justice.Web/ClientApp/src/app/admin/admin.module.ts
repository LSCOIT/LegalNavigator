import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminRoutingModule } from './admin-routing.module';
import { BsDropdownModule } from 'ngx-bootstrap';
import { AdminComponent } from './admin/admin.component';
import { PrivacyPromiseAdminComponent } from './privacy-promise/privacy-promise-admin.component';
import { AdminService } from './admin.service';
import { AboutAdminComponent } from './about/about-admin.component';
import { AdminAuthGuard } from './admin-auth/admin-auth.guard';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { UploadCuratedExperienceTemplateComponent } from './upload-curated-experience-template/upload-curated-experience-template.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot()
  ],
  declarations: [
    AdminComponent,
    PrivacyPromiseAdminComponent,
    AboutAdminComponent,
    AdminDashboardComponent,
    UploadCuratedExperienceTemplateComponent
  ],
  exports: [
    AdminComponent,
    PrivacyPromiseAdminComponent,
    UploadCuratedExperienceTemplateComponent
  ],
  providers: [
    AdminService,
    AdminAuthGuard
  ]
})
export class AdminModule { }
