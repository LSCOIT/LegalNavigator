import { Component, OnInit, Optional } from '@angular/core';
import { TabDirective } from "ngx-bootstrap";

import { PersonalizedPlanService } from '../../guided-assistant/personalized-plan/personalized-plan.service';


@Component({
  selector: 'app-incoming-resources',
  templateUrl: './incoming-resources.component.html',
  styleUrls: ['./incoming-resources.component.scss']
})
export class IncomingResourcesComponent implements OnInit {

  constructor(
    private personalizedPlanService: PersonalizedPlanService,
    @Optional() private tab: TabDirective,
  ) { }

  private getIncomingResources(): void {
    this.personalizedPlanService.getPersonalizedResources('incoming-resources').subscribe(personalizedResources => {
      console.log(personalizedResources);
      // this.personalizedResources = personalizedResources;
    });
  }

  ngOnInit() {
    if (this.tab) {
      this.tab.select.subscribe(() => this.getIncomingResources());
    } else {
      this.getIncomingResources();
    }
  }

}
