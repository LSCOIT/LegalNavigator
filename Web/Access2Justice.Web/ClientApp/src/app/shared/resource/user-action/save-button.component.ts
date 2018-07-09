import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { PersonalizedPlanService } from '../../../guided-assistant/personalized-plan/personalized-plan.service';
import { Resources } from '../../../guided-assistant/personalized-plan/personalized-plan';

@Component({
  selector: 'app-save-button',
  template: `
<img src="./assets/images/small-icons/save.svg" class="nav-icon" aria-hidden="true" (click) ="savePlanResources()" />
<span>Save to profile</span>
 `
})
export class SaveButtonComponent implements OnInit {
  resources: Resources;
  @Input() savedFrom: string;

  constructor(private router: Router,
    private activeRoute: ActivatedRoute,
    private personalizedPlanService: PersonalizedPlanService) { }

  savePlanResources(): void {
    this.resources = { url: '', itemId: '' };
    this.resources.url = this.router.url;
    if (this.activeRoute.snapshot.params['topic']) {
      this.resources.itemId = this.activeRoute.snapshot.params['topic'];
    } else if (this.activeRoute.snapshot.params['id']) {
      this.resources.itemId = this.activeRoute.snapshot.params['id'];
    }
    this.personalizedPlanService.saveResourcesToSession(this.resources);
  }

  ngOnInit() {
  }

}
