import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AboutTemplateComponent } from "./about/about-template.component";
import { AdminAuthGuard } from "./admin-auth/admin-auth.guard";
import { AdminDashboardComponent } from "./admin-dashboard/admin-dashboard.component";
import { AdminComponent } from "./admin/admin.component";
import { CuratedExperienceTemplateComponent } from "./curated-experience/curated-experience-template.component";
import { HelpFaqsTemplateComponent } from "./help-faqs/help-faqs-template.component";
import { HomeTemplateComponent } from "./home/home-template.component";
import { PersonalizedPlanTemplateComponent } from "./personalized-plan/personalized-plan-template.component";
import { PrivacyPromiseTemplateComponent } from "./privacy-promise/privacy-promise-template.component";
import { NgxEditorModule } from 'ngx-editor';
const adminRoutes: Routes = [
  {
    path: "admin",
    component: AdminComponent,
    canActivate: [AdminAuthGuard],
    children: [
      {
        path: "",
        children: [
          {
            path: "about",
            component: AboutTemplateComponent,
            canActivateChild: [AdminAuthGuard]
          },
          {
            path: "home",
            component: HomeTemplateComponent,
            canActivateChild: [AdminAuthGuard]
          },
          {
            path: "help",
            component: HelpFaqsTemplateComponent,
            canActivateChild: [AdminAuthGuard]
          },
          {
            path: "privacy",
            component: PrivacyPromiseTemplateComponent,
            canActivateChild: [AdminAuthGuard]
          },
          {
            path: "curated-experience",
            component: CuratedExperienceTemplateComponent,
            canActivateChild: [AdminAuthGuard]
          },
          {
            path: "plan",
            component: PersonalizedPlanTemplateComponent,
            canActivateChild: [AdminAuthGuard]
          },
          { path: "", component: AdminDashboardComponent }
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(adminRoutes), NgxEditorModule],
  exports: [RouterModule]
})
export class AdminRoutingModule {}
