import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { AboutAdminComponent } from './about/about-admin.component';
import { PrivacyPromiseAdminComponent } from './privacy-promise/privacy-promise-admin.component';
import { AdminAuthGuard } from './admin-auth/admin-auth.guard';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { UploadCuratedExperienceTemplateComponent } from './upload-curated-experience-template/upload-curated-experience-template.component';
import { CuratedExperienceAuthGuard } from './curated-experience-auth/curated-experience-auth.guard';

const adminRoutes: Routes = [
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AdminAuthGuard],
    children: [
      {
        path: '',
        children: [
          { path: 'about', component: AboutAdminComponent },
          { path: 'privacy', component: PrivacyPromiseAdminComponent },
          {
            path: 'curated-experience',
            component: UploadCuratedExperienceTemplateComponent,
            canActivate: [CuratedExperienceAuthGuard]
          },
          { path: '', component: AdminDashboardComponent }
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(adminRoutes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
