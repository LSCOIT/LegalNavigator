import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { AboutTemplateComponent } from './about/about-template.component';
import { PrivacyPromiseTemplateComponent } from './privacy-promise/privacy-promise-template.component';
import { AdminAuthGuard } from './admin-auth/admin-auth.guard';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { CuratedExperienceTemplateComponent } from './curated-experience/curated-experience-template.component';

const adminRoutes: Routes = [
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AdminAuthGuard],
    children: [
      {
        path: '',
        children: [
          { path: 'about', component: AboutTemplateComponent, canActivateChild: [AdminAuthGuard] },
          { path: 'privacy', component: PrivacyPromiseTemplateComponent, canActivateChild: [AdminAuthGuard] },
          { path: 'curated-experience', component: CuratedExperienceTemplateComponent, canActivateChild: [AdminAuthGuard] },
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
