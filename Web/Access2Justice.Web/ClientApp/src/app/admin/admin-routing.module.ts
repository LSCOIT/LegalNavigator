import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './admin/admin.component';
import { AboutAdminComponent } from './about/about-admin.component';
import { PrivacyPromiseAdminComponent } from './privacy-promise/privacy-promise-admin.component';

const adminRoutes: Routes = [
  {
    path: 'admin',
    component: AdminComponent,
    children: [
      {
        path: '',
        children: [
          { path: 'about', component: AboutAdminComponent },
          { path: 'privacy', component: PrivacyPromiseAdminComponent },
          { path: '', component: AdminComponent }
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
