import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Resources } from '../../../profile/personalized-plan/personalized-plan';
import { PersonalizedPlanService } from '../../../profile/personalized-plan/personalized-plan.service';

@Component({
  selector: 'app-save-button',
  template: `
<img src="./assets/images/small-icons/save.svg" class="nav-icon" aria-hidden="true" (click) ="savePlanResources()" />
<span>Save to profile</span>
 `
})
export class SaveButtonComponent implements OnInit {
  activeRoute: any;
  resources: Resources;
  @Input() savedFrom: string;

  constructor(private router: Router,
    private personalizedPlanService: PersonalizedPlanService) { }

  savePlanResources(): void {
    this.resources = { url: '', resourceType: '', itemId: '' };
    this.resources.url = this.router.url;
    this.resources.resourceType = "Organizations";
    this.resources.itemId = this.savedFrom; // "GUID";
    this.personalizedPlanService.saveResourcesToSession(this.resources);
    console.log("Book marked!!!");
  }

  ngOnInit() {
  }

}
