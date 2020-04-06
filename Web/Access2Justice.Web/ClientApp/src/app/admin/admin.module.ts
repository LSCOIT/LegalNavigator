import { CommonModule } from "@angular/common";
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BsDropdownModule, TabsModule, PaginationModule } from "ngx-bootstrap";
import { NavigateDataService } from "../common/services/navigate-data.service";
import { AboutTemplateComponent } from "./about/about-template.component";
import { AdminAuthGuard } from "./admin-auth/admin-auth.guard";
import { AdminDashboardComponent } from "./admin-dashboard/admin-dashboard.component";
import { AdminRoutingModule } from "./admin-routing.module";
import { AdminService } from "./admin.service";
import { AdminComponent } from "./admin/admin.component";
import { NgxEditorModule } from "ngx-editor";
import { CuratedExperienceTemplateComponent } from "./curated-experience/curated-experience-template.component";
import { HelpFaqsTemplateComponent } from "./help-faqs/help-faqs-template.component";
import { HomeTemplateComponent } from "./home/home-template.component";
import { PersonalizedPlanTemplateComponent } from "./personalized-plan/personalized-plan-template.component";
import { PrivacyPromiseTemplateComponent } from "./privacy-promise/privacy-promise-template.component";

import { TooltipModule } from "ngx-bootstrap/tooltip";
import { CuratedExperienceListComponent } from "./curated-experience/curated-experiences-list.component";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    NgxEditorModule,
    TooltipModule.forRoot(),
    PaginationModule.forRoot(),
  ],
  declarations: [
    AdminComponent,
    PrivacyPromiseTemplateComponent,
    AboutTemplateComponent,
    AdminDashboardComponent,
    CuratedExperienceTemplateComponent,
    HomeTemplateComponent,
    HelpFaqsTemplateComponent,
    PersonalizedPlanTemplateComponent,
    CuratedExperienceListComponent,
  ],
  exports: [
    AdminComponent,
    PrivacyPromiseTemplateComponent,
    AboutTemplateComponent,
    CuratedExperienceTemplateComponent,
  ],
  providers: [AdminService, AdminAuthGuard, NavigateDataService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AdminModule {}
