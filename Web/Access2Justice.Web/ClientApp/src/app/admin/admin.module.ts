import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminRoutingModule } from './admin-routing.module';
import { BsDropdownModule, TabsModule } from 'ngx-bootstrap';
import { AdminComponent } from './admin/admin.component';
import { PrivacyPromiseTemplateComponent } from './privacy-promise/privacy-promise-template.component';
import { AdminService } from './admin.service';
import { AboutTemplateComponent } from './about/about-template.component';
import { AdminAuthGuard } from './admin-auth/admin-auth.guard';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { CuratedExperienceTemplateComponent } from './curated-experience/curated-experience-template.component';
import { NavigateDataService } from '../shared/navigate-data.service';
import { HomeTemplateComponent } from './home/home-template.component';
import { HelpFaqsTemplateComponent } from './help-faqs/help-faqs-template.component';
import { PersonalizedPlanTemplateComponent } from './personalized-plan/personalized-plan-template.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot()
  ],
  declarations: [
    AdminComponent,
    PrivacyPromiseTemplateComponent,
    AboutTemplateComponent,
    AdminDashboardComponent,
    CuratedExperienceTemplateComponent,
    HomeTemplateComponent,
    HelpFaqsTemplateComponent,
    PersonalizedPlanTemplateComponent
  ],
  exports: [
    AdminComponent,
    PrivacyPromiseTemplateComponent,
    AboutTemplateComponent,
    CuratedExperienceTemplateComponent
  ],
  providers: [
    AdminService,
    AdminAuthGuard,
    NavigateDataService
  ]
})
export class AdminModule { }
